using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Storage; // Namespace for CloudStorageAccount
using Microsoft.Azure.Storage.Queue; // Namespace for Queue storage types
using Azure;
using Azure.Data.AppConfiguration;
using Azure.Identity;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace BackupFromStorage
{
    public static class BackupFromStorageQueue
    {
        [FunctionName("BackupFromStorageQueue")]
        public static void Run([TimerTrigger("0 */10 * * * *")]TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

                // Connect to Azure App Configuration stores
                ConfigurationClient primaryAppConfigClient = new ConfigurationClient(new Uri(Environment.GetEnvironmentVariable("PrimaryStoreEndpoint")), new ManagedIdentityCredential());
                ConfigurationClient secondaryAppConfigClient = new ConfigurationClient(new Uri(Environment.GetEnvironmentVariable("SecondaryStoreEndpoint")), new ManagedIdentityCredential());

                // Connect to storage queue
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("StorageQueueConnectionString"));
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                CloudQueue queue = queueClient.GetQueueReference("eventgridqueue");
                CloudQueueMessage customMessage = new CloudQueueMessage($"Refreshing Azure App Configuration store at: {DateTime.Now}.");

                // Peek at the next message
                CloudQueueMessage peekedMessage = queue.PeekMessage();
                while (peekedMessage != null)
                {
                    var messageBody = peekedMessage.AsString;
                    try
                    {
                        JObject dataObject = JObject.Parse(messageBody);
                        string topic = dataObject.Property("topic")?.Value.ToString();
                        if (topic != null && topic == Environment.GetEnvironmentVariable("PrimaryStoreResourceId"))
                        {
                            // We have events that need to be processed
                            // Add custom message to the end of queue
                            queue.AddMessage(customMessage);
                            break;
                        }
                    }
                    catch (JsonReaderException)
                    {
                        // If its not a valid JSON, ignore the message 
                        // because it will be discarded from the queue
                    }
                    log.LogInformation($"Deleting message from queue: {messageBody}");
                    queue.DeleteMessage(queue.GetMessage());
                    peekedMessage = queue.PeekMessage();
                }

                // return if there are no messages in the queue
                if (peekedMessage == null)
                {
                    log.LogInformation($"No events received for processing.");
                    return;
                }

                // Read all settings from Primary Store and create local copy
                IEnumerable<ConfigurationSetting> primaryStoreSettings = primaryAppConfigClient.GetConfigurationSettings(new SettingSelector());
                IDictionary<Tuple<string, string>, ConfigurationSetting> primaryStoreLocalSettings = new Dictionary<Tuple<string, string>, ConfigurationSetting>();
                foreach (ConfigurationSetting setting in primaryStoreSettings)
                {
                    var keyLabel = new Tuple<string, string>(setting.Key, setting.Label);
                    primaryStoreLocalSettings.Add(keyLabel, setting);
                }

                // Read all settings from secondary store and create local copy
                IEnumerable<ConfigurationSetting> secondaryStoreSettings = secondaryAppConfigClient.GetConfigurationSettings(new SettingSelector());
                IDictionary<Tuple<string, string>, ConfigurationSetting> secondaryStoreLocalSettings = new Dictionary<Tuple<string, string>, ConfigurationSetting>();
                foreach (ConfigurationSetting setting in secondaryStoreSettings)
                {
                    var keyLabel = new Tuple<string, string>(setting.Key, setting.Label);
                    secondaryStoreLocalSettings.Add(keyLabel, setting);
                }

                foreach (ConfigurationSetting setting in primaryStoreSettings)
                {
                    var keyLabel = new Tuple<string, string>(setting.Key, setting.Label);
                    if (secondaryStoreLocalSettings.ContainsKey(keyLabel))
                    {
                        if (setting.ETag != secondaryStoreLocalSettings[keyLabel].ETag)
                        {
                            // Write updated setting to secondary store
                            secondaryAppConfigClient.SetConfigurationSetting(setting);
                            log.LogDebug($"Successfully updated key: {setting.Key} label: {setting.Label}");
                        }
                        primaryStoreLocalSettings.Remove(keyLabel);
                        secondaryStoreLocalSettings.Remove(keyLabel);
                    }
                }

                // If primaryStoreLocalSettings has any elements, they are new keys that need to be added to secondary store
                foreach (var kv in primaryStoreLocalSettings)
                {
                    secondaryAppConfigClient.SetConfigurationSetting(kv.Value);
                    log.LogDebug($"Successfully added key: {kv.Key.Item1} label: {kv.Key.Item2} to secondary store.");
                }

                // If secondaryStoreLocalSettings has any elements, they are to be deleted from secondary store
                foreach (var kv in secondaryStoreLocalSettings)
                {
                    secondaryAppConfigClient.DeleteConfigurationSetting(kv.Value);
                    log.LogDebug($"Successfully deleted key: {kv.Key.Item1} label: {kv.Key.Item2} from secondary store.");
                }

                // Delete processed events from the front of storage queue
                // until you reach the custom message added earlier
                peekedMessage = queue.PeekMessage();
                while (peekedMessage != null)
                {
                    var messageBody = peekedMessage.AsString;
                    log.LogInformation($"Deleting message from queue: {messageBody}");
                    queue.DeleteMessage(queue.GetMessage());
                    if (messageBody == customMessage.AsString)
                    {
                        break;
                    }
                    peekedMessage = queue.PeekMessage();
                }
            }
            catch (RequestFailedException exception)
            {
                log.LogError($"Service request failed from the server.\nStatus: {exception.Status}\nError Message: {exception.Message}");
            }
            catch (AggregateException exception) when ((exception as AggregateException)?.InnerExceptions?.All(ex => ex is RequestFailedException) ?? false)
            {
                log.LogError($"Service request failed from the server with the following exceptions:");
                foreach (RequestFailedException ex in exception.InnerExceptions)
                {
                    log.LogError($"\nStatus: {ex.Status}\nError Message: {ex.Message}");
                }
            }
        }
    }
}
