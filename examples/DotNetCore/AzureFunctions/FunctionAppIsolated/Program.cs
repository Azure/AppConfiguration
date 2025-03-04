using Azure.Identity;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;

var builder = FunctionsApplication.CreateBuilder(args);

// Connect to Azure App Configuration
builder.Configuration.AddAzureAppConfiguration(options =>
{
    Uri endpoint = new(Environment.GetEnvironmentVariable("AZURE_APPCONFIG_ENDPOINT") ?? 
        throw new InvalidOperationException("The environment variable 'AZURE_APPCONFIG_ENDPOINT' is not set or is empty."));
    options.Connect(endpoint, new DefaultAzureCredential())
           // Load all keys that start with `TestApp:` and have no label
           .Select("TestApp:*")
           // Reload configuration if any selected key-values have changed.
           // Use the default refresh interval of 30 seconds. It can be overridden via AzureAppConfigurationRefreshOptions.SetRefreshInterval.
           .ConfigureRefresh(refreshOptions =>
           {
               refreshOptions.RegisterAll();
           })
           // Load all feature flags with no label. To load feature flags with specific keys and labels, set via FeatureFlagOptions.Select.
           // Use the default refresh interval of 30 seconds. It can be overridden via FeatureFlagOptions.SetRefreshInterval.
           .UseFeatureFlags();
});

// Add Azure App Configuration middleware and feature management to the service collection.
builder.Services
    .AddAzureAppConfiguration()
    .AddFeatureManagement();

// Use Azure App Configuration middleware for dynamic configuration and feature flag refresh.
builder.UseAzureAppConfiguration();

builder.ConfigureFunctionsWebApplication();

builder.Build().Run();
