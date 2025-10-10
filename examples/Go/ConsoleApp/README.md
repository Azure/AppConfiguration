# Azure App Configuration Console Refresh Example

This example demonstrates how to use the refresh functionality of Azure App Configuration in a console/command-line application.

## Overview

This console application:

1. Loads configuration values from Azure App Configuration
2. Binds them to target configuration struct
3. Automatically refreshes the configuration when changed in Azure App Configuration

## Running the Example

### Prerequisites

You need [an Azure subscription](https://azure.microsoft.com/free/) and the following Azure resources to run the examples:

- [Azure App Configuration store](https://learn.microsoft.com/en-us/azure/azure-app-configuration/quickstart-azure-app-configuration-create?tabs=azure-portal)

The examples retrieve credentials to access your App Configuration store from environment variables.

### Add key-values

Add the following key-values to the App Configuration store and leave **Label** and **Content Type** with their default values:

| Key                    | Value          |
|------------------------|----------------|
| *Config.Message*       | *Hello World!* |
| *Config.Font.Color*    | *blue*         |
| *Config.Font.Size*     | *12*           |

### Setup

1. Initialize a new Go module.

    ```bash
    go mod init console-example-refresh
    ```
1. Add the Azure App Configuration provider as a dependency.

    ```bash
    go get github.com/Azure/AppConfiguration-GoProvider/azureappconfiguration
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

### Testing the Refresh Functionality

1. Start the application
2. While it's running, modify the values in your Azure App Configuration store
3. Within 10 seconds (the configured refresh interval), the application should detect and apply the changes
4. You don't need to restart the application to see the updated values