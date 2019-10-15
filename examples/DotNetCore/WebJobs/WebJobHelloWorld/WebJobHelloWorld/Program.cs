using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace WebJobHelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var hostBuilder = new HostBuilder();
            hostBuilder.ConfigureHostConfiguration((config) =>
            {
                LoadConfiguration(config);
            });
            hostBuilder.ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices();
                b.AddAzureStorage();
            });
            hostBuilder.ConfigureLogging((context, b) =>
            {
                b.AddConsole();
            });

            var host = hostBuilder.Build();
            using (host)
            {
                host.Run();
            }
        }

        private static void LoadConfiguration(IConfigurationBuilder configBuilder)
        {
            //
            // This application attempts to connect to Azure App Configuration to retrieve Azure Blob Storage name, and Queue Name for the Azure Web Job.
            // It reads the ConnectionString for the App Configuration Service from environment variables.
            configBuilder.AddEnvironmentVariables();

            var config = configBuilder.Build();
            if (string.IsNullOrEmpty(config["ConnectionString"]))
            {
                throw new ArgumentNullException("Please set the 'ConnectionString' environment variable to a valid Azure App Configuration connection string and re-run this example.");
            }

            configBuilder.AddAzureAppConfiguration(options =>
            {
                options.Connect(config["ConnectionString"])
                    .Use("WebJob:*")
                    .TrimKeyPrefix("WebJob:");
            });

            config = configBuilder.Build();

            if (string.IsNullOrEmpty(config["AzureWebJobsStorage"]))
            {
                throw new ArgumentNullException("AzureWebJobsStorage not found.");
            }
            else if (string.IsNullOrEmpty(config["QueueName"]))
            {
                throw new ArgumentNullException("QueueName not found.");
            }
        }
    }
}
