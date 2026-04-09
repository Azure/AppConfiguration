# Azure App Configuration - AI Agent chat application

This sample demonstrates using Azure App Configuration to load agent YAML specifications that define AI agent behavior, prompts, and model configurations for a chat application.

## Features
- Integrates with Azure AI Agent Framework to create a conversational AI agent
- Loads agent YAML specifications from Azure App Configuration.

## Prerequisites

- .NET 10 SDK 
- An Azure subscription with:
  - An Azure App Configuration store
  - An Azure AI project with a deployed gpt-5 model.
- User has **App Configuration Reader** role assigned for the Azure App Configuration resource.
- User has **Azure AI User** role assigned for the Azure AI project.

## Setup

1. Clone the repository and navigate to the `examples\DotNetCore\ChatAgent` directory:
   ```bash
   git clone https://github.com/Azure/AppConfiguration.git
   cd examples\DotNetCore\ChatAgent
   ```

1. Install the required packages:

    ```bash
    dotnet restore
    ```

1. Add the following key-values to your Azure App Configuration store.

   | Key | Value |
   |-----|-------|
   | ChatAgent:Spec | _See YAML below_ |
   | ChatAgent:ProjectEndpoint | _Your Azure AI project endpoint_ |
   | ChatAgent:DeploymentName| gpt-5 |

   **YAML specification for _ChatAgent:Spec_**
   ```yaml
    kind: Prompt
    name: ChatAgent
    description: Agent example with web search
    instructions: You are a helpful assistant with access to web search.
    model:
        id: gpt-5
        connection:
            kind: remote
    tools:
      - kind: webSearch
        name: WebSearchTool
        description: Search the web for live information.
   ```

1. Set the required environment variable:

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

    ```cmd
   dotnet run
   ```

1. Type the message "What is the weather in Seattle today?" when prompted with "How can I help?" and then press the Enter key

   ```Output
   How can I help? (type 'quit' to exit)
   User: What is the weather in Seattle today ?
   Agent response: Seattle weather for today (Thursday, April 9, 2026):

   - Current conditions (as of ~10:48 AM PDT): 55°F, sunny. Wind N 6 mph (gusts 7), humidity 55%, pressure 30.05 in. ([wunderground.com](https://www.wunderground.com/weather/us/wa/seattle))
   - Today’s forecast: Mostly sunny and mild. High around 64–65°F; tonight’s low near 43–44°F. Very low chance of precipitation and light winds. ([wunderground.com](https://www.wunderground.com/weather/us/wa/seattle))

   Want the hour‑by‑hour forecast or weekend outlook?
   Press enter to continue...
   ```