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

1. Clone the repository and navigate to the `examples\Python\WeatherAgent` directory:
   ```bash
   git clone https://github.com/Azure/AppConfiguration.git
   cd examples\Python\WeatherAgent
   ```

1. Next, create a new Python virtual environment where you can safely install the packages:

   On macOS or Linux run the following command:
   ```bash
   python -m venv .venv
   source .venv/bin/activate
   ```

   On Windows run:
   ```bash
   python -m venv .venv
   .venv\scripts\activate
   ```

1. Install the required packages:

   ```bash
   pip install -r requirements.txt
   ```

1. Configure your Azure App Configuration store with the following key-value pairs:

   | Key | Value |
   |-----|-------|
   | WeatherAgent:Spec | _See YAML below_ |
   | WeatherAgent:ProjectEndpoint | _Your Foundry project endpoint_ |

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

   If you use the Windows command prompt, run the following command and restart the command prompt to allow the change to take effect:

   ```cmd
   setx AZURE_APPCONFIGURATION_ENDPOINT "<endpoint-of-your-app-configuration-store>"
   ```

   If you use PowerShell, run the following command:
   ```powershell
   $Env:AZURE_APPCONFIGURATION_ENDPOINT="<endpoint-of-your-app-configuration-store>"
   ```

   If you use macOS or Linux run the following command:
   ```bash
   export AZURE_APPCONFIGURATION_ENDPOINT='<endpoint-of-your-app-configuration-store>'
   ```

## Run the Application

Start the console application:

```bash
python app.py
```