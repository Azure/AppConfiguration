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

## Setup

1. Clone the repository and navigate to the `examples\Python\ChatAgent` directory:
   ```bash
   git clone https://github.com/Azure/AppConfiguration.git
   cd examples\Python\ChatAgent
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

1. Add the following key-values to your Azure App Configuration store.

   | Key | Value |
   |-----|-------|
   | ChatAgent:Spec | _See YAML below_ |
   | CharAgent:ProjectEndpoint | _Your Foundry project endpoint_ |

   **YAML specification for _ChatAgent:Spec_:**
   ```yaml
    kind: Prompt
    name: ChatAgent
    description: Agent example with web search
    instructions: You are a helpful assistant with access to web search.
    model:
        id: gpt-4.1
        connection:
            kind: remote
    tools:
      - kind: web_search
        name: WebSearchTool
        description: Search the web for live information.
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

1. Start the console application:

   ```bash
   python app.py
   ```

2. Type the message "What is the weather in Seattle today?" when prompted with "How can I help?" and then press the Enter key

   ```output
   How can I help? (type 'quit' to exit)
   User: What is the weather today in Seattle ?     
   Agent response:  Today in Seattle, expect steady rain throughout the day with patchy fog, and a high temperature around 57°F (14°C). 
   Winds are from the south-southwest at 14 to 17 mph, with gusts as high as 29 mph. Flood and wind advisories are in effect due to ongoing heavy rain and saturated conditions. Rain is likely to continue into the night, with a low near 49°F. Please stay aware of weather alerts if you are traveling or in low-lying areas [National Weather Service Seattle](https://forecast.weather.gov/zipcity.php?inputstring=Seattle%2CWA) [The Weather Channel Seattle Forecast](https://weather.com/weather/today/l/Seattle+Washington?canonicalCityId=1138ce33fd1be51ab7db675c0da0a27c).
   Press enter to continue...
   ```

