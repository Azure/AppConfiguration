# Azure App Configuration - Python ChatApp Sample

This sample demonstrates using Azure App Configuration to configure Azure OpenAI settings for a chat application built with Python and Flask.

## Features

- Built with Python and Flask
- Uses azure-appconfiguration-provider for configuration management
- Integrates with Azure OpenAI for chat completions
- Dynamically refreshes configuration from Azure App Configuration
- Supports both web interface and console-like views

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
   ChatApp:Model:model - Model name (e.g., "gpt-35-turbo")
   ChatApp:Model:max_tokens - Maximum tokens for completion (e.g., 1000)
   ChatApp:Model:temperature - Temperature parameter (e.g., 0.7)
   ChatApp:Model:top_p - Top p parameter (e.g., 0.95)
   ChatApp:Model:messages:0:role - Role for message 0 (e.g., "system")
   ChatApp:Model:messages:0:content - Content for message 0
   ChatApp:Model:messages:1:role - Role for message 1 (e.g., "user")
   ChatApp:Model:messages:1:content - Content for message 1
   ```

4. Set the required environment variables:
   ```bash
   export AZURE_APPCONFIG_ENDPOINT="https://your-appconfig.azconfig.io"
   export AZURE_OPENAI_API_KEY="your-openai-api-key"  # Optional if using DefaultAzureCredential
   ```

## Running the Application

Start the Flask application:
```bash
python app.py
```

The application will be available at http://localhost:5000/

## Views

- **Chat Interface** (/) - A web interface for chatting with the AI
- **Console Mode** (/console) - A view that mimics the .NET Core console application experience

## Configuration Refresh

The application refreshes the configuration on each request, so any changes made in Azure App Configuration will be reflected immediately in the app.