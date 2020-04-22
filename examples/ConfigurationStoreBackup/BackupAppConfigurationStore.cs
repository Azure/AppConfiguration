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
        private const int maxMessagesToRead = 32;

        [FunctionName("BackupAppConfigurationStore")]
        public static void Run([TimerTrigger("0 */10 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            if (primaryAppConfigClient == null || secondaryAppConfigClient == null)
            {
                log.LogError($"Could not connect to App Configuration Store. Please validate the store endpoints that you have provided.");
                return;
            }
            if (queueClient == null)
            {
                log.LogError($"Could not connect to storage queue. Please validate the storage connection string and storage queue name.");
                return;
            }

            // Block the thread so that another function instance cannot be triggered while this is still processing.
            ProcessStorageQueueMessagesAsync(log).GetAwaiter().GetResult();
        }

        private static async Task ProcessStorageQueueMessagesAsync(ILogger log)
        {
            // Peek to see if there are events in the queue.
            while ((await queueClient.PeekMessagesAsync()).Value.Length > 0)
            {
                Response<QueueMessage[]> retrievedMessages = await queueClient.ReceiveMessagesAsync(maxMessagesToRead);

                // Extract list of key+labels from event data.
                HashSet<KeyLabel> updatedKeyLabels = ExtractKeyLabelsFromEvents(retrievedMessages.Value, log);

                // If there are any valid App Configuration events, update secondary store.
                if (updatedKeyLabels.Count > 0)
                {
                    bool isBackupSuccessful = await BackupKeyValuesAsync(updatedKeyLabels, log);
                    if (!isBackupSuccessful)
                    {
                        // Abort this function without deleting retrievedMessages from storage queue.
                        log.LogError($"App Configuration store backup is aborted. It will be attempted next time the function is triggered.");
                        break;
                    }
                }

                // Delete this batch of events from storage queue.
                foreach (QueueMessage message in retrievedMessages.Value)
                {
                    try
                    {
                        await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                        log.LogDebug($"Sucessfully deleted message from queue. Message ID: {message.MessageId}");
                    }
                    catch (RequestFailedException ex) when (
                        ex.ErrorCode == QueueErrorCode.PopReceiptMismatch ||
                        ex.ErrorCode == QueueErrorCode.MessageNotFound)
                    {
                        // We can continue processing the rest of the queue and safely ignore these exceptions.
                        log.LogDebug($"Failed to delete message from queue. Message ID: {message.MessageId}\nException: {ex}");
                    }
                }
            }
        }

        private static HashSet<KeyLabel> ExtractKeyLabelsFromEvents(QueueMessage[] retrievedMessages, ILogger log)
        {
            HashSet<KeyLabel> updatedKeyLabels = new HashSet<KeyLabel>();
            foreach (QueueMessage message in retrievedMessages)
            {
                string decodedMessage = string.Empty;
                try
                {
                    // Event grid will encode events in Base64 format before publishing to storage queue. Decode the message before parsing it.
                    decodedMessage = Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageText));
                    JToken dataObject = JObject.Parse(decodedMessage).Property("data")?.Value;
                    if (dataObject != null && dataObject["key"] != null)
                    {
                        string key = dataObject["key"].ToString();
                        string label = dataObject["label"]?.ToString();
                        updatedKeyLabels.Add(new KeyLabel(key, label));
                    }
                }
                catch (FormatException)
                {
                    log.LogInformation($"Queue message in invalid Base64 format is ignored.\nMessage: {message.MessageText}");
                }
                catch (JsonReaderException)
                {
                    log.LogInformation($"Queue message in invalid JSON format is ignored.\nMessage: {decodedMessage}");
                }
            }
            return updatedKeyLabels;
        }

        private static async Task<bool> BackupKeyValuesAsync(HashSet<KeyLabel> updatedKeyLabels, ILogger log)
        {
            bool isBackupSuccessful = false;
            try
            {
                // Read all settings from primary store and update secondary store.
                await foreach (ConfigurationSetting setting in primaryAppConfigClient.GetConfigurationSettingsAsync(new SettingSelector()))
                {
                    KeyLabel primaryStoreKeyLabel = new KeyLabel(setting.Key, setting.Label);
                    if (updatedKeyLabels.Contains(primaryStoreKeyLabel))
                    {
                        // Current setting retrieved from primary store needs to be updated in secondary store.
                        await secondaryAppConfigClient.SetConfigurationSettingAsync(setting);
                        log.LogInformation($"Successfully updated key: {setting.Key} label: {setting.Label}");
                        
                        updatedKeyLabels.Remove(primaryStoreKeyLabel);
                        if (updatedKeyLabels.Count == 0)
                        {
                            break;
                        }
                    }
                }

                // Delete key labels still present in updatedKeyLabels from secondary store.
                foreach (var keyLabel in updatedKeyLabels)
                {
                    await secondaryAppConfigClient.DeleteConfigurationSettingAsync(keyLabel.Key, keyLabel.Label);
                }
                isBackupSuccessful = true;
            }
            catch (RequestFailedException exception)
            {
                log.LogError($"Request to Azure App Configuration failed.\nStatus: {exception.Status}\nError Message: {exception.Message}\nStack Trace: {exception.StackTrace}");
            }
            catch (AggregateException exception) when ((exception as AggregateException)?.InnerExceptions?.All(ex => ex is RequestFailedException) ?? false)
            {
                StringBuilder sb = new StringBuilder();
                foreach (RequestFailedException ex in exception.InnerExceptions)
                {
                    sb.Append($"\nStatus: {ex.Status}\nError Message: {ex.Message}\nStack Trace: {ex.StackTrace}");
                }
                log.LogError($"Request to Azure App Configuration failed with the following exceptions: {sb}");
            }
            return isBackupSuccessful;
        }
    }
}
