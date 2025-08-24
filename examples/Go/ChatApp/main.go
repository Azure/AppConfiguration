package main

import (
	"bufio"
	"context"
	"fmt"
	"log"
	"os"
	"strings"
	"time"

	"github.com/Azure/AppConfiguration-GoProvider/azureappconfiguration"
	"github.com/Azure/azure-sdk-for-go/sdk/azidentity"
	openai "github.com/openai/openai-go"
	"github.com/openai/openai-go/azure"
)

type ChatApp struct {
	configProvider    *azureappconfiguration.AzureAppConfiguration
	openAIClient openai.Client
	aiConfig     AIConfig
}

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
	APIKey    string
}

type Message struct {
	Role    string `json:"role"`
	Content string `json:"content"`
}

func loadAzureAppConfiguration(ctx context.Context) (*azureappconfiguration.AzureAppConfiguration, error) {
	endpoint := os.Getenv("AZURE_APPCONFIGURATION_ENDPOINT")
	if endpoint == "" {
		return nil, fmt.Errorf("AZURE_APPCONFIGURATION_ENDPOINT environment variable is not set")
	}

	credential, err := azidentity.NewDefaultAzureCredential(nil)
	if err != nil {
		return nil, fmt.Errorf("failed to create Azure credential: %w", err)
	}

	authOptions := azureappconfiguration.AuthenticationOptions{
		Endpoint:  endpoint,
		Credential: credential,
	}

	options := &azureappconfiguration.Options{
		Selectors: []azureappconfiguration.Selector{
			// Load all keys that start with "ChatApp:" and have no label
			{
				KeyFilter:   "ChatApp:*",
			},
		},
		TrimKeyPrefixes: []string{"ChatApp:"},
		RefreshOptions: azureappconfiguration.KeyValueRefreshOptions{
			Enabled:  true,
			Interval: 10 * time.Second,
		},
		KeyVaultOptions: azureappconfiguration.KeyVaultOptions{
			Credential: credential,
		},
	}

	appConfig, err := azureappconfiguration.Load(ctx, authOptions, options)
	if err != nil {
		return nil, fmt.Errorf("failed to load configuration: %w", err)
	}

	return appConfig, nil
}

// Create an Azure OpenAI client using API key if available, otherwise use the DefaultAzureCredential
func (app *ChatApp) createAzureOpenAIClient() error {
	if app.aiConfig.AzureOpenAI.APIKey != "" {
		// Use API key for authentication
		client := openai.NewClient(
			azure.WithAPIKey(app.aiConfig.AzureOpenAI.APIKey),
			azure.WithEndpoint(app.aiConfig.AzureOpenAI.Endpoint, app.aiConfig.AzureOpenAI.APIVersion),
		)
		app.openAIClient = client
		return nil
	}

	// Use DefaultAzureCredential for authentication
	tokenCredential, err := azidentity.NewDefaultAzureCredential(nil)
	if err != nil {
		return fmt.Errorf("failed to create Azure credential: %w", err)
	}

	client := openai.NewClient(
		azure.WithEndpoint(app.aiConfig.AzureOpenAI.Endpoint, app.aiConfig.AzureOpenAI.APIVersion),
		azure.WithTokenCredential(tokenCredential),
	)

	app.openAIClient = client
	return nil
}

func (app *ChatApp) callAzureOpenAI(userMessage string) (string, error) {
	messages := []openai.ChatCompletionMessageParamUnion{}
	for _, msg := range app.aiConfig.ChatCompletion.Messages {
		switch msg.Role {
		case "system":
			messages = append(messages, openai.SystemMessage(msg.Content))
		case "user":
			messages = append(messages, openai.UserMessage(msg.Content))
		case "assistant":
			messages = append(messages, openai.AssistantMessage(msg.Content))
		}
	}

	// Add the user's input message
	messages = append(messages, openai.UserMessage(userMessage))

	// Create chat completion parameters
	params := openai.ChatCompletionNewParams{
		Messages:    messages,
		Model:       app.aiConfig.ChatCompletion.Model,
		MaxTokens:   openai.Int(app.aiConfig.ChatCompletion.MaxTokens),
		Temperature: openai.Float(app.aiConfig.ChatCompletion.Temperature),
		TopP:        openai.Float(app.aiConfig.ChatCompletion.TopP),
	}

	ctx := context.Background()
	completion, err := app.openAIClient.Chat.Completions.New(ctx, params)
	if err != nil {
		return "", fmt.Errorf("failed to get chat completion: %w", err)
	}

	if len(completion.Choices) == 0 {
		return "", fmt.Errorf("chat completion returned zero choices")
	}

	return completion.Choices[0].Message.Content, nil
}

func (app *ChatApp) runInteractiveChat() {
    fmt.Println("Chat started! What's on your mind?")
	reader := bufio.NewReader(os.Stdin)

	for {
		fmt.Print("You: ")
		userInput, err := reader.ReadString('\n')
		if err != nil {
			log.Printf("Error reading input: %v", err)
			continue
		}

		userInput = strings.TrimSpace(userInput)
		if userInput == "" {
			fmt.Println("Exiting Chat. Goodbye!")
			break
		}

		// Refresh configuration
		ctx := context.Background()
		if err := app.configProvider.Refresh(ctx); err != nil {
			log.Printf("Error refreshing configuration: %v", err)
		}

		// Get AI response
		fmt.Print("AI: ")
		response, err := app.callAzureOpenAI(userInput)
		if err != nil {
			log.Printf("Error calling OpenAI: %v", err)
			fmt.Println("Sorry, I encountered an error. Please try again.")
			continue
		}

		fmt.Println(response)
		fmt.Println()
	}
}

func main() {
	ctx := context.Background()
	configProvider, err := loadAzureAppConfiguration(ctx)
	if err != nil {
		log.Fatal("Error loading Azure App Configuration:", err)
		return
	}
	
	// Load AI configuration from Azure App Configuration
	var aiConfig AIConfig
	if err := configProvider.Unmarshal(&aiConfig, &azureappconfiguration.ConstructionOptions{Separator: ":"}); err != nil {
		log.Fatal("Error loading AI configuration:", err)
	}

	// Register a callback to refresh AI configuration on changes
	configProvider.OnRefreshSuccess(func() {
		if err := configProvider.Unmarshal(&aiConfig, &azureappconfiguration.ConstructionOptions{Separator: ":"}); err != nil {
			log.Printf("Error refreshing AI configuration: %v", err)
		}
	})

	app := &ChatApp{
		configProvider: configProvider,
		aiConfig:      aiConfig,
	}

	// Initialize Azure OpenAI client
	if err := app.createAzureOpenAIClient(); err != nil {
		log.Fatalf("Failed to create Azure OpenAI client: %v", err)
	}

	app.runInteractiveChat()
}
