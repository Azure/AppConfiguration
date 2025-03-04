using System;
using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

[assembly: FunctionsStartup(typeof(FunctionAppInProcess.Startup))]

namespace FunctionAppInProcess
{
    internal class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            // Add Azure App Configuration as additional configuration source
            builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
            {
                Uri endpoint = new(Environment.GetEnvironmentVariable("AZURE_APPCONFIG_ENDPOINT") ?? string.Empty);
                options.Connect(endpoint, new DefaultAzureCredential())
                       // Load all keys that start with `TestApp:` and have no label
                       .Select("TestApp:*")
                       // Reload configuration if any selected key-values have changed.
                       // Use the default refresh interval of 30 seconds. It can be overridden via AzureAppConfigurationRefreshOptions.SetRefreshInterval.
                       .ConfigureRefresh(refreshOptions =>
                            refreshOptions.RegisterAll()
                        )
                       // Load all feature flags with no label. To load feature flags with specific keys and labels, set via FeatureFlagOptions.Select.
                       // Use the default refresh interval of 30 seconds. It can be overridden via FeatureFlagOptions.SetRefreshInterval.
                       .UseFeatureFlags();
            });
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Add Azure App Configuration middleware and feature management to the service collection.
            builder.Services.AddAzureAppConfiguration();
            builder.Services.AddFeatureManagement();
        }
    }
}
