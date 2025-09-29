package main

import (
	"context"
	"fmt"
	"log"
	"os"
	"time"

	"github.com/Azure/AppConfiguration-GoProvider/azureappconfiguration"
)

type Config struct {
	Font    Font
	Message string
}

type Font struct {
	Color string
	Size  int
}

// loadConfiguration handles loading the configuration from Azure App Configuration
func loadConfiguration() (Config, error) {
	// Get connection string from environment variable
	connectionString := os.Getenv("AZURE_APPCONFIG_CONNECTION_STRING")

	// Configuration setup
	options := &azureappconfiguration.Options{
		Selectors: []azureappconfiguration.Selector{
			{
				KeyFilter: "Config.*",
			},
		},
		// Remove the prefix when mapping to struct fields
		TrimKeyPrefixes: []string{"Config."},
	}

	authOptions := azureappconfiguration.AuthenticationOptions{
		ConnectionString: connectionString,
	}

	// Create configuration provider with timeout
	ctx, cancel := context.WithTimeout(context.Background(), 10*time.Second)
	defer cancel()

	appCfgProvider, err := azureappconfiguration.Load(ctx, authOptions, options)
	if err != nil {
		return Config{}, err
	}

	// Parse configuration into struct
	var config Config
	err = appCfgProvider.Unmarshal(&config, nil)
	if err != nil {
		return Config{}, err
	}

	return config, nil
}

func main() {
	fmt.Println("Azure App Configuration - Console Example")
	fmt.Println("----------------------------------------")

	// Load configuration
	fmt.Println("Loading configuration from Azure App Configuration...")
	config, err := loadConfiguration()
	if err != nil {
		log.Fatalf("Error loading configuration: %s", err)
	}

	// Display the configuration values
	fmt.Println("\nConfiguration Values:")
	fmt.Println("--------------------")
	fmt.Printf("Font Color: %s\n", config.Font.Color)
	fmt.Printf("Font Size: %d\n", config.Font.Size)
	fmt.Printf("Message: %s\n", config.Message)
}