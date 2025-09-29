package main

import (
	"context"
	"log"
	"os"
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
	// Load configuration
	config, err := loadConfiguration()
	if err != nil {
		log.Fatalf("Error loading configuration: %s", err)
	}

	if config.App.DebugMode {
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

	// Load HTML templates
	r.LoadHTMLGlob("templates/*")

	// Define a route for the homepage
	r.GET("/", func(c *gin.Context) {
		c.HTML(200, "index.html", gin.H{
			"Title":   "Home",
			"Message": config.Message,
			"App":     config.App.Name,
		})
	})

	// Define a route for the About page
	r.GET("/about", func(c *gin.Context) {
		c.HTML(200, "about.html", gin.H{
			"Title": "About",
		})
	})

	// Start the server
	if err := r.Run(":8080"); err != nil {
		log.Fatalf("Error starting server: %s", err)
	}
}