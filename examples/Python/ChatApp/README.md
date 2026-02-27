# Azure App Configuration - Python ChatApp Sample

This sample demonstrates using Azure App Configuration to configure Azure AI Foundry settings for a chat application built with Python.

## Features

- Integrates with Azure AI Foundry for chat completions
- Dynamically refreshes configuration from Azure App Configuration

## Prerequisites

- Python 3.9 or later
- An Azure subscription with access to:
  - Azure App Configuration service
  - Azure AI Foundry project
- Required environment variables:
  - `AZURE_APPCONFIGURATION_ENDPOINT`: Endpoint URL of your Azure App Configuration instance

## Setup

1. Clone the repository
1. Install the required packages:

   ```bash
   pip install -r requirements.txt
   ```

1. Configure your Azure App Configuration store with these settings:

   ```console
   ChatApp:AzureAIFoundry:Endpoint - Your Azure AI Foundry project endpoint URL
   ChatApp:ChatCompletion - An AI configuration entry containing the following settings:
     - model - Model name (e.g., "gpt-5")
     - messages - An array of messages with role and content for each message
   ChatApp:Sentinel - A sentinel key to trigger configuration refreshes
   ```

1. Set the required environment variables:

   ```bash
   export AZURE_APPCONFIGURATION_ENDPOINT="https://your-appconfig.azconfig.io"
   ```

## Running the Application

Start the console application:

```bash
python app.py
```

The application will:

1. Display the initial configured messages from Azure App Configuration
2. Generate a response from the AI
3. Prompt you to enter your message (Just select enter to quit)
4. Maintain conversation history during the session

## Configuration Refresh

The application refreshes the configuration at the beginning of each conversation cycle, so any changes made to the base configuration in Azure App Configuration will be incorporated into the model parameters (temperature, max_tokens, etc.) while maintaining your ongoing conversation history. Updating the configuration in Azure App Configuration will automatically reflect in the application without requiring a restart, once the `ChatApp:Sentinel` key is updated.
