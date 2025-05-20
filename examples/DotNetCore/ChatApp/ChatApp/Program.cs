using Azure.AI.OpenAI;
using Azure.Core;
using Azure.Identity;
using ChatApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using OpenAI.Chat;

TokenCredential credential = new DefaultAzureCredential();
IConfigurationRefresher _refresher = null;

// Load configuration from Azure App Configuration
IConfiguration configuration = new ConfigurationBuilder()
    .AddAzureAppConfiguration(options =>
    {
        Uri endpoint = new(Environment.GetEnvironmentVariable("AZURE_APPCONFIG_ENDPOINT") ??
            throw new InvalidOperationException("The environment variable 'AZURE_APPCONFIG_ENDPOINT' is not set or is empty."));
        options.Connect(endpoint, credential)
               // Load all keys that have a specific prefix and no label.
               .Select("ChatLLM:*")
               // Reload configuration if any selected key-values have changed.
               // Use the default refresh interval of 30 seconds. It can be overridden via refreshOptions.SetRefreshInterval.
               .ConfigureRefresh(refreshOptions =>
               {
                   refreshOptions.RegisterAll()
                                 .SetRefreshInterval(TimeSpan.FromSeconds(5));
               });

        _refresher = options.GetRefresher();
    })
    .Build();

// Retrieve the model connection information from the configuration
Uri chatEndpoint = new (configuration["ChatLLM:Endpoint"]);
string deploymentName = configuration["ChatLLM:DeploymentName"];

// Create a chat client
AzureOpenAIClient azureClient = new(chatEndpoint, credential);
ChatClient chatClient = azureClient.GetChatClient(deploymentName);

while (true)
{
    // Refresh the configuration from Azure App Configuration
    await _refresher.TryRefreshAsync();

    // Configure chat completion with AI configuration
    var modelConfig = configuration.GetSection("ChatLLM:Model").Get<ModelConfiguration>();
    var requestOptions = new ChatCompletionOptions()
    {
        MaxOutputTokenCount = modelConfig.MaxTokens,
        Temperature = modelConfig.Temperature,
        TopP = modelConfig.TopP
    };

    foreach (var message in modelConfig.Messages)
    {
        Console.WriteLine($"{message.Role}: {message.Content}");
    }

    // Get chat response from AI
    var response = chatClient.CompleteChat(modelConfig.ChatMessages, requestOptions);
    System.Console.WriteLine($"AI response: {response.Value.Content[0].Text}");

    Console.WriteLine("Press Enter to continue...");
    Console.ReadLine();
}
