using System;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Azure.AppConfiguration.WebDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(builder =>
                    {
                        //
                        // This example uses the Microsoft.Azure.AppConfiguration.AspNetCore NuGet package:
                        // - Establishes the connection to Azure App Configuration using DefaultAzureCredential.
                        // - Loads configuration from Azure App Configuration.
                        // - Sets up dynamic configuration refresh triggered by a sentinel key.

                        // Prerequisite
                        // - An Azure App Configuration store is created
                        // - The application identity is granted "App Configuration Data Reader" role in the App Configuration store
                        // - "AzureAppConfigurationEndpoint" is set to the App Configuration endpoint in either appsettings.json or environment
                        // - The "WebDemo" section in the appsettings.json is imported to the App Configuration store
                        // - A sentinel key "WebDemo:Sentinel" is created in App Configuration to signal the refresh of configuration

                        var settings = builder.Build();
                        string appConfigurationEndpoint = settings["AzureAppConfigurationEndpoint"];
                        if (!string.IsNullOrEmpty(appConfigurationEndpoint))
                        {
                            builder.AddAzureAppConfiguration(options =>
                            {
                                options.Connect(new Uri(appConfigurationEndpoint), new DefaultAzureCredential())
                                       .Select(keyFilter: "WebDemo:*")
                                       .ConfigureRefresh((refreshOptions) =>
                                       {
                                           // Indicates that all configuration should be refreshed when the given key has changed.
                                           refreshOptions.Register(key: "WebDemo:Sentinel", refreshAll: true);
                                       });
                            });
                        }
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
