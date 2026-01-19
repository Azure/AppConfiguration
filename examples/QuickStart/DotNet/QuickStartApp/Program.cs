using Microsoft.Extensions.Configuration;
using Azure.Identity;

Uri endpoint = new(Environment.GetEnvironmentVariable("AZURE_APPCONFIGURATION_ENDPOINT") ??
    throw new InvalidOperationException("The environment variable 'AZURE_APPCONFIGURATION_ENDPOINT' is not set or is empty."));

var builder = new ConfigurationBuilder();
builder.AddAzureAppConfiguration(options =>
{
    options.Connect(endpoint, new DefaultAzureCredential())
        .Select("QuickStartApp*");
});

var config = builder.Build();
var key = "QuickStartApp:Settings:Message";
Console.WriteLine(config[key] ?? $"Please create a key-value with the key '{key}' in Azure App Configuration.");
