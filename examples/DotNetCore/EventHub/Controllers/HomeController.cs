using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SimpleAppConfigEventHub.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Newtonsoft.Json;
using Azure.Messaging.EventHubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace SimpleAppConfigEventHub.Controllers
{
    public class HomeController : Controller
    {
        public Settings Settings { get; }

        private EventHubConnection _eventHubConnection;

        private EventProcessorClient _processorClient = null;

        public HomeController(IOptions<Settings> options, IOptions<EventHubConnection> eventHubConnection)
        {
            Settings = options.Value;
            _eventHubConnection = eventHubConnection.Value;

            InitEventHubProcessor();

            _processorClient.ProcessEventAsync += ProcessEventHandler;
            _processorClient.ProcessErrorAsync += ProcessErrorHandler;

            _processorClient.StartProcessing();
        }

        public IActionResult Index()
        {
            ViewData["Messages"] = Settings?.Messages;
            ViewData["FontColor"] = Settings?.FontColor;
            ViewData["FontSize"] = Settings?.FontSize;
            ViewData["BackgroundColor"] = Settings?.BackgroundColor;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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

        private string ReadFromConfigStore(string key, string label)
        {
            if (string.IsNullOrWhiteSpace(label))
            {
                label = LabelFilter.Null;
            }

            var builder = new ConfigurationBuilder();
            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(_eventHubConnection.AppConfigConnectionString)
                        .Select(key, label);
            });

            IConfiguration configuration = builder.Build();
            return configuration[key];
        }

        private Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            string rawEvent = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());
            IEnumerable<EventContract> events = JsonConvert.DeserializeObject<List<EventContract>>(rawEvent);

            foreach (var e in events)
            {
                var key = e.Data.Key;
                var label = e.Data.Label;

                if (string.Equals(key, "Messages", StringComparison.InvariantCultureIgnoreCase))
                {
                    var value = ReadFromConfigStore(key, label);
                    Settings.Messages = value;
                }
                else if (string.Equals(key, "FontSize", StringComparison.InvariantCultureIgnoreCase))
                {
                    var value = ReadFromConfigStore(key, label);
                    Settings.FontSize = Int32.Parse(value);
                }
                else if (string.Equals(key, "FontColor", StringComparison.InvariantCultureIgnoreCase))
                {
                    var value = ReadFromConfigStore(key, label);
                    Settings.FontColor = value;
                }
                else if (string.Equals(key, "BackgroundColor", StringComparison.InvariantCultureIgnoreCase))
                {
                    var value = ReadFromConfigStore(key, label);
                    Settings.BackgroundColor = value;
                }
            }

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
