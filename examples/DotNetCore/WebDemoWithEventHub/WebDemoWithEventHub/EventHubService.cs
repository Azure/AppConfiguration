using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace WebDemoWithEventHub
{
    public class EventHubService
    {
        IConfigurationRoot _configRoot;

        public EventHubService(IConfiguration config)
        {
            EventHubConnection eventHubConnection = config.GetSection("WebDemo:Connection").Get<EventHubConnection>();
            InitEventHubProcessor(eventHubConnection);
            if (config is IConfigurationRoot)
            {
                _configRoot = (IConfigurationRoot)config;
            }            
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
            _configRoot?.Reload();
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
