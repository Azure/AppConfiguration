using System;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebDemoWithEventHub
{
    public class EventHubService
    {
        private IConfigurationRefresher _configurationRefresher;

        private ILogger<EventHubService> _logger;

        private EventProcessorClient _eventProcessorClient;

        public EventHubService(IOptions<EventHubConnection> eventHubConnection, IConfigurationRefresherProvider refresherProvider, ILogger<EventHubService> logger)
        {
            _logger = logger;
            InitEventHubProcessor(eventHubConnection.Value);
            _configurationRefresher = refresherProvider.Refreshers.FirstOrDefault();
        }

        private void InitEventHubProcessor(EventHubConnection eventHubConnection)
        {
            if (eventHubConnection == null)
            {
                throw new ArgumentNullException(nameof(eventHubConnection));
            }

            if (string.IsNullOrEmpty(eventHubConnection.EventHubConnectionString))
            {
                throw new ArgumentNullException(nameof(eventHubConnection.EventHubConnectionString));
            }

            if (string.IsNullOrEmpty(eventHubConnection.EventHubName))
            {
                throw new ArgumentNullException(nameof(eventHubConnection.EventHubName));
            }

            if (string.IsNullOrEmpty(eventHubConnection.StorageConnectionString))
            {
                throw new ArgumentNullException(nameof(eventHubConnection.StorageConnectionString));
            }

            if (string.IsNullOrEmpty(eventHubConnection.StorageContainerName))
            {
                throw new ArgumentNullException(nameof(eventHubConnection.StorageContainerName));
            }

            string consumerGroup = string.IsNullOrEmpty(eventHubConnection.EventHubConsumerGroup)
                ? EventHubConsumerClient.DefaultConsumerGroupName
                : eventHubConnection.EventHubConsumerGroup;

            var storageClient = new BlobContainerClient(connectionString: eventHubConnection.StorageConnectionString,
                                                        blobContainerName: eventHubConnection.StorageContainerName);

            _eventProcessorClient = new EventProcessorClient(checkpointStore: storageClient,
                                                       consumerGroup: consumerGroup,
                                                       connectionString: eventHubConnection.EventHubConnectionString,
                                                       eventHubName: eventHubConnection.EventHubName);

            _eventProcessorClient.ProcessEventAsync += ProcessEventHandler;
            _eventProcessorClient.ProcessErrorAsync += ProcessErrorHandler;
            _eventProcessorClient.StartProcessing();
        }

        private Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            _logger.LogInformation("EventHub update received. Triggering cache invalidation.");

            // Build EventGridEvent from notification message
            EventGridEvent eventGridEvent = EventGridEvent.Parse(BinaryData.FromBytes(eventArgs.Data.EventBody));

            // Create PushNotification from eventGridEvent
            eventGridEvent.TryCreatePushNotification(out PushNotification pushNotification);

            // Prompt Configuration Refresh based on the PushNotification
            _configurationRefresher.ProcessPushNotification(pushNotification);
            
            return Task.CompletedTask;
        }

        private Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            _logger.LogError($"\n\nERROR: Partition: '{eventArgs.PartitionId}': an unhandled exception was encountered.\n\n");
            _logger.LogError(eventArgs.Exception.Message);

            return Task.CompletedTask;
        }
    }
}
