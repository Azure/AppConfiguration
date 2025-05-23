# Azure App Configuration - Python ChatApp Sample

This sample demonstrates using Azure App Configuration to configure Azure OpenAI settings for a chat application built with Python.

## Features

- Integrates with Azure OpenAI for chat completions
- Dynamically refreshes configuration from Azure App Configuration

## Prerequisites

- Python 3.8 or later
- An Azure subscription with access to:
  - Azure App Configuration service
  - Azure OpenAI service
- Required environment variables:
  - `AZURE_APPCONFIG_ENDPOINT`: URL of your Azure App Configuration instance
  - `AZURE_OPENAI_API_KEY`: API key for Azure OpenAI (optional if using DefaultAzureCredential)

## Setup

1. Clone the repository
2. Install the required packages:
   ```bash
   pip install -r requirements.txt
   ```
3. Configure your Azure App Configuration store with these settings:
   ```
   ChatApp:AzureOpenAI:Endpoint - Your Azure OpenAI endpoint URL
   ChatApp:AzureOpenAI:DeploymentName - Your Azure OpenAI deployment name
   ChatApp:Model - An AI configuration entry containing the following settings:
     - model - Model name (e.g., "gpt-35-turbo")
     - max_tokens - Maximum tokens for completion (e.g., 1000)
     - temperature - Temperature parameter (e.g., 0.7)
     - top_p - Top p parameter (e.g., 0.95)
     - messages - An array of messages with role and content for each message
   ```

4. Set the required environment variables:
   ```bash
   export AZURE_APPCONFIG_ENDPOINT="https://your-appconfig.azconfig.io"
   export AZURE_OPENAI_API_KEY="your-openai-api-key"  # Optional if using DefaultAzureCredential
   ```

## Running the Application

Start the console application:
```bash
python app.py
```

The application will display the configured messages and the AI's response. Press Enter to continue and refresh the configuration from Azure App Configuration.

## Configuration Refresh

The application refreshes the configuration on each loop iteration, so any changes made in Azure App Configuration will be reflected in the next response after you press Enter.