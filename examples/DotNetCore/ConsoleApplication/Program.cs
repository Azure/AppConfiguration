using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

// Setup configuration builder
var builder = new ConfigurationBuilder();

// Load base configuration from appsettings.json and environment variables
builder.AddJsonFile("appsettings.json")
       .AddEnvironmentVariables();

// Build initial configuration
var initialConfig = builder.Build();

// Check for App Configuration endpoint
string? endpoint = initialConfig["AppConfigurationEndpoint"];
if (string.IsNullOrEmpty(endpoint))
{
    Console.WriteLine("App Configuration endpoint not found.");
    Console.WriteLine("Please set the 'AppConfigurationEndpoint' environment variable to a valid Azure App Configuration endpoint URL and re-run this example.");
    return 1;
}

// Connect to Azure App Configuration
IConfigurationRefresher refresher = null!;
builder.AddAzureAppConfiguration(options =>
{
    // Use DefaultAzureCredential for Microsoft Entra ID authentication
    options.Connect(new Uri(endpoint), new DefaultAzureCredential())
           // Load all keys that start with "Settings:" and have no label.
           .Select("Settings:*")
           .TrimKeyPrefix("Settings:")
           .ConfigureRefresh(refreshOptions =>
           {
               // Reload configuration if any selected key-values have changed.
               // Use the default refresh interval of 30 seconds. It can be overridden via refreshOptions.SetRefreshInterval.
               refreshOptions.RegisterAll();
           });

    // Get an instance of the refresher that can be used to refresh data
    refresher = options.GetRefresher();
});

// Build the final configuration
IConfiguration configuration = builder.Build();

// Application main loop
while (true)
{
    // Trigger and wait for an async refresh for configuration settings
    await refresher.TryRefreshAsync();

    Console.Clear();
        
    Console.WriteLine($"{configuration["AppName"]} has been configured to run in {configuration["Language"]}");
    Console.WriteLine();

    Console.WriteLine(string.Equals(configuration["Language"], "Spanish", StringComparison.OrdinalIgnoreCase)
        ? "Buenos Dias."
        : "Good morning");
    Console.WriteLine();
        
    Console.WriteLine("Press CTRL+C to exit...");

    await Task.Delay(10000);
}
