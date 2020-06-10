using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;
using System;

[assembly: FunctionsStartup(typeof(AzureFunctionPushRefresh.Startup))]

namespace AzureFunctionPushRefresh
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            IConfigurationRefresher configurationRefresher = null;

            // Load configuration from Azure App Configuration
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddAzureAppConfiguration(options =>
            {
                options.Connect(Environment.GetEnvironmentVariable("ConnectionString"))
                       // Load all keys that start with `TestApp:`
                       .Select("TestApp:*")
                       // Configure to reload configuration if the registered 'Sentinel' key is modified
                       .ConfigureRefresh(refreshOptions =>
                            refreshOptions.Register("TestApp:Settings:Sentinel", refreshAll: true)
                                          .SetCacheExpiration(TimeSpan.FromDays(30))
                        )
                       // Indicate to load feature flags
                       .UseFeatureFlags(ffOptions => ffOptions.CacheExpirationTime = TimeSpan.FromDays(30));
                configurationRefresher = options.GetRefresher();
            });
            IConfiguration configuration = configurationBuilder.Build();

            // Make settings, feature manager and configuration refresher available through DI
            builder.Services.Configure<Settings>(configuration.GetSection("TestApp:Settings"));
            builder.Services.AddFeatureManagement(configuration);
            builder.Services.AddSingleton(configurationRefresher);
        }
    }
}
