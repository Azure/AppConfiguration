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
    public class EventHubService : ISettingsProvider
    {
        private Settings settings;

        private IConfiguration configuration;

        private EventHubConnection eventHubConnection;

        private EventProcessorClient processorClient;

        private IConfigurationRefresher refresher;

        public EventHubService(IConfiguration config, IOptions<Settings> options)
        {
            var builder = new ConfigurationBuilder();
            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(config["AppConfigConnectionString"])
                       .ConfigureKeyVault(kv =>
                       {
                           kv.SetCredential(new DefaultAzureCredential());
                       })
                       .Select(keyFilter: "WebDemo:*")
                       .ConfigureRefresh((refreshOptions) =>
                       {
                            refreshOptions.Register(key: "WebDemo:Settings", refreshAll: true);
                       });

                refresher = options.GetRefresher();
            });

            configuration = builder.Build();

            eventHubConnection = configuration.GetSection("WebDemo:Connection").Get<EventHubConnection>();
            settings = configuration.GetSection("WebDemo:Settings").Get<Settings>();

            InitEventHubProcessor();
        }

        public Settings GetSettings() => settings;

        private void InitEventHubProcessor()
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

            processorClient = new EventProcessorClient(checkpointStore: storageClient,
                                                       consumerGroup: consumerGroup,
                                                       connectionString: eventHubConnection.EventHubConnectionString,
                                                       eventHubName: eventHubConnection.EventHubName);

            processorClient.ProcessEventAsync += ProcessEventHandler;
            processorClient.ProcessErrorAsync += ProcessErrorHandler;
            processorClient.StartProcessing();
        }

        private async Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            await refresher.TryRefreshAsync();
            settings = configuration.GetSection("WebDemo:Settings").Get<Settings>();
        }

        private Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine($"\n\nERROR: Partition: '{eventArgs.PartitionId}': an unhandled exception was encountered.\n\n");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
