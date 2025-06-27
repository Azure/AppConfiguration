// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//

using Azure.AI.OpenAI;
using Azure.Core;
using Azure.Identity;
using ChatApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using OpenAI.Chat;

TokenCredential credential = new DefaultAzureCredential();
IConfigurationRefresher refresher = null;

// Load configuration from Azure App Configuration
IConfiguration configuration = new ConfigurationBuilder()
    .AddAzureAppConfiguration(options =>
    {
        Uri endpoint = new(Environment.GetEnvironmentVariable("AZURE_APPCONFIGURATION_ENDPOINT") ??
            throw new InvalidOperationException("The environment variable 'AZURE_APPCONFIGURATION_ENDPOINT' is not set or is empty."));
        options.Connect(endpoint, credential)
               // Load all keys that start with "ChatApp:" and have no label.
               .Select("ChatApp:*")
               // Reload configuration if any selected key-values have changed.
               // Use the default refresh interval of 30 seconds. It can be overridden via refreshOptions.SetRefreshInterval.
               .ConfigureRefresh(refreshOptions =>
               {
                   refreshOptions.RegisterAll();
               })
               .ConfigureKeyVault(keyVaultOptions =>
               {
                   // Use the DefaultAzureCredential to access Key Vault secrets.
                   keyVaultOptions.SetCredential(credential);
               });

        refresher = options.GetRefresher();
    })
    .Build();

// Retrieve the OpenAI connection information from the configuration
var azureOpenAIConfiguration = configuration.GetSection("ChatApp:AzureOpenAI").Get<AzureOpenAIConfiguration>();

// Create a chat client using API key if available, otherwise use the DefaultAzureCredential
AzureOpenAIClient azureClient;
if (!string.IsNullOrEmpty(azureOpenAIConfiguration.ApiKey))
{
    azureClient = new AzureOpenAIClient(new Uri(azureOpenAIConfiguration.Endpoint), new Azure.AzureKeyCredential(azureOpenAIConfiguration.ApiKey));
}
else
{
    azureClient = new AzureOpenAIClient(new Uri(azureOpenAIConfiguration.Endpoint), credential);
}
ChatClient chatClient = azureClient.GetChatClient(azureOpenAIConfiguration.DeploymentName);

// Initialize chat conversation
var chatConversation = new List<ChatMessage>();
Console.WriteLine("Chat started! What's on your mind?");
while (true)
{
    // Refresh the configuration from Azure App Configuration
    await refresher.TryRefreshAsync();

    // Configure chat completion with AI configuration
    var chatCompletionConfiguration = configuration.GetSection("ChatApp:ChatCompletion").Get<ChatCompletionConfiguration>();
    var requestOptions = new ChatCompletionOptions()
    {
        MaxOutputTokenCount = chatCompletionConfiguration.MaxTokens,
        Temperature = chatCompletionConfiguration.Temperature,
        TopP = chatCompletionConfiguration.TopP
    };

    // Get user input
    Console.Write("You: ");
    string? userInput = Console.ReadLine();

    // Exit if user input is empty
    if (string.IsNullOrEmpty(userInput))
    {
        Console.WriteLine("Exiting chat. Goodbye!");
        break;
    }

    // Add user message to chat conversation
    chatConversation.Add(ChatMessage.CreateUserMessage(userInput));

    // Get latest system message from AI configuration
    var chatMessages = new List<ChatMessage>(GetChatMessages(chatCompletionConfiguration));
    chatMessages.AddRange(chatConversation);

    // Get AI response and add it to chat conversation
    var response = await chatClient.CompleteChatAsync(chatMessages, requestOptions);
    string aiResponse = response.Value.Content[0].Text;
    Console.WriteLine($"AI: {aiResponse}");
    chatConversation.Add(ChatMessage.CreateAssistantMessage(aiResponse));

    Console.WriteLine();
}

// Helper method to convert configuration messages to ChatMessage objects
static IEnumerable<ChatMessage> GetChatMessages(ChatCompletionConfiguration chatCompletionConfiguration)
{
    return chatCompletionConfiguration.Messages.Select<Message, ChatMessage>(message => message.Role switch
    {
        "system" => ChatMessage.CreateSystemMessage(message.Content),
        "user" => ChatMessage.CreateUserMessage(message.Content),
        "assistant" => ChatMessage.CreateAssistantMessage(message.Content),
        _ => throw new ArgumentException($"Unknown role: {message.Role}", nameof(message.Role))
    });
}
