﻿// Copyright (c) Microsoft Corporation.
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
        Uri endpoint = new(Environment.GetEnvironmentVariable("AZURE_APPCONFIG_ENDPOINT") ??
            throw new InvalidOperationException("The environment variable 'AZURE_APPCONFIG_ENDPOINT' is not set or is empty."));
        options.Connect(endpoint, credential)
               // Load all keys that start with "ChatApp:" and have no label.
               .Select("ChatApp:*")
               // Reload configuration if any selected key-values have changed.
               // Use the default refresh interval of 30 seconds. It can be overridden via refreshOptions.SetRefreshInterval.
               .ConfigureRefresh(refreshOptions =>
               {
                   refreshOptions.RegisterAll();
               });

        refresher = options.GetRefresher();
    })
    .Build();

// Retrieve the OpenAI connection information from the configuration
Uri openaiEndpoint = new (configuration["ChatApp:AzureOpenAI:Endpoint"]);
string deploymentName = configuration["ChatApp:AzureOpenAI:DeploymentName"];

// Create a chat client
AzureOpenAIClient azureClient = new(openaiEndpoint, credential);
ChatClient chatClient = azureClient.GetChatClient(deploymentName);

while (true)
{
    // Refresh the configuration from Azure App Configuration
    await refresher.TryRefreshAsync();

    // Configure chat completion with AI configuration
    var modelConfiguration = configuration.GetSection("ChatApp:Model").Get<ModelConfiguration>();
    var requestOptions = new ChatCompletionOptions()
    {
        MaxOutputTokenCount = modelConfiguration.MaxTokens,
        Temperature = modelConfiguration.Temperature,
        TopP = modelConfiguration.TopP
    };

    foreach (var message in modelConfiguration.Messages)
    {
        Console.WriteLine($"{message.Role}: {message.Content}");
    }

    // Get chat response from AI
    var response = await chatClient.CompleteChatAsync(GetChatMessages(modelConfiguration), requestOptions);
    System.Console.WriteLine($"AI response: {response.Value.Content[0].Text}");

    Console.WriteLine("Press Enter to continue...");
    Console.ReadLine();
}

static IEnumerable<ChatMessage> GetChatMessages(ModelConfiguration modelConfiguration)
{
    return modelConfiguration.Messages.Select<Message, ChatMessage>(message => message.Role switch
    {
        "system" => ChatMessage.CreateSystemMessage(message.Content),
        "user" => ChatMessage.CreateUserMessage(message.Content),
        "assistant" => ChatMessage.CreateAssistantMessage(message.Content),
        _ => throw new ArgumentException($"Unknown role: {message.Role}", nameof(message.Role))
    });
}
