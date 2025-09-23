package main

import (
	"bufio"
	"context"
	"fmt"
	"log"
	"os"

	"github.com/Azure/AppConfiguration-GoProvider/azureappconfiguration"
	"github.com/Azure/azure-sdk-for-go/sdk/azidentity"
	openai "github.com/openai/openai-go"
	"github.com/openai/openai-go/azure"
)

type AIConfig struct {
	ChatCompletion ChatCompletion
	AzureOpenAI    AzureOpenAI
}

type ChatCompletion struct {
	Model       string    `json:"model"`
	Messages    []Message `json:"messages"`
	MaxTokens   int64     `json:"max_tokens"`
	Temperature float64   `json:"temperature"`
	TopP        float64   `json:"top_p"`
}

type AzureOpenAI struct {
	Endpoint   string
	APIVersion string
	APIKey     string
}

type Message struct {
	Role    string `json:"role"`
	Content string `json:"content"`
}

var aiConfig AIConfig
var tokenCredential, _ = azidentity.NewDefaultAzureCredential(nil)

func main() {
	configProvider, err := loadAzureAppConfiguration(context.Background())
	if err != nil {
		log.Fatal("Error loading Azure App Configuration:", err)
	}

	// Configure chat completion with AI configuration
	configProvider.Unmarshal(&aiConfig, &azureappconfiguration.ConstructionOptions{Separator: ":"})

	// Register a callback to refresh AI configuration on changes
	configProvider.OnRefreshSuccess(func() {
		configProvider.Unmarshal(&aiConfig, &azureappconfiguration.ConstructionOptions{Separator: ":"})
	})

	// Create a chat client using API key if available, otherwise use the DefaultAzureCredential
	var openAIClient openai.Client
	if aiConfig.AzureOpenAI.APIKey != "" {
		openAIClient = openai.NewClient(azure.WithAPIKey(aiConfig.AzureOpenAI.APIKey), azure.WithEndpoint(aiConfig.AzureOpenAI.Endpoint, aiConfig.AzureOpenAI.APIVersion))
	} else {
		openAIClient = openai.NewClient(azure.WithEndpoint(aiConfig.AzureOpenAI.Endpoint, aiConfig.AzureOpenAI.APIVersion), azure.WithTokenCredential(tokenCredential))
	}

	// Initialize chat conversation
	var chatConversation []openai.ChatCompletionMessageParamUnion
	fmt.Println("Chat started! What's on your mind?")
	reader := bufio.NewReader(os.Stdin)

	for {
		// Refresh the configuration from Azure App Configuration
		configProvider.Refresh(context.Background())

		// Get user input
		fmt.Print("You: ")
		userInput, _ := reader.ReadString('\n')

		// Exit if user input is empty
		if userInput == "" {
			fmt.Println("Exiting Chat. Goodbye!")
			break
		}

		// Add user message to chat conversation
		chatConversation = append(chatConversation, openai.UserMessage(userInput))

		// Get AI response and add it to chat conversation
		response, _ := getAIResponse(openAIClient, chatConversation)
		fmt.Printf("AI: %s\n", response)
		chatConversation = append(chatConversation, openai.AssistantMessage(response))

		fmt.Println()
	}
}

// Load configuration from Azure App Configuration
func loadAzureAppConfiguration(ctx context.Context) (*azureappconfiguration.AzureAppConfiguration, error) {
	endpoint := os.Getenv("AZURE_APPCONFIGURATION_ENDPOINT")
	if endpoint == "" {
		return nil, fmt.Errorf("AZURE_APPCONFIGURATION_ENDPOINT environment variable is not set")
	}

	authOptions := azureappconfiguration.AuthenticationOptions{
		Endpoint:   endpoint,
		Credential: tokenCredential,
	}

	options := &azureappconfiguration.Options{
		Selectors: []azureappconfiguration.Selector{
			// Load all keys that start with "ChatApp:" and have no label
			{
				KeyFilter: "ChatApp:*",
			},
		},
		TrimKeyPrefixes: []string{"ChatApp:"},
		// Reload configuration if any selected key-values have changed.
		// Use the default refresh interval of 30 seconds. It can be overridden via RefreshOptions.Interval
		RefreshOptions: azureappconfiguration.KeyValueRefreshOptions{
			Enabled:  true,
		},
		KeyVaultOptions: azureappconfiguration.KeyVaultOptions{
			Credential: tokenCredential,
		},
	}

	return azureappconfiguration.Load(ctx, authOptions, options)
}

func getAIResponse(openAIClient openai.Client, chatConversation []openai.ChatCompletionMessageParamUnion) (string, error) {
	var completionMessages []openai.ChatCompletionMessageParamUnion

	for _, msg := range aiConfig.ChatCompletion.Messages {
		switch msg.Role {
		case "system":
			completionMessages = append(completionMessages, openai.SystemMessage(msg.Content))
		case "user":
			completionMessages = append(completionMessages, openai.UserMessage(msg.Content))
		case "assistant":
			completionMessages = append(completionMessages, openai.AssistantMessage(msg.Content))
		}
	}

	// Add the chat conversation history
	completionMessages = append(completionMessages, chatConversation...)

	// Create chat completion parameters
	params := openai.ChatCompletionNewParams{
		Messages:    completionMessages,
		Model:       aiConfig.ChatCompletion.Model,
		MaxTokens:   openai.Int(aiConfig.ChatCompletion.MaxTokens),
		Temperature: openai.Float(aiConfig.ChatCompletion.Temperature),
		TopP:        openai.Float(aiConfig.ChatCompletion.TopP),
	}

	if completion, err := openAIClient.Chat.Completions.New(context.Background(), params); err != nil {
		return "", err
	} else {
		return completion.Choices[0].Message.Content, nil
	}
}
