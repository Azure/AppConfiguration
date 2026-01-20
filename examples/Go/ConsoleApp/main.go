package main

import (
	"context"
	"fmt"
	"log"
	"os"
	"os/signal"
	"syscall"
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

func main() {
	// Load configuration from Azure App Configuration
	configProvider, err := loadAzureAppConfiguration()
	if err != nil {
		log.Fatalf("Error loading configuration: %s", err)
	}

	// Parse initial configuration into struct
	var config Config
	err = configProvider.Unmarshal(&config, nil)
	if err != nil {
		log.Fatalf("Error unmarshalling configuration: %s", err)
	}

	// Display the initial configuration
	displayConfig(config)

	// Register refresh callback to update and display the configuration
	configProvider.OnRefreshSuccess(func() {
		fmt.Println("\n Configuration changed! Updating values...")
		
		// Re-unmarshal the configuration
		var updatedConfig Config
		err := configProvider.Unmarshal(&updatedConfig, nil)
		if err != nil {
			log.Printf("Error unmarshalling updated configuration: %s", err)
			return
		}
		
		// Update our working config
		config = updatedConfig
		
		// Display the updated configuration
		displayConfig(config)
	})

	// Setup a channel to listen for termination signals
	done := make(chan os.Signal, 1)
	signal.Notify(done, syscall.SIGINT, syscall.SIGTERM)

	fmt.Println("\nWaiting for configuration changes...")
	fmt.Println("(Update values in Azure App Configuration to see refresh in action)")
	fmt.Println("Press Ctrl+C to exit")

	// Start a ticker to periodically trigger refresh
	ticker := time.NewTicker(5 * time.Second)
	defer ticker.Stop()

	// Keep the application running until terminated
	for {
		select {
		case <-ticker.C:
			// Trigger refresh in background
			go func() {
				ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
				defer cancel()
				
				if err := configProvider.Refresh(ctx); err != nil {
					log.Printf("Error refreshing configuration: %s", err)
				}
			}()
		case <-done:
			fmt.Println("\nExiting...")
			return
		}
	}
}

// loadAzureAppConfiguration loads the configuration from Azure App Configuration
func loadAzureAppConfiguration() (*azureappconfiguration.AzureAppConfiguration, error) {
	// Get connection string from environment variable
	connectionString := os.Getenv("AZURE_APPCONFIG_CONNECTION_STRING")

	// Options setup
	options := &azureappconfiguration.Options{
		Selectors: []azureappconfiguration.Selector{
			{
				KeyFilter: "Config.*",
			},
		},
		// Remove the prefix when mapping to struct fields
		TrimKeyPrefixes: []string{"Config."},
		// Enable refresh every 10 seconds
		RefreshOptions: azureappconfiguration.KeyValueRefreshOptions{
			Enabled:  true,
			Interval: 10 * time.Second,
		},
	}

	authOptions := azureappconfiguration.AuthenticationOptions{
		ConnectionString: connectionString,
	}

	// Create configuration provider with timeout
	ctx, cancel := context.WithTimeout(context.Background(), 10*time.Second)
	defer cancel()

	return azureappconfiguration.Load(ctx, authOptions, options)
}

// displayConfig prints the current configuration values
func displayConfig(config Config) {
	fmt.Println("\nCurrent Configuration Values:")
	fmt.Println("--------------------")
	fmt.Printf("Font Color: %s\n", config.Font.Color)
	fmt.Printf("Font Size: %d\n", config.Font.Size)
	fmt.Printf("Message: %s\n", config.Message)
	fmt.Println("--------------------")
}
