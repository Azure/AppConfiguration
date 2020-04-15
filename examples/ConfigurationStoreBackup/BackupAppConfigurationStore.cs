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

using Azure;
using Azure.Data.AppConfiguration;
using Azure.Identity;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationStoreBackup
{
    public static class BackupAppConfigurationStore
    {
        private static readonly ConfigurationClient primaryAppConfigClient = new ConfigurationClient(new Uri(Environment.GetEnvironmentVariable("PrimaryStoreEndpoint")), new ManagedIdentityCredential());
        private static readonly ConfigurationClient secondaryAppConfigClient = new ConfigurationClient(new Uri(Environment.GetEnvironmentVariable("SecondaryStoreEndpoint")), new ManagedIdentityCredential());
        private static readonly QueueClient queueClient = new QueueClient(Environment.GetEnvironmentVariable("AzureWebJobsStorage"), "eventgridqueue");
        private static readonly int maxMessagesToRead = 32;

        [FunctionName("BackupAppConfigurationStore")]
        public static void Run([TimerTrigger("0 */10 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (primaryAppConfigClient == null || secondaryAppConfigClient == null)
            {
                log.LogError($"Could not connect to App Configuration Store. Please validate the store endpoints that you have provided.");
                return;
            }

            // Block the thread so that another function instance cannot be triggered while this is still processing.
            ProcessStorageQueueMessagesAsync(log).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private static async Task ProcessStorageQueueMessagesAsync(ILogger log)
        {
            // Peek to see if there are events in the queue.
            while (await AnyMessageInQueueAsync())
            {
                Response<QueueMessage[]> retrievedMessages = await queueClient.ReceiveMessagesAsync(maxMessagesToRead).ConfigureAwait(false);

                // Extract list of key+labels from event data.
                HashSet<KeyLabel> updatedKeyLabels = ExtractKeyLabelsFromEvents(retrievedMessages.Value, log);

                // If there are any valid App Configuration events, update secondary store.
                if (updatedKeyLabels.Count > 0)
                {
                    bool backupCompleted = await BackupKeyValuesAsync(updatedKeyLabels, log).ConfigureAwait(false);
                    if (!backupCompleted)
                    {
                        // Abort this function without deleting retrievedMessages from storage queue.
                        log.LogError($"Backup failed because of error(s) returned by Azure App Configuration service.");
                        return;
                    }
                }

                // Delete this batch of events from storage queue.
                foreach (QueueMessage message in retrievedMessages.Value)
                {
                    log.LogDebug($"Deleting message: {message.MessageId}");
                    await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt).ConfigureAwait(false);
                }
            }
        }

        private static async Task<bool> AnyMessageInQueueAsync()
        {
            var peekedMessages = await queueClient.PeekMessagesAsync().ConfigureAwait(false);
            return peekedMessages.Value.Length > 0; 
        }

        private static HashSet<KeyLabel> ExtractKeyLabelsFromEvents(QueueMessage[] retrievedMessages, ILogger log)
        {
            HashSet<KeyLabel> updatedKeyLabels = new HashSet<KeyLabel>(new KeyLabelComparer());
            foreach (QueueMessage message in retrievedMessages)
            {
                string decodedMessage = string.Empty;
                try
                {
                    // Event grid will encode events in Base64 format before publishing to storage queue. Decode the message before parsing it.
                    decodedMessage = Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageText));
                    JToken dataObject = JObject.Parse(decodedMessage).Property("data")?.Value;
                    if (dataObject != null)
                    {
                        string key = dataObject["key"].ToString();
                        string label = dataObject["label"]?.ToString();
                        updatedKeyLabels.Add(new KeyLabel(key, label));
                    }
                }
                catch (FormatException)
                {
                    // If its not in valid base64 format, ignore the queue message. 
                    log.LogInformation($"Queue message in invalid Base64 format will be ignored.\nMessage: {message.MessageText}");
                }
                catch (JsonReaderException)
                {
                    // If its not a valid JSON, ignore the queue message. 
                    log.LogInformation($"Queue message in invalid JSON format will be ignored.\nMessage: {decodedMessage}");
                }
                catch (NullReferenceException)
                {
                    // If key is null, ignore the queue message. 
                    log.LogInformation($"Event data contains null 'key'. Queue message will be ignored.\nMessage: {decodedMessage}");
                }
            }
            return updatedKeyLabels;
        }

        private static async Task<bool> BackupKeyValuesAsync(HashSet<KeyLabel> updatedKeyLabels, ILogger log)
        {
            bool backupCompleted = false;
            try
            {
                // Read all settings from primary store and update secondary store.
                await foreach (ConfigurationSetting setting in primaryAppConfigClient.GetConfigurationSettingsAsync(new SettingSelector()).ConfigureAwait(false))
                {
                    KeyLabel primaryStoreKeyLabel = new KeyLabel(setting.Key, setting.Label);
                    if (updatedKeyLabels.Contains(primaryStoreKeyLabel))
                    {
                        // Current setting retrieved from primary store needs to be updated in secondary store.
                        await secondaryAppConfigClient.SetConfigurationSettingAsync(setting).ConfigureAwait(false);
                        log.LogInformation($"Successfully updated key: {setting.Key} label: {setting.Label}");
                        
                        updatedKeyLabels.Remove(primaryStoreKeyLabel);
                        if (updatedKeyLabels.Count == 0)
                        {
                            // If no more key labels were updated, we can stop processing.
                            break;
                        }
                    }
                }

                // Delete key labels still present in updatedKeyLabels from secondary store.
                foreach (var keyLabel in updatedKeyLabels)
                {
                    await secondaryAppConfigClient.DeleteConfigurationSettingAsync(keyLabel.Key, keyLabel.Label).ConfigureAwait(false);
                }
                backupCompleted = true;
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
            return backupCompleted;
        }
    }
}
