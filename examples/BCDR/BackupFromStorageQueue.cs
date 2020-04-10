/*
 * 
 * 
	1. We set up Event Grid events emitted by Azure App Configuration to be routed to an Azure Storage queue. Note that event order is not guaranteed by Event grid or storage queue.  
	2. We create a timer triggered Azure function to read contents of storage queue every <10> minutes. In this tutorial, we are using the same storage account for this Azure function and Storage Queue. 
	3. Azure function checks if there are any messages from event grid in the queue. Any other messages in the queue in invalid format will be discarded.
	4. If there are any messages in the queue, retrieve a batch of maximum 32 messages at a time for processing.
	5. For all retrieved messages, extract the key+label from event message and store them in a hash set. We need to keep track of any unique key+label that was modified or deleted.
	6. Fetch all settings from primary store. 
	7. Update only those settings in secondary store which have a corresponding event in the storage queue.
	8. Delete all settings that were present in storage queue but not in primary store.
    9. Delete all messages that we received in this batch from the storage queue.
    10. Repeat from step 4 if there are any messages remaining in storage queue.
 *
 *
 */

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Azure.Storage.Queues;
using Azure;
using Azure.Data.AppConfiguration;
using Azure.Identity;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Azure.Storage.Queues.Models;

namespace BackupFromStorage
{
    public static class BackupFromStorageQueue
    {
        private static HashSet<KeyLabel> ExtractKeyLabelsFromEvents(QueueMessage[] messages, ILogger log)
        {
            HashSet<KeyLabel> updatedKeyLabels = new HashSet<KeyLabel>(new KeyLabelComparer());
            foreach (QueueMessage message in messages)
            {
                try
                {
                    // Event grid will encode events in Base64 format before publishing to storage queue
                    // Decode the message before parsing it
                    string decodedMessage = Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageText));
                    try
                    {
                        JObject eventObject = JObject.Parse(decodedMessage);
                        string topic = eventObject.Property("topic")?.Value.ToString();
                        if (topic != null && topic == Environment.GetEnvironmentVariable("PrimaryStoreResourceId"))
                        {
                            JObject dataObject = JObject.Parse(eventObject.Property("data")?.Value.ToString());
                            string key = dataObject["key"].ToString();
                            string label = dataObject["label"]?.ToString();
                            updatedKeyLabels.Add(new KeyLabel(key, label));
                        }
                    }
                    catch (JsonReaderException)
                    {
                        // If its not a valid JSON, ignore the message 
                        log.LogInformation($"Queue message in invalid JSON format will be ignored.\nMessage: {decodedMessage}");
                    }
                }
                catch (FormatException)
                {
                    // If its not in valid base64 format, ignore the message 
                    log.LogInformation($"Queue message in invalid Base64 format will be ignored.\nMessage: {message.MessageText}");
                }
            }
            return updatedKeyLabels;
        }

        private static void BackupKeyValues(HashSet<KeyLabel> updatedKeyLabels, ILogger log)
        {
            // Connect to Azure App Configuration stores
            ConfigurationClient primaryAppConfigClient = new ConfigurationClient(new Uri(Environment.GetEnvironmentVariable("PrimaryStoreEndpoint")), new ManagedIdentityCredential());
            ConfigurationClient secondaryAppConfigClient = new ConfigurationClient(new Uri(Environment.GetEnvironmentVariable("SecondaryStoreEndpoint")), new ManagedIdentityCredential());

            // Read all settings from Primary Store and update secondary store
            foreach (ConfigurationSetting setting in primaryAppConfigClient.GetConfigurationSettings(new SettingSelector()))
            {
                KeyLabel primaryStoreKeyLabel = new KeyLabel(setting.Key, setting.Label);
                if(updatedKeyLabels.Contains(primaryStoreKeyLabel))
                {
                    // current setting retrieved from primary store needs to be updated in secondary store
                    secondaryAppConfigClient.SetConfigurationSetting(setting);
                    log.LogInformation($"Successfully updated key: {setting.Key} label: {setting.Label}");
                    updatedKeyLabels.Remove(primaryStoreKeyLabel);
                }
            }

            // Delete key labels still present in updatedKeyLabels from secondary store
            foreach (var keyLabel in updatedKeyLabels)
            {
                secondaryAppConfigClient.DeleteConfigurationSetting(keyLabel.Key, keyLabel.Label);
                log.LogInformation($"Successfully deleted key: {keyLabel.Key} label: {keyLabel.Label} from secondary store.");
            }
        }

        [FunctionName("BackupFromStorageQueue")]
        public static void Run([TimerTrigger("0 */10 * * * *")]TimerInfo myTimer, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
                
                // Create storage queue client
                QueueClient queueClient = new QueueClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "eventgridqueue");

                // Peek to see if there are events in the queue
                while (queueClient.PeekMessages().Value.Length > 0 )
                {
                    Response<QueueMessage[]> messages = queueClient.ReceiveMessages(maxMessages: 32);

                    // extract list of key+labels from event data
                    HashSet<KeyLabel> updatedKeyLabels = ExtractKeyLabelsFromEvents(messages.Value, log);

                    // If there are any valid App Configuration events, update secondary store
                    if(updatedKeyLabels.Count > 0)
                    {
                        BackupKeyValues(updatedKeyLabels, log);
                    }

                    // Delete this batch of events from storage queue
                    foreach (QueueMessage message in messages.Value)
                    {
                        log.LogInformation($"Deleting message: {message.MessageId}");
                        queueClient.DeleteMessage(message.MessageId, message.PopReceipt);
                    }
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

    public class KeyLabel
    {
        public string Key { get; set; }
        public string Label { get; set; }

        public KeyLabel(string key, string label)
        {
            Key = key;
            Label = label;
        }
    }

    public class KeyLabelComparer : IEqualityComparer<KeyLabel>
    {
        public bool Equals(KeyLabel x, KeyLabel y)
        {
            return string.Equals(x.Key, y.Key, StringComparison.InvariantCulture) 
                && string.Equals(x.Label, y.Label, StringComparison.InvariantCulture);
        }

        public int GetHashCode(KeyLabel obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}
