using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace Microsoft.Azure.AppConfiguration.WebDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    //
                    // This example uses the Microsoft.Extensions.Configuration.AzureAppConfiguration NuGet package:
                    // Loads settings from Azure App Configuration.
                    // Sets up the provider to listen for changes triggered by a sentinel value.
                    // Establishes the connection to Azure App Configuration via Managed Identity for Azure Services.
                    var settings = config.Build();

                    config.AddAzureAppConfiguration(options => {

                        options.ConnectWithManagedIdentity(settings["AzureAppConfigurationEndpoint"])
                               .Use(keyFilter: "WebDemo:*")
                               .ConfigureRefresh((refreshOptions) =>
                               {
                                   //
                                   // Indicates that all settings should be refreshed when the given key has changed.
                                   refreshOptions.Register(key: "WebDemo:Sentinel", label: LabelFilter.Null, refreshAll: true);
                               });
                    });

                    settings = config.Build();
                })
                .UseStartup<Startup>()
                .Build();
    }
}
