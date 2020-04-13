namespace SimpleAppConfigEventHub
{
    public class EventHubConnection
    {
        public string EventHubConnectionString { get; set; }

        public string EventHubName { get; set; }

        public string BlobStorageConnectionString { get; set; }

        public string BlobStorageContainerName { get; set; }

        public string EventHubConsumerGroup { get; set; }
        
        public string AppConfigConnectionString { get; set; }
    }
}
