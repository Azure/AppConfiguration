using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AzureFunctionPushRefresh
{
    public class PushRefreshFunction
    {
        private readonly Settings _settings;
        private readonly IConfigurationRefresher _configurationRefresher;

        public PushRefreshFunction(IOptionsSnapshot<Settings> settings, IConfigurationRefresher configurationRefresher)
        {
            _settings = settings.Value;
            _configurationRefresher = configurationRefresher;
        }

        [FunctionName("PushRefreshFunction")]
        public void PushRefresh([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation($"Received event with type {eventGridEvent.EventType} and time {eventGridEvent.EventTime}.");

            // Set the cache as dirty since the configuration in App Configuration has changed.
            // This will ensure that the next call to RefreshAsync or TryRefreshAsync would cause cached values to be revalidated.
            _configurationRefresher.SetDirty();
        }

        [FunctionName("ShowMessage")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Signal to refresh the configuration if the registered key(s) is modified.
            // This will be a no-op if the cache expiration time window is not reached.
            // The configuration is refreshed asynchronously without blocking the execution of the current function.
            _ = _configurationRefresher.TryRefreshAsync();

            string message = _settings.Message;

            return message != null
                ? (ActionResult)new OkObjectResult(message)
                : new BadRequestObjectResult($"Please create a key-value with the key 'TestApp:Settings:Message' in Azure App Configuration.");
        }
    }
}
