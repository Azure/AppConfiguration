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
        static readonly MyConfig _myConfig = new MyConfig();

        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Configuration will be refreshed if the cache is expired and the 'Sentinel' key is modified.
            // Remove the 'await' operator if the configuration is preferred to be refreshed without blocking.
            await _myConfig.ConfigurationRefresher.Refresh();

            string keyName = "TestApp:Settings:Message";
            string message = _myConfig.Configuration[keyName];
            
            return message != null
                ? (ActionResult)new OkObjectResult(message)
                : new BadRequestObjectResult($"Please pass a message from App Configuration with key name '{keyName}'");
        }
    }

    class MyConfig
    {
        public IConfiguration Configuration { set; get; }
        public IConfigurationRefresher ConfigurationRefresher { set; get; }

        public MyConfig()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(Environment.GetEnvironmentVariable("ConnectionString"))
                       // Reload all configuration if the 'Sentinel' key is modified and set cache expiration to 1 minute.
                       .ConfigureRefresh(refreshOptions =>
                            refreshOptions.Register("Sentinel", refreshAll: true)
                                          .SetCacheExpiration(TimeSpan.FromSeconds(60))
                );
                ConfigurationRefresher = options.GetRefresher();
            });
            Configuration = builder.Build();
        }
    }
}
