# Azure App Configuration Gin Web Example

This example demonstrates how to use Azure App Configuration in a web application built with the Gin framework.

## Overview

This web application:

1. Loads configuration values from Azure App Configuration
2. Configures the Gin web framework based on those values

## Running the Example

### Prerequisites

You need [an Azure subscription](https://azure.microsoft.com/free/) and the following Azure resources to run the examples:

- [Azure App Configuration store](https://learn.microsoft.com/en-us/azure/azure-app-configuration/quickstart-azure-app-configuration-create?tabs=azure-portal)

The examples retrieve credentials to access your App Configuration store from environment variables.

### Add key-values

Add the following key-values to the App Configuration store and leave **Label** and **Content Type** with their default values:

| Key                    | Value              |
|------------------------|--------------------|
| *Config.Message*       | *Hello World!*     |
| *Config.App.Name*      | *Gin Web App*      |
| *Config.App.DebugMode* | *true*             |

### Setup

1. Initialize a new Go module.

    ```bash
    go mod init gin-example-app
    ```
1. Add the required dependencies.

    ```bash
    go get github.com/Azure/AppConfiguration-GoProvider/azureappconfiguration
    go get github.com/gin-gonic/gin
    ```

1. Set the connection string as an environment variable:

    ```bash
    # Windows
    set AZURE_APPCONFIG_CONNECTION_STRING=your-connection-string

    # Linux/macOS
    export AZURE_APPCONFIG_CONNECTION_STRING=your-connection-string
    ```

### Run the Application

```bash
go run main.go
```

Then navigate to `http://localhost:8080` in your web browser.