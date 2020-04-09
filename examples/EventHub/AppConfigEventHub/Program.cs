using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading;

namespace AppConfigEventHub
{
    class Program
    {
        static EventProcessorClient processor = null;

        static void Main(string[] _)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .AddEnvironmentVariables()
                .Build();

            string eventHubConnectionString = config["EventHubConnectionString"];
            string eventHubName = config["EventHubName"];
            string blobStorageConnectionString = config["BlobStorageConnectionString"];
            string blobContainerName = config["BlobStorageContainerName"];

            //
            // Check if the required configurations are available.
            if (string.IsNullOrEmpty(eventHubConnectionString))
            {
                Console.WriteLine("EventHubConnectionString not specified.");
                return;
            }

            if (string.IsNullOrEmpty(eventHubName))
            {
                Console.WriteLine("EventHubName not specified.");
                return;
            }

            if (string.IsNullOrEmpty(blobStorageConnectionString))
            {
                Console.WriteLine("BlobStorageConnectionString not specified.");
                return;
            }

            if (string.IsNullOrEmpty(blobContainerName))
            {
                Console.WriteLine("BlobStorageContainerName not specified.");
                return;
            }

            //
            // Read from the default consumer group. Can be modified to read from others.
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;

            //
            // Create client to read from blob storage.
            BlobContainerClient storageClient = new BlobContainerClient(blobStorageConnectionString, blobContainerName);

            //
            // Create EventHub Processor Client
            processor = new EventProcessorClient(storageClient, consumerGroup, eventHubConnectionString, eventHubName);

            //
            // Register event handlers for event received and error.
            processor.ProcessEventAsync += ProcessEventHandler;
            processor.ProcessErrorAsync += ProcessErrorHandler;

            var cts = new CancellationTokenSource();
            Run(cts.Token);

            Console.ReadKey();
            cts.Cancel();
        }

        static async Task Run(CancellationToken token)
        { 
            if (processor == null) return;

            do
            {
                await processor.StartProcessingAsync();
                await Task.Delay(TimeSpan.FromMilliseconds(100));
            } while (!token.IsCancellationRequested);

            await processor.StopProcessingAsync();
        }

        static Task ProcessEventHandler(ProcessEventArgs eventArgs)
        {
            //
            // Get the raw event data and deserialize it.
            string rawEvents = Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray());
            IEnumerable<EventContract> events = JsonConvert.DeserializeObject<List<EventContract>>(rawEvents);

            foreach (EventContract e in events)
            {
                var key = e.Data.Key;
                var label = e.Data.Label ?? "(no label)";
                var eType = e.EventType;
                var eTime = e.EventTime;
                Console.WriteLine($"Key: '{key}'; Label: '{label}'; Changed at: '{eTime}'; Type: '{eType}'\n");
            }

            return Task.CompletedTask;
        }

        static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
        {
            Console.WriteLine($"\n\nERROR: Partition: '{eventArgs.PartitionId}': an unhandled exception was encountered.\n\n");
            Console.WriteLine(eventArgs.Exception.Message);
            return Task.CompletedTask;
        }
    }
}
