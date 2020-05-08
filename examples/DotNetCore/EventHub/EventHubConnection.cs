namespace SimpleAppConfigEventHub
{
    public class EventHubConnection
    {
        /// <summary>
        /// Connection String for Azure Event Hub instance.
        /// </summary>
        public string EventHubConnectionString { get; set; }

        /// <summary>
        /// Name of the Event Hub within the Event Hub namespace.
        /// </summary>
        public string EventHubName { get; set; }

        /// <summary>
        /// Connection String for the Azure Blob Storage.
        /// </summary>
        public string BlobStorageConnectionString { get; set; }

        /// <summary>
        /// Name of the Storage Container.
        /// </summary>
        public string BlobStorageContainerName { get; set; }

        /// <summary>
        /// Name of the consumer group to connect to.
        /// By default, a default consumer group is created for Event Hub.
        /// More can be created as needed.
        /// </summary>
        public string EventHubConsumerGroup { get; set; }
        
        /// <summary>
        /// Connection String for the Azure App Configuration instance.
        /// </summary>
        public string AppConfigConnectionString { get; set; }
    }
}
