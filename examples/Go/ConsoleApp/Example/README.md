# Azure App Configuration Console Example

This example demonstrates how to use Azure App Configuration in a console/command-line application.

## Overview

This simple console application:

1. Loads configuration values from Azure App Configuration
2. Binds them to target configuration struct

## Running the Example

### Prerequisites

You need [an Azure subscription](https://azure.microsoft.com/free/) and the following Azure resources to run the examples:

- [Azure App Configuration store](https://learn.microsoft.com/en-us/azure/azure-app-configuration/quickstart-azure-app-configuration-create?tabs=azure-portal)

The examples retrieve credentials to access your App Configuration store from environment variables.
Alternatively, edit the source code to include the appropriate credentials.

### Add key-values

Add the following key-values to the App Configuration store and leave **Label** and **Content Type** with their default values. For more information about how to add key-values to a store using the Azure portal or the CLI, go to [Create a key-value](./quickstart-azure-app-configuration-create.md#create-a-key-value).

| Key                    | Value          |
|------------------------|----------------|
| *Config.Message*       | *Hello World!* |
| *Config.Font.Color*    | *blue*         |
| *Config.Font.Size*     | *12*           |

### Setup

1. Initialize a new Go module.

    ```bash
    go mod init console-example-app
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