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
            // If these are present in appsettings.json or in environment variables, it reads those instead.
            var localConfigBuilder = new ConfigurationBuilder();
            localConfigBuilder.AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            var localConfiguration = localConfigBuilder.Build();

            //
            // If "WebJob" settings are present in local configuration, use that.
            if (!string.IsNullOrEmpty(localConfiguration["AzureWebJobsStorage"]) && !string.IsNullOrEmpty(localConfiguration["QueueName"]))
            {
                configBuilder = localConfigBuilder;
                return;
            }

            if (string.IsNullOrEmpty(localConfiguration["ConnectionString"]))
            {
                Console.WriteLine("'ConnectionString' is null. Cannot connecto Azure App Config to retrieve necessary parameters for AzureWebJob.");
                throw new ArgumentNullException();
            }

            //
            // Load config from Azure App Config.
            configBuilder.AddAzureAppConfiguration(options =>
            {
                options.Connect(localConfiguration["ConnectionString"])
                    .Use("WebJob:*")
                    .TrimKeyPrefix("WebJob:");
            });
        }
    }
}
