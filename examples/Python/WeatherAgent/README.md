# Azure App Configuration - AI Agent chat application

This sample demonstrates using Azure App Configuration to load agent YAML specifications that define AI agent behavior, prompts, and model configurations for a chat application.

## Features

- Integrates with Azure AI Agent Framework to create a conversational AI agent
- Loads agent YAML specifications from Azure App Configuration.

## Prerequisites

- Python 3.8 or later
- An Azure subscription with access to:
  - Azure App Configuration service
  - Microsoft Foundry project
- Required environment variables:
  - `AZURE_APPCONFIGURATION_ENDPOINT`: Endpoint URL of your Azure App Configuration instance

## Setup

1. Clone the repository
1. Install the required packages:

   ```bash
   pip install -r requirements.txt
   ```

1. Configure your Azure App Configuration store with the following key-value pairs:

   | Key | Value |
   |-----|-------|
   | _WeatherAgent:Spec_ | See YAML below |
   | _WeatherAgent:ProjectEndpoint_ | Your Foundry project endpoint |

   **YAML specification for _WeatherAgent:Spec_:**
   ```yaml
   kind: Prompt
   name: WeatherAgent
   description: Weather Agent
   instructions: You are a helpful assistant.
   model:
       id: gpt-4.1
       connection:
           kind: remote
   ```
1. Set the required environment variables:

   ```bash
   export AZURE_APPCONFIGURATION_ENDPOINT="https://your-appconfig.azconfig.io"
   ```

## Run the Application

Start the console application:

```bash
python app.py
```