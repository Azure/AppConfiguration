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
from typing import Dict, Any
from azure.core.credentials import TokenCredential
from azure.identity import DefaultAzureCredential, get_bearer_token_provider
from azure.appconfiguration.provider import load, SettingSelector, WatchKey
from openai import AzureOpenAI
from models import AzureOpenAIConfiguration, ChatCompletionConfiguration


def main():
    global appconfig, azure_openai_config, chat_completion_config, azure_client, chat_messages, chat_conversation, credential
    # Create a credential using DefaultAzureCredential
    credential = DefaultAzureCredential()

    # Get the App Configuration endpoint from environment variables
    app_config_endpoint = os.environ.get("AZURE_APPCONFIGURATION_ENDPOINT")
    if not app_config_endpoint:
        raise ValueError(
            "The environment variable 'AZURE_APPCONFIGURATION_ENDPOINT' is not set or is empty."
        )

    # Initialize chat conversation
    chat_messages = []
    chat_conversation = []

    def configure_app():
        global appconfig, azure_openai_config, chat_completion_config, azure_client, chat_messages, credential
        azure_openai_config = _extract_openai_config(appconfig)
        azure_client = _create_ai_client(azure_openai_config, credential)

        # Configure chat completion with AI configuration
        chat_completion_config = ChatCompletionConfiguration(
            **appconfig["ChatCompletion"]
        )
        chat_messages = chat_completion_config.messages

    # Create the configuration provider with refresh settings
    appconfig = load(
        endpoint=app_config_endpoint,
        selects=[SettingSelector(key_filter="ChatApp:*")],
        credential=credential,
        keyvault_credential=credential,  # Use the same credential for Key Vault references
        trim_prefixes=["ChatApp:"],
        refresh_on=[WatchKey(key="ChatApp:ChatCompletion")],
        on_refresh_success=configure_app,
    )
    configure_app()

    print("Chat started! What's on your mind?")

    while True:
        # Refresh the configuration from Azure App Configuration
        appconfig.refresh()

        # Get user input
        user_input = input("You: ")

        # Exit if user input is empty
        if not user_input.strip():
            print("Exiting chat. Goodbye!")
            break

        # Add user message to chat conversation
        chat_conversation.append({"role": "user", "content": user_input})

        # Get latest system message from AI configuration
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
        chat_conversation.append({"role": "assistant", "content": ai_response})
        print(f"AI: {ai_response}")


def _create_ai_client(
    azure_openai_config: AzureOpenAIConfiguration, credential: TokenCredential = None
) -> AzureOpenAI:
    # Create an Azure OpenAI client
    if azure_openai_config.api_key:
        return AzureOpenAI(
            azure_endpoint=azure_openai_config.endpoint,
            api_key=azure_openai_config.api_key,
            api_version=azure_openai_config.api_version,
        )
    else:
        return AzureOpenAI(
            azure_endpoint=azure_openai_config.endpoint,
            azure_ad_token_provider=get_bearer_token_provider(
                credential or DefaultAzureCredential(),
                "https://cognitiveservices.azure.com/.default",
            ),
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


if __name__ == "__main__":
    main()
