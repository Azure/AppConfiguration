using Azure.Core;
using Azure.Identity;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Azure.AI.Projects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

TokenCredential credential = new AzureCliCredential();
IConfigurationRefresher refresher = null;

// Load configuration from Azure App Configuration
IConfiguration configuration = new ConfigurationBuilder()
    .AddAzureAppConfiguration(options =>
    {
        Uri endpoint = new(Environment.GetEnvironmentVariable("AZURE_APPCONFIGURATION_ENDPOINT") ??
            throw new InvalidOperationException("The environment variable 'AZURE_APPCONFIGURATION_ENDPOINT' is not set or is empty"));
        options.Connect(endpoint, credential)
            .Select("ChatAgent:*")
            .ConfigureRefresh(refreshOptions =>
            {
                refreshOptions.RegisterAll();
            });
        refresher = options.GetRefresher();
    }).Build();

var endpoint = configuration["ChatAgent:ProjectEndpoint"];
var deploymentName = configuration["ChatAgent:DeploymentName"];

IChatClient chatClient = new AIProjectClient(
    new Uri(endpoint), credential)
    .GetProjectOpenAIClient()
    .GetProjectResponsesClient()
    .AsIChatClient();

var agentSpec = configuration["ChatAgent:Spec"];

var agentFactory = new ChatClientPromptAgentFactory(chatClient);

AIAgent agent = await agentFactory.CreateFromYamlAsync(agentSpec);

while (true)
{
    Console.WriteLine("How can I help? (type 'quit' to exit)");

    Console.Write("User: ");

    var userInput = Console.ReadLine();

    if (userInput?.Trim().ToLower() == "quit")
    {
        break;
    }

    var response = await agent.RunAsync(userInput);

    Console.WriteLine($"Agent response: {response}");
    Console.WriteLine("Press enter to continue...");
    Console.ReadLine();
}

Console.WriteLine("Goodbye!");