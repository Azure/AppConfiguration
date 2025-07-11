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
from azure.identity import DefaultAzureCredential, get_bearer_token_provider
from azure.appconfiguration.provider import load, SettingSelector, WatchKey
from openai import AzureOpenAI
from models import AzureOpenAIConfiguration, ChatCompletionConfiguration

APP_CONFIG_ENDPOINT_KEY = "AZURE_APPCONFIGURATION_ENDPOINT"
CHAT_APP_KEY = "ChatApp:"
AZURE_OPENAI_KEY = "AzureOpenAI:"
CHAT_COMPLETION_KEY = "ChatCompletion"
BEARER_SCOPE = "https://cognitiveservices.azure.com/.default"


# Initialize CREDENTIAL and config
CREDENTIAL = DefaultAzureCredential()

# Initialize chat state
APPCONFIG = None
CHAT_COMPLETION_CONFIG = None


def main():
    global APPCONFIG
    app_config_endpoint = os.environ.get(APP_CONFIG_ENDPOINT_KEY)
    if not app_config_endpoint:
        raise ValueError(f"The environment variable '{APP_CONFIG_ENDPOINT_KEY}' is not set or is empty.")

    # Load configuration
    APPCONFIG = load(
        endpoint=app_config_endpoint,
        selects=[SettingSelector(key_filter=f"{CHAT_APP_KEY}*")],
        credential=CREDENTIAL,
        keyvault_credential=CREDENTIAL,
        trim_prefixes=[CHAT_APP_KEY],
        refresh_on=[WatchKey(key=f"{CHAT_APP_KEY}Sentinel")],
        on_refresh_success=configure_app,
    )
    configure_app()

    print("Chat started! What's on your mind?")

    azure_openai_config = AzureOpenAIConfiguration(
        api_key=APPCONFIG.get(f"{AZURE_OPENAI_KEY}ApiKey", ""),
        endpoint=APPCONFIG.get(f"{AZURE_OPENAI_KEY}Endpoint", ""),
        deployment_name=APPCONFIG.get(f"{AZURE_OPENAI_KEY}DeploymentName", ""),
        api_version=APPCONFIG.get(f"{AZURE_OPENAI_KEY}ApiVersion", ""),
    )
    azure_client = create_ai_client(azure_openai_config)

    chat_messages = []

    while True:
        # Refresh the configuration from Azure App Configuration
        APPCONFIG.refresh()

        # Get user input
        user_input = input("You: ")

        # Exit if user input is empty
        if not user_input.strip():
            print("Exiting chat. Goodbye!")
            break

        # Add user message to chat conversation
        chat_messages.append({"role": "user", "content": user_input})

        chat_conversation = CHAT_COMPLETION_CONFIG.messages
        chat_conversation.extend(chat_messages)

        # Get AI response and add it to chat conversation
        response = azure_client.chat.completions.create(
            model=azure_openai_config.deployment_name,
            messages=chat_conversation,
            max_tokens=CHAT_COMPLETION_CONFIG.max_tokens,
            temperature=CHAT_COMPLETION_CONFIG.temperature,
            top_p=CHAT_COMPLETION_CONFIG.top_p,
        )

        ai_response = response.choices[0].message.content
        chat_messages.append({"role": "assistant", "content": ai_response})
        print(f"AI: {ai_response}")


def configure_app():
    """
    Configure the chat application with settings from Azure App Configuration.
    """
    global CHAT_COMPLETION_CONFIG
    # Configure chat completion with AI configuration
    CHAT_COMPLETION_CONFIG = ChatCompletionConfiguration(**APPCONFIG[CHAT_COMPLETION_KEY])


def create_ai_client(azure_open_ai_config: AzureOpenAIConfiguration) -> AzureOpenAI:
    """
    Create an Azure OpenAI client using the configuration from Azure App Configuration.
    """
    if azure_open_ai_config.api_key:
        return AzureOpenAI(
            azure_endpoint=azure_open_ai_config.endpoint,
            api_key=azure_open_ai_config.api_key,
            api_version=azure_open_ai_config.api_version,
            azure_deployment=azure_open_ai_config.deployment_name,
        )
    else:
        return AzureOpenAI(
            azure_endpoint=azure_open_ai_config.endpoint,
            azure_ad_token_provider=get_bearer_token_provider(
                CREDENTIAL,
                BEARER_SCOPE,
            ),
            api_version=azure_open_ai_config.api_version,
            azure_deployment=APPCONFIG.get(f"{AZURE_OPENAI_KEY}DeploymentName", ""),
        )


if __name__ == "__main__":
    main()
