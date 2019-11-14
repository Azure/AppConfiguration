using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;

namespace FunctionApp
{
    public static class Function1
    {
        // Use the static modifier to create a singleton instance of Configuraion. This avoids
        // reloading of configuration for every Azure Function call.
        // The configuration will be cached and can be refreshed based on customization.
        private static IConfiguration Configuration { set; get; }
        private static IConfigurationRefresher ConfigurationRefresher { set; get; }

        static Function1()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(Environment.GetEnvironmentVariable("ConnectionString"))
                       // Configure to reload all configuration if the 'Sentinel' key is modified and
                       // set cache expiration time window to 1 minute.
                       .ConfigureRefresh(refreshOptions =>
                            refreshOptions.Register("Sentinel", refreshAll: true)
                                          .SetCacheExpiration(TimeSpan.FromSeconds(60))
                );
                ConfigurationRefresher = options.GetRefresher();
            });
            Configuration = builder.Build();
        }

        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Signal to refresh the configuration if the 'Sentinel' key is modified. This will be no-op
            // if the cache expiration time window is not reached.
            // Remove the 'await' operator if the configuration is preferred to be refreshed without blocking.
            await ConfigurationRefresher.Refresh();

            string keyName = "TestApp:Settings:Message";
            string message = Configuration[keyName];
            
            return message != null
                ? (ActionResult)new OkObjectResult(message)
                : new BadRequestObjectResult($"Please create a key-value with the key '{keyName}' in App Configuration.");
        }
    }
}
