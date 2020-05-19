using System;
using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebDemoWithEventHub
{
    public class Program
    {
        private static IConfigurationRefresher refresher = null;

        public static void Main(string[] args)
        {
            IHostBuilder builder = CreateHostBuilder(args);
            builder.ConfigureServices((services) =>
            {
                services.AddSingleton<IConfigRefresher>(new ConfigRefresher() { Refresher = refresher });
            });

            builder.Build().Run();
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
                                        .ConfigureKeyVault(kv =>
                                        {
                                            kv.SetCredential(new DefaultAzureCredential());
                                        })
                                       .Select(keyFilter: "WebDemo:*")
                                       .ConfigureRefresh((refreshOptions) =>
                                       {
                                           refreshOptions.Register(key: "WebDemo:Settings", refreshAll: true);
                                       });

                                refresher = options.GetRefresher();
                            });
                        }
                    });

                    webBuilder.UseStartup<Startup>();
                });
    }
}
