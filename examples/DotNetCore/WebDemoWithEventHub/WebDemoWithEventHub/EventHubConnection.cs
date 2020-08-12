namespace WebDemoWithEventHub
{
    public class EventHubConnection
    {
        /// <summary>
        /// ConnectionString for EventHub Instance.
        /// </summary>
        public string EventHubConnectionString { get; set; }

        /// <summary>
        /// Name of EventHub Namespace.
        /// </summary>
        public string EventHubNamespace { get; set; }

        /// <summary>
        /// Name of EventHub instance with the EventHub Namespace.
        /// </summary>
        public string EventHubName { get; set; }

        /// <summary>
        /// Name of the ConsumerGroup to connect to.
        /// By default, 1 consumer group is created is created per EventHub.
        /// More can be configured at creation or later.
        /// </summary>
        public string EventHubConsumerGroup { get; set; }

        /// <summary>
        /// ConnectionString for the Azure Blob Storage.
        /// </summary>
        public string StorageConnectionString { get; set; }

        /// <summary>
        /// Name of the storage container.
        /// </summary>
        public string StorageContainerName { get; set; }
    }
}
