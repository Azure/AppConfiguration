using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Newtonsoft.Json;

namespace SimpleAppConfigEventHub
{
    public class EventHubService : IEventHubService
    {
        public Settings Settings { get; private set; }

        private IConfiguration Configuration;

        private EventHubConnection _eventHubConnection;

        private EventProcessorClient _processorClient = null;

        private IConfigurationRefresher _refresher = null;

        private IEnumerable<string> _kvSelectors = null;

        public EventHubService(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder();

            _eventHubConnection = configuration.GetSection("EventHubConnection").Get<EventHubConnection>();
            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(configuration["EventHubConnection:AppConfigConnectionString"])
                       .Select(keyFilter: "Demo:Settings:*");
            });

            Configuration = builder.Build();
            Settings = Configuration.GetSection("Demo:Settings").Get<Settings>();
            _kvSelectors = (new string[]{
                "Demo:Settings:BackgroundColor",
                "Demo:Settings:FontColor",
                "Demo:Settings:FontSize",
                "Demo:Settings:Messages"}).OfType<string>().ToList();

            InitAppConfig();
            InitEventHubProcessor();

            _processorClient.ProcessEventAsync += ProcessEventHandler;
            _processorClient.ProcessErrorAsync += ProcessErrorHandler;

            _processorClient.StartProcessing();
        }
        
        private void InitAppConfig()
        {
            var builder = new ConfigurationBuilder();
            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(_eventHubConnection.AppConfigConnectionString)
                       .Select("Demo:Settings:*");

                _refresher = options.GetRefresher();
            });

            IConfiguration configuration = builder.Build();
        }

        private void InitEventHubProcessor()
        {
            string eventHubConnectionString = _eventHubConnection.EventHubConnectionString;
            string eventHubName = _eventHubConnection.EventHubName;
            string blobStorageConnectionString = _eventHubConnection.BlobStorageConnectionString;
            string blobContainerName = _eventHubConnection.BlobStorageContainerName;
            string consumerGroup = _eventHubConnection.EventHubConsumerGroup;

            //
            // Check if the required configurations are available.
            if (string.IsNullOrEmpty(eventHubConnectionString))
            {
                throw new ArgumentNullException(nameof(eventHubConnectionString));
            }

            if (string .IsNullOrEmpty(eventHubName))
            {
                throw new ArgumentNullException(nameof(eventHubName));
            }

            if (string.IsNullOrEmpty(blobStorageConnectionString))
            {
                throw new ArgumentNullException(nameof(blobStorageConnectionString));
            }
            
            if (string.IsNullOrEmpty(blobContainerName))
            {
                throw new ArgumentNullException(nameof(blobContainerName));
            }

            //
            // If no consumer group is provided, use default one.
            if (string.IsNullOrEmpty(consumerGroup))
            {
                consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
            }

            //
            // Create blob storage client.
            var storageClient = new BlobContainerClient(connectionString: blobStorageConnectionString,
                                                        blobContainerName: blobContainerName);
            
            _processorClient = new EventProcessorClient(checkpointStore: storageClient,
                                                        consumerGroup: consumerGroup,
                                                        connectionString: eventHubConnectionString,
                                                        eventHubName: eventHubName);
        }

        private async Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            string rawEvent = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());
            IEnumerable<KeyValueEvent> events = JsonConvert.DeserializeObject<List<KeyValueEvent>>(rawEvent);
            bool shouldRefresh = false;

            foreach (var e in events)
            {
                var key = e.Data.Key;
                if (IsWatchedKeyValue(key))
                {
                    shouldRefresh = true;
                    break;
                }
            }

            if (shouldRefresh)
            {
                await _refresher.RefreshAsync();
                Settings = Configuration.GetSection("Demo:Settings").Get<Settings>();
            }
        }

        private Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine($"\n\nERROR: Partition: '{eventArgs.PartitionId}': an unhandled exception was encountered.\n\n");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }

        private bool IsWatchedKeyValue(string key)
        {
            var kv = _kvSelectors.FirstOrDefault(k => string.Equals(k, key, StringComparison.InvariantCultureIgnoreCase));
            return kv != null;
        }
    }
}