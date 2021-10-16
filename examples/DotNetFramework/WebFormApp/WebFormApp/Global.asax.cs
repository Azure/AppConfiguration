using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace WebFormApp
{
    public class Global : System.Web.HttpApplication
    {
        public static IConfiguration Configuration;
        public static IFeatureManager FeatureManager;

        private static IConfigurationRefresher _configurationRefresher;
        private static IServiceProvider _serviceProvider;

        protected void Application_Start(object sender, EventArgs e)
        {
            // Initialize configuration from Azure App Configuration.
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(Environment.GetEnvironmentVariable("ConnectionString"))
                       // Load all keys that start with `TestApp:`
                       .Select("TestApp:*")
                       // Configure to reload configuration if the registered key 'TestApp:Settings:Sentinel' is modified.
                       // Use the default cache expiration of 30 seconds. It can be overriden via AzureAppConfigurationOptions.SetCacheExpiration.
                       .ConfigureRefresh(refresh => 
                       {
                           refresh.Register("TestApp:Settings:Sentinel", refreshAll:true);
                       })
                       // Use the default cache expiration of 30 seconds. It can be overriden via FeatureFlagOptions.CacheExpirationInterval.
                       .UseFeatureFlags();

                _configurationRefresher = options.GetRefresher();
            });

            // Build configuration
            Configuration = builder.Build();

            // Build feature manager
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(Configuration)
                    .AddFeatureManagement();
            _serviceProvider = services.BuildServiceProvider();
            FeatureManager = _serviceProvider.GetRequiredService<IFeatureManager>();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // Signal to refresh the configuration if the registered key(s) is modified.
            // This will be a no-op if the cache expiration time window is not reached.
            // The configuration is refreshed asynchronously without blocking the execution of the current request.
            _ = _configurationRefresher.TryRefreshAsync();
        }
    }
}
