# Azure App Configuration - Go ChatApp Sample

An interactive console chat application that integrates with Azure OpenAI services using Azure App Configuration for dynamic AI Configuration management.

## Overview

This Go console application provides a seamless chat experience with Azure OpenAI, featuring:

- Integration with Azure OpenAI for chat completions
- Dynamic AI configuration refresh from Azure App Configuration
- Secure authentication options using API key or Microsoft Entra ID

## Prerequisites

- Go 1.23 or later
- Azure subscription
- Azure OpenAI service instance
- Azure App Configuration service instance

## Setup

### Environment Variables

Set the following environment variable:

- `AZURE_APPCONFIGURATION_ENDPOINT`: Endpoint URL of your Azure App Configuration instance

### Azure App Configuration Keys

Configure the following keys in your Azure App Configuration:

#### Azure OpenAI Connection Settings

- `ChatApp:AzureOpenAI:Endpoint` - Your Azure OpenAI endpoint URL
- `ChatApp:AzureOpenAI:APIVersion` - the Azure OpenAI API version to target. See [Azure OpenAI apiversions](https://learn.microsoft.com/en-us/azure/ai-services/openai/reference#rest-api-versioning) for current API versions.
- `ChatApp:AzureOpenAI:ApiKey` - Key Vault reference to the API key for Azure OpenAI (optional)

#### Chat Completion Configuration

- `ChatApp:ChatCompletion` - An AI Configuration for chat completion containing the following settings:
  - `model` - Model name (e.g., "gpt-4o")
  - `max_tokens` - Maximum tokens for completion (e.g., 1000)
  - `temperature` - Temperature parameter (e.g., 0.7)
  - `top_p` - Top p parameter (e.g., 0.95)
  - `messages` - An array of messages with role and content for each message

## Authentication

The application supports the following authentication methods:

- **Azure App Configuration**: Uses `DefaultAzureCredential` for authentication via Microsoft Entra ID.
- **Azure OpenAI**: Supports authentication using either an API key or `DefaultAzureCredential` via Microsoft Entra ID.
- **Azure Key Vault** *(optional, if using Key Vault references for API keys)*: Authenticates using `DefaultAzureCredential` via Microsoft Entra ID.

## Usage

1. **Initialize a new Go module**: `go mod init chatapp-quickstart`
1. **Install dependencies**: `go mod tidy`
1. **Start the Application**: Run the application using `go run main.go`
1. **Begin Chatting**: Type your messages when prompted with "You: "
1. **Continue Conversation**: The AI will respond and maintain conversation context
1. **Exit**: Press Enter without typing a message to exit gracefully

### Example Session
```
Chat started! What's on your mind?
You: Hello, how are you?
AI: Hello! I'm doing well, thank you for asking. How can I help you today?

You: What can you tell me about machine learning?
AI: Machine learning is a subset of artificial intelligence that focuses on...

You: [Press Enter to exit]
Exiting chat. Goodbye!
```

## Troubleshooting

**"AZURE_APPCONFIGURATION_ENDPOINT environment variable not set"**
- Ensure the environment variable is properly set
- Verify the endpoint URL is correct

**Authentication Failures**
- Ensure you have the `App Configuration Data Reader` role on the Azure App Configuration instance
- For Microsoft Entra ID authentication: Verify you have the `Cognitive Services OpenAI User` role on the Azure OpenAI instance
- For API key authentication: 
  - Confirm you have secret read access to the Key Vault storing the API key
  - Verify that a Key Vault reference for the API key is properly configured in Azure App Configuration

**No AI Response**
- Verify deployment name matches your Azure OpenAI deployment
- Check token limits and quotas