# Copyright (c) Microsoft Corporation.
# Licensed under the MIT license.
#
import os
from azure.identity import DefaultAzureCredential, get_bearer_token_provider
from azure.appconfiguration.provider import load, SettingSelector
from openai import AzureOpenAI
from models import ModelConfiguration, Message
from typing import TypeVar, List

# Get Azure App Configuration endpoint from environment variable
ENDPOINT = os.environ.get("AZURE_APPCONFIG_ENDPOINT")
if not ENDPOINT:
    raise ValueError(
        "The environment variable 'AZURE_APPCONFIG_ENDPOINT' is not set or is empty."
    )

# Initialize Azure credentials
credential = DefaultAzureCredential()

# Create selector for ChatApp configuration
chat_app_selector = SettingSelector(key_filter="ChatApp:*")

# Load configuration from Azure App Configuration
global config
config = load(
    endpoint=ENDPOINT,
    selects=[chat_app_selector],
    credential=credential,
    keyvault_credential=credential,  # Use the same credential for Key Vault references
    trim_prefixes=["ChatApp:"],
)

T = TypeVar("T")


# Get OpenAI configuration
def get_openai_client():
    """Create and return an Azure OpenAI client"""
    endpoint = config.get("AzureOpenAI:Endpoint")
    # Get API key from App Configuration or fall back to environment variable
    api_key = config.get("AzureOpenAI:ApiKey", os.environ.get("AZURE_OPENAI_API_KEY"))
    api_version = config.get(
        "AzureOpenAI:ApiVersion", "2023-05-15"
    )  # Read API version from config or use default

    # For DefaultAzureCredential auth if no API key is available
    if not api_key:
        token_provider = get_bearer_token_provider(
            DefaultAzureCredential(), "https://cognitiveservices.azure.com/.default"
        )
        return AzureOpenAI(
            azure_endpoint=endpoint,
            api_version=api_version,
            azure_ad_token_provider=token_provider,
        )

    # For API key auth
    return AzureOpenAI(
        azure_endpoint=endpoint, api_key=api_key, api_version=api_version
    )


def get_chat_messages(messages: List[Message]):
    """Convert from model Message objects to OpenAI messages format"""
    return [{"role": msg.role, "content": msg.content} for msg in messages]


def main():
    """Main entry point for the console app"""
    # Get OpenAI client
    client = get_openai_client()

    # Get deployment name
    deployment_name = config.get("AzureOpenAI:DeploymentName")

    # Initialize conversation history with the configuration messages
    conversation_history = []
    first_run = True

    print("Chat Application - type 'exit' to quit\n")

    while True:
        # Refresh configuration from Azure App Configuration
        config.refresh()

        # Get model configuration using data binding
        model_config = ModelConfiguration.from_dict(config.get("Model"))

        # On first run, initialize conversation history with configuration messages
        # and display the initial messages
        if first_run:
            conversation_history = model_config.messages.copy()
            for msg in conversation_history:
                print(f"{msg.role}: {msg.content}")
            first_run = False

        # Get chat messages for the API
        messages = get_chat_messages(conversation_history)

        # Get response from OpenAI
        response = client.chat.completions.create(
            model=deployment_name,
            messages=messages,
            max_tokens=model_config.max_tokens,
            temperature=model_config.temperature,
            top_p=model_config.top_p,
        )

        # Extract assistant message
        assistant_message = response.choices[0].message.content

        # Display the response
        print(f"assistant: {assistant_message}")

        # Add assistant response to conversation history
        conversation_history.append(
            Message(role="assistant", content=assistant_message)
        )

        # Get user input for the next message
        print("\nuser: ", end="")
        user_input = input().strip()

        # Check if user wants to exit
        if user_input.lower() == "exit":
            print("Exiting application...")
            break

        # Add user input to conversation history
        conversation_history.append(Message(role="user", content=user_input))


if __name__ == "__main__":
    main()
