using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionAppIsolated
{
    public class ReadQueuedMessage
    {
        private readonly ILogger<ReadQueuedMessage> _logger;

        public ReadQueuedMessage(ILogger<ReadQueuedMessage> logger)
        {
            _logger = logger;
        }

        // Queue triggered function with queue name defined in Azure App Configuration.
        // The queue name is stored with the key `TestApp:Storage:QueueName` in Azure App Configuration.
        // `AZURE_APPCONFIG_REFERENCE_QUEUENAME` is an app setting of the Function App referencing this key.
        // Learn more about App Configuration Reference:
        // https://learn.microsoft.com/azure/app-service/app-service-configuration-references
        [Function(nameof(ReadQueuedMessage))]
        public void Run([QueueTrigger(queueName: "%AZURE_APPCONFIG_REFERENCE_QUEUENAME%")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");
        }
    }
}
