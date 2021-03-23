using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public static class ReadQueuedMessage
    {
        // Queue triggered function with queue name defined in Azure App Configuration
        [FunctionName("ReadQueuedMessage")]
        public static void Run(
            [QueueTrigger(queueName: "%TestApp:Storage:QueueName%")] string myQueueItem, 
            ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
