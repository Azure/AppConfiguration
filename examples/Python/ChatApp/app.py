# -------------------------------------------------------------------------
# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License. See License.txt in the project root for
# license information.
# --------------------------------------------------------------------------
"""
Azure OpenAI Chat Application using Azure App Configuration.
This script demonstrates how to create a chat application that uses Azure App Configuration
to manage settings and Azure OpenAI to power chat interactions.
"""

import os
from typing import List, Dict, Any
from azure.core.credentials import TokenCredential
from azure.identity import DefaultAzureCredential, get_bearer_token_provider
from azure.appconfiguration.provider import load, SettingSelector, WatchKey
from openai import AzureOpenAI
from models import AzureOpenAIConfiguration, Message, ModelConfiguration


def main():
    # Create a credential using DefaultAzureCredential
    credential = DefaultAzureCredential()
    refresher = None

    # Get the App Configuration endpoint from environment variables
    app_config_endpoint = os.environ.get("AZURE_APPCONFIGURATION_ENDPOINT")
    if not app_config_endpoint:
        raise ValueError(
            "The environment variable 'AZURE_APPCONFIGURATION_ENDPOINT' is not set or is empty."
        )

    try:
        azure_openai_config = None
        azure_client = None

        def on_refresh_success():
            azure_openai_config, azure_client = _create_ai_client(provider)

        chat_app_selector = SettingSelector(key_filter="ChatApp:*")

        global provider
        # Create the configuration provider with refresh settings
        provider = load(
            endpoint=app_config_endpoint,
            selects=[chat_app_selector],
            credential=credential,
            keyvault_credential=credential,  # Use the same credential for Key Vault references
            trim_prefixes=["ChatApp:"],
            refresh_on=[WatchKey(key="ChatApp:ChatCompletion:Messages")],
            on_refresh_success=on_refresh_success,
        )

        azure_openai_config, azure_client = _create_ai_client(provider, credential)

        # Initialize chat conversation
        chat_conversation = []
        print("Chat started! What's on your mind?")

        while True:
            # Refresh the configuration from Azure App Configuration
            provider.refresh()

            # Configure chat completion with AI configuration
            chat_completion_config = _extract_chat_completion_config(provider)

            # Get user input
            user_input = input("You: ")

            # Exit if user input is empty
            if not user_input.strip():
                print("Exiting chat. Goodbye!")
                break

            # Add user message to chat conversation
            chat_conversation.append({"role": "user", "content": user_input})

            # Get latest system message from AI configuration
            chat_messages = _get_chat_messages(chat_completion_config)
            chat_messages.extend(chat_conversation)

            # Get AI response and add it to chat conversation
            response = azure_client.chat.completions.create(
                model=azure_openai_config.deployment_name,
                messages=chat_messages,
                max_tokens=chat_completion_config.max_tokens,
                temperature=chat_completion_config.temperature,
                top_p=chat_completion_config.top_p,
            )

            ai_response = response.choices[0].message.content
            print(f"AI: {ai_response}")

    except Exception as e:
        print(f"Error: {e}")
    finally:
        # Stop the refresher when done
        if refresher:
            refresher.stop()


def _create_ai_client(
    azure_openai_config: AzureOpenAIConfiguration, credential: TokenCredential = None
) -> AzureOpenAI:
    # Extract Azure OpenAI configuration
    azure_openai_config = _extract_openai_config(provider)
    # Create an Azure OpenAI client
    if azure_openai_config.api_key:
        return azure_openai_config, AzureOpenAI(
            azure_endpoint=azure_openai_config.endpoint,
            api_key=azure_openai_config.api_key,
            api_version=azure_openai_config.api_version,
        )
    token_provider = get_bearer_token_provider(
        credential or DefaultAzureCredential(),
        "https://cognitiveservices.azure.com/.default",
    )
    return azure_openai_config, AzureOpenAI(
        azure_endpoint=azure_openai_config.endpoint,
        azure_ad_token_provider=token_provider,
        api_version=azure_openai_config.api_version,
    )


def _extract_openai_config(config_data: Dict[str, Any]) -> AzureOpenAIConfiguration:
    """
    Extract Azure OpenAI configuration from the configuration data.

    :param config_data: The configuration data from Azure App Configuration
    :return: An AzureOpenAIConfiguration object
    """
    prefix = "AzureOpenAI:"
    return AzureOpenAIConfiguration(
        api_key=config_data.get(f"{prefix}ApiKey", ""),
        endpoint=config_data.get(f"{prefix}Endpoint", ""),
        deployment_name=config_data.get(f"{prefix}DeploymentName", ""),
        api_version=config_data.get(f"{prefix}ApiVersion", "2023-05-15"),
    )


def _extract_chat_completion_config(config_data: Dict[str, Any]) -> ModelConfiguration:
    """
    Extract chat completion configuration from the configuration data.

    :param config_data: The configuration data from Azure App Configuration
    """
    prefix = "ChatApp:ChatCompletion:"

    # Extract messages from configuration
    messages_data = []
    for i in range(10):  # Assuming a reasonable maximum of 10 messages
        role_key = f"{prefix}Messages:{i}:Role"
        content_key = f"{prefix}Messages:{i}:Content"

        if role_key in config_data and content_key in config_data:
            messages_data.append(
                {"role": config_data[role_key], "content": config_data[content_key]}
            )

    messages = [Message.from_dict(msg) for msg in messages_data]

    return ModelConfiguration(
        model=config_data.get(f"{prefix}Model", ""),
        messages=messages,
        max_tokens=config_data.get(f"{prefix}MaxTokens", 1024),
        temperature=config_data.get(f"{prefix}Temperature", 0.7),
        top_p=config_data.get(f"{prefix}TopP", 0.95),
    )


def _get_chat_messages(
    chat_completion_config: ModelConfiguration,
) -> List[Dict[str, str]]:
    """
    Convert configuration messages to chat message dictionaries.

    :param chat_completion_config: The chat completion configuration
    :return: A list of chat message dictionaries
    """
    chat_messages = []

    for message in chat_completion_config.messages:
        if message.role in ["system", "user", "assistant"]:
            chat_messages.append({"role": message.role, "content": message.content})
        else:
            raise ValueError(f"Unknown role: {message.role}")

    return chat_messages


if __name__ == "__main__":
    main()
