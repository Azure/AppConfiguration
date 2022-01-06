using System;
using Microsoft.Azure.AppConfiguration.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;

namespace FunctionAppIsolatedMode
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddAzureAppConfiguration(options =>
                    {
                        options.Connect(Environment.GetEnvironmentVariable("ConnectionString"))
                               // Load all keys that start with `TestApp:` and have no label
                               .Select("TestApp:*")
                               // Configure to reload configuration if the registered key 'TestApp:Settings:Sentinel' is modified.
                               // Use the default cache expiration of 30 seconds. It can be overriden via AzureAppConfigurationRefreshOptions.SetCacheExpiration.
                               .ConfigureRefresh(refreshOptions =>
                               {
                                   refreshOptions.Register("TestApp:Settings:Sentinel", refreshAll: true);
                               })
                               // Load all feature flags with no label. To load specific feature flags and labels, set via FeatureFlagOptions.Select.
                               // Use the default cache expiration of 30 seconds. It can be overriden via FeatureFlagOptions.CacheExpirationInterval.
                               .UseFeatureFlags();
                    });
                })
                .ConfigureServices(services =>
                {
                    // Make Azure App Configuration services and feature manager available through dependency injection
                    services.AddAzureAppConfiguration()
                            .AddFeatureManagement();
                })
                .ConfigureFunctionsWorkerDefaults(app =>
                {
                    // Use Azure App Configuration middleware for data refresh
                    app.UseAzureAppConfiguration();
                })
                .Build();

            host.Run();
        }
    }
}
