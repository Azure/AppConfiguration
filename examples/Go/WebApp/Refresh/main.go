package main

import (
	"context"
	"log"
	"os"
	"sync"
	"time"

	"github.com/Azure/AppConfiguration-GoProvider/azureappconfiguration"
	"github.com/gin-gonic/gin"
)

type Config struct {
	App     App
	Message string
}

type App struct {
	Name      string
	DebugMode bool
}

// Global configuration that will be updated on refresh
var (
	config     Config
	configLock sync.RWMutex
)

// loadConfiguration handles loading the configuration from Azure App Configuration
func loadConfiguration() (*azureappconfiguration.AzureAppConfiguration, error) {
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

// updateConfig safely updates the global configuration
func updateConfig(newConfig Config) {
	configLock.Lock()
	defer configLock.Unlock()
	config = newConfig
}

// getConfig safely retrieves the global configuration
func getConfig() Config {
	configLock.RLock()
	defer configLock.RUnlock()
	return config
}

// configRefreshMiddleware is a Gin middleware that attempts to refresh configuration on incoming requests
func configRefreshMiddleware(appCfgProvider *azureappconfiguration.AzureAppConfiguration) gin.HandlerFunc {
	return func(c *gin.Context) {
		// Start refresh in a goroutine to avoid blocking the request
		go func() {
			ctx, cancel := context.WithTimeout(context.Background(), 5*time.Second)
			defer cancel()

			if err := appCfgProvider.Refresh(ctx); err != nil {
				// Just log the error, don't interrupt request processing
				log.Printf("Error refreshing configuration: %s", err)
			}
		}()

		// Continue processing the request
		c.Next()
	}
}

// setupRouter creates and configures the Gin router
func setupRouter(appCfgProvider *azureappconfiguration.AzureAppConfiguration) *gin.Engine {
	// Get the current config
	currentConfig := getConfig()

	if currentConfig.App.DebugMode {
		// Set Gin to debug mode
		gin.SetMode(gin.DebugMode)
		log.Println("Running in DEBUG mode")
	} else {
		// Set Gin to release mode for production
		gin.SetMode(gin.ReleaseMode)
		log.Println("Running in RELEASE mode")
	}

	// Initialize Gin router
	r := gin.Default()

	// Apply our configuration refresh middleware to all routes
	r.Use(configRefreshMiddleware(appCfgProvider))

	// Load HTML templates
	r.LoadHTMLGlob("templates/*")

	// Define a route for the homepage
	r.GET("/", func(c *gin.Context) {
		// Get the latest config for each request
		currentConfig := getConfig()
		c.HTML(200, "index.html", gin.H{
			"Title":   "Home",
			"Message": currentConfig.Message,
			"App":     currentConfig.App.Name,
		})
	})

	// Define a route for the About page
	r.GET("/about", func(c *gin.Context) {
		c.HTML(200, "about.html", gin.H{
			"Title": "About",
		})
	})

	return r
}

func main() {
	// Load initial configuration
	appCfgProvider, err := loadConfiguration()
	if err != nil {
		log.Fatalf("Error loading configuration: %s", err)
	}

	// Parse configuration into struct and update the global config
	var initialConfig Config
	err = appCfgProvider.Unmarshal(&initialConfig, nil)
	if err != nil {
		log.Fatalf("Error unmarshalling configuration: %s", err)
	}
	updateConfig(initialConfig)

	// Register refresh callback
	appCfgProvider.OnRefreshSuccess(func() {
		log.Println("Configuration changed! Updating values...")

		// Re-unmarshal the configuration
		var updatedConfig Config
		err := appCfgProvider.Unmarshal(&updatedConfig, nil)
		if err != nil {
			log.Printf("Error unmarshalling updated configuration: %s", err)
			return
		}

		// Update our working config
		updateConfig(updatedConfig)

		// Log the changes
		log.Printf("Updated configuration: Message=%s, App.Name=%s, App.DebugMode=%v",
			updatedConfig.Message, updatedConfig.App.Name, updatedConfig.App.DebugMode)
	})

	// Setup the router with refresh middleware
	r := setupRouter(appCfgProvider)

	// Start the server
	log.Println("Starting server on :8080")
	if err := r.Run(":8080"); err != nil {
		log.Fatalf("Error starting server: %s", err)
	}
}