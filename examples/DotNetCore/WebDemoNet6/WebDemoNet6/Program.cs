using Microsoft.FeatureManagement;
using WebDemoNet6;

var builder = WebApplication.CreateBuilder(args);

// Load configuration from Azure App Configuration
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(Environment.GetEnvironmentVariable("ConnectionString"))
           // Load all keys that start with `WebDemo:` and have no label
           .Select("WebDemo:*")
           // Configure to reload configuration if the registered key 'WebDemo:Sentinel' is modified.
           // Use the default cache expiration of 30 seconds. It can be overriden via AzureAppConfigurationRefreshOptions.SetCacheExpiration.
           .ConfigureRefresh(refreshOptions =>
           {
               refreshOptions.Register("WebDemo:Sentinel", refreshAll: true);
           })
           // Load all feature flags with no label. To load specific feature flags and labels, set via FeatureFlagOptions.Select.
           // Use the default cache expiration of 30 seconds. It can be overriden via FeatureFlagOptions.CacheExpirationInterval.
           .UseFeatureFlags();
});

// Add services to the container.
builder.Services.AddRazorPages();

// Add Azure App Configuration and feature management services to the container.
builder.Services.AddAzureAppConfiguration()
                .AddFeatureManagement();

// Bind configuration to the Settings object
builder.Services.Configure<Settings>(builder.Configuration.GetSection("WebDemo:Settings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Use Azure App Configuration middleware for dynamic configuration refresh.
app.UseAzureAppConfiguration();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
