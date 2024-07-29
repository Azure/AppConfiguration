using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.AddAzureAppConfiguration(options =>
{
    options.Connect(Environment.GetEnvironmentVariable("AZURE_APPCONFIG_CONNECTION_STRING"))
        .Select("QuickStartApp*");
});

var config = builder.Build();
var key = "QuickStartApp:Settings:Message";
Console.WriteLine(config[key] ?? $"Please create a key-value with the key '{key}' in Azure App Configuration.");
