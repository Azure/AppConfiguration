using System;
using Azure.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace WebJobHelloWorld;

class Program
{
    static void Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureHostConfiguration(config =>
            {
                LoadConfiguration(config);
            })
            .ConfigureWebJobs(builder =>
            {
                builder.AddAzureStorageQueues();
            })
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
            })
            .Build();
            
        host.Run();
    }

    private static void LoadConfiguration(IConfigurationBuilder configBuilder)
    {
        //
        // This application attempts to connect to Azure App Configuration to retrieve Azure Blob Storage name, and Queue Name for the Azure Web Job.
        // It reads the Endpoint URI for the App Configuration Service from environment variables.
        configBuilder.AddEnvironmentVariables();

        var config = configBuilder.Build();
        if (string.IsNullOrEmpty(config["AppConfigurationEndpoint"]))
        {
            throw new ArgumentNullException("Please set the 'AppConfigurationEndpoint' environment variable to a valid Azure App Configuration endpoint URI and re-run this example.");
        }

        configBuilder.AddAzureAppConfiguration(options =>
        {
            var endpoint = config["AppConfigurationEndpoint"] ?? 
                throw new ArgumentNullException("AppConfigurationEndpoint", "The AppConfigurationEndpoint environment variable cannot be null or empty.");
            
            options.Connect(new Uri(endpoint), new DefaultAzureCredential())
                .Select("WebJob:*")
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
