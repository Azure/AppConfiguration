using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;

namespace FunctionAppInProcess
{
    public class ShowMessage(IConfiguration configuration, IConfigurationRefresherProvider refresherProvider)
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IConfigurationRefresher _configurationRefresher = refresherProvider.Refreshers.First();

        [FunctionName("ShowMessage")]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Signal to refresh the configuration if the registered key(s) is modified.
            // This will be a no-op if the refresh interval has not elapsed.
            // The configuration is refreshed asynchronously without blocking the execution of the current function.
            _ = _configurationRefresher.TryRefreshAsync();

            // Read configuration data
            string key = "TestApp:Settings:Message";
            string message = _configuration[key];

            return new OkObjectResult(message ?? $"Please create a key-value with the key '{key}' in Azure App Configuration.");
        }
    }
}
