using Azure.Identity;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace WebDemoWithEventHub
{
    public class EventHubService
    {
        private Settings _settings;

        private EventHubConnection _eventHubConnection;

        private EventProcessorClient _processorClient;

        private IConfigRefresher _configRefresher;

        public EventHubService(IConfiguration config, IConfigRefresher configRefresher)
        {
            _configRefresher = configRefresher;

            _eventHubConnection = config.GetSection("WebDemo:Connection").Get<EventHubConnection>();
            _settings = config.GetSection("WebDemo:Settings").Get<Settings>();

            InitEventHubProcessor();
        }

        private void InitEventHubProcessor()
        {
            if (string.IsNullOrEmpty(_eventHubConnection.EventHubConnectionString))
            {
                throw new ArgumentNullException(nameof(_eventHubConnection.EventHubConnectionString));
            }

            if (string.IsNullOrEmpty(_eventHubConnection.EventHubName))
            {
                throw new ArgumentNullException(nameof(_eventHubConnection.EventHubName));
            }

            if (string.IsNullOrEmpty(_eventHubConnection.StorageConnectionString))
            {
                throw new ArgumentNullException(nameof(_eventHubConnection.StorageConnectionString));
            }

            if (string.IsNullOrEmpty(_eventHubConnection.StorageContainerName))
            {
                throw new ArgumentNullException(nameof(_eventHubConnection.StorageContainerName));
            }

            string consumerGroup = string.IsNullOrEmpty(_eventHubConnection.EventHubConsumerGroup)
                ? EventHubConsumerClient.DefaultConsumerGroupName
                : _eventHubConnection.EventHubConsumerGroup;

            var storageClient = new BlobContainerClient(connectionString: _eventHubConnection.StorageConnectionString,
                                                        blobContainerName: _eventHubConnection.StorageContainerName);

            _processorClient = new EventProcessorClient(checkpointStore: storageClient,
                                                       consumerGroup: consumerGroup,
                                                       connectionString: _eventHubConnection.EventHubConnectionString,
                                                       eventHubName: _eventHubConnection.EventHubName);

            _processorClient.ProcessEventAsync += ProcessEventHandler;
            _processorClient.ProcessErrorAsync += ProcessErrorHandler;
            _processorClient.StartProcessing();
        }

        private Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            _configRefresher.RefreshConfiguration();
            return Task.CompletedTask;
        }

        private Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine($"\n\nERROR: Partition: '{eventArgs.PartitionId}': an unhandled exception was encountered.\n\n");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
