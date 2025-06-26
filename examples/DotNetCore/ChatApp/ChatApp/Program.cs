// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using Azure;
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
var openAIConfiguration = configuration.GetSection("ChatApp:AzureOpenAI").Get<OpenAIConfiguration>();

// Create a chat client using API key if available, otherwise use the DefaultAzureCredential
AzureOpenAIClient azureClient;
if (!string.IsNullOrEmpty(openAIConfiguration.ApiKey))
{
    azureClient = new AzureOpenAIClient(new Uri(openAIConfiguration.Endpoint), new Azure.AzureKeyCredential(openAIConfiguration.ApiKey));
}
else
{
    azureClient = new AzureOpenAIClient(new Uri(openAIConfiguration.Endpoint), credential);
}
ChatClient chatClient = azureClient.GetChatClient(openAIConfiguration.DeploymentName);

// Initialize chat conversation
var chatConversation = new List<ChatMessage>();
Console.WriteLine("Chat started! What's on your mind?");
while (true)
{
    // Refresh the configuration from Azure App Configuration
    await refresher.TryRefreshAsync();

    // Configure chat completion with AI configuration
    var completionConfiguration = configuration.GetSection("ChatApp:Completion").Get<CompletionConfiguration>();
    var requestOptions = new ChatCompletionOptions()
    {
        MaxOutputTokenCount = completionConfiguration.MaxTokens,
        Temperature = completionConfiguration.Temperature,
        TopP = completionConfiguration.TopP
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
    var chatMessages = new List<ChatMessage>(GetChatMessages(completionConfiguration));
    chatMessages.AddRange(chatConversation);

    // Get AI response and update chat conversation
    var response = await chatClient.CompleteChatAsync(chatMessages, requestOptions);
    var aiResponse = response.Value.Content[0].Text;
    System.Console.WriteLine($"AI: {aiResponse}");
    chatConversation.Add(ChatMessage.CreateAssistantMessage(aiResponse));

    Console.WriteLine();
}

// Helper method to convert configuration messages to ChatMessage objects
static IEnumerable<ChatMessage> GetChatMessages(CompletionConfiguration completionConfiguration)
{
    return completionConfiguration.Messages.Select<Message, ChatMessage>(message => message.Role switch
    {
        "system" => ChatMessage.CreateSystemMessage(message.Content),
        "user" => ChatMessage.CreateUserMessage(message.Content),
        "assistant" => ChatMessage.CreateAssistantMessage(message.Content),
        _ => throw new ArgumentException($"Unknown role: {message.Role}", nameof(message.Role))
    });
}
