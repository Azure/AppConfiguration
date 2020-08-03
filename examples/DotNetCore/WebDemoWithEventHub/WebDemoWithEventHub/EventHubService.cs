using System;
using System.Linq;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;

namespace WebDemoWithEventHub
{
    public class EventHubService
    {
        private IConfigurationRefresher _refresher;

        private ILogger<EventHubService> _logger;

        public EventHubService(IConfiguration config, IConfigurationRefresherProvider providers, ILogger<EventHubService> logger)
        {
            _logger = logger;

            EventHubConnection eventHubConnection = config.GetSection("WebDemo:EventHubConnection").Get<EventHubConnection>();
            InitEventHubProcessor(eventHubConnection);

            _refresher = providers.Refreshers.FirstOrDefault(r => r is IConfigurationRefresher);
        }

        private void InitEventHubProcessor(EventHubConnection eventHubConnection)
        {
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

            var processorClient = new EventProcessorClient(checkpointStore: storageClient,
                                                       consumerGroup: consumerGroup,
                                                       connectionString: eventHubConnection.EventHubConnectionString,
                                                       eventHubName: eventHubConnection.EventHubName);

            processorClient.ProcessEventAsync += ProcessEventHandler;
            processorClient.ProcessErrorAsync += ProcessErrorHandler;
            processorClient.StartProcessing();
        }

        private Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            _logger.LogInformation("EventHub update received. Triggering cache invalidation.");

            //
            // Invalidate cache to force refresh.
            _refresher?.SetDirty(maxDelay: TimeSpan.FromSeconds(1));
            
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
