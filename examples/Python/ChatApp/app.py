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


class ChatApp:
    """
    Chat Application using Azure OpenAI and Azure App Configuration.
    """

    def __init__(self):
        # Initialize credential and config
        self._credential = DefaultAzureCredential()

        # Initialize chat state
        self._chat_messages = []
        self._chat_conversation = []

        # Load configuration
        self._appconfig = self._load_config()
        self.configure_app()

    def _get_app_config_endpoint(self):
        app_config_endpoint = os.environ.get(APP_CONFIG_ENDPOINT_KEY)
        if not app_config_endpoint:
            raise ValueError(f"The environment variable '{APP_CONFIG_ENDPOINT_KEY}' is not set or is empty.")
        return app_config_endpoint

    def _load_config(self):
        return load(
            endpoint=self._get_app_config_endpoint(),
            selects=[SettingSelector(key_filter=f"{CHAT_APP_KEY}*")],
            credential=self._credential,
            keyvault_credential=self._credential,
            trim_prefixes=[CHAT_APP_KEY],
            refresh_on=[WatchKey(key=f"{CHAT_APP_KEY}{CHAT_COMPLETION_KEY}")],
            on_refresh_success=self.configure_app,
        )

    def configure_app(self):
        """
        Configure the chat application with settings from Azure App Configuration.
        """
        self.azure_openai_config = self._extract_openai_config()
        self.azure_client = self._create_ai_client()

        # Configure chat completion with AI configuration
        self.chat_completion_config = ChatCompletionConfiguration(**self._appconfig[CHAT_COMPLETION_KEY])
        self._chat_messages = self.chat_completion_config.messages

    def _create_ai_client(self) -> AzureOpenAI:
        # Create an Azure OpenAI client
        if self.azure_openai_config.api_key:
            return AzureOpenAI(
                azure_endpoint=self.azure_openai_config.endpoint,
                api_key=self.azure_openai_config.api_key,
                api_version=self.azure_openai_config.api_version,
                azure_deployment=self._appconfig.get(f"{AZURE_OPENAI_KEY}DeploymentName", ""),
            )
        else:
            return AzureOpenAI(
                azure_endpoint=self.azure_openai_config.endpoint,
                azure_ad_token_provider=get_bearer_token_provider(self._credential, BEARER_SCOPE),
                api_version=self.azure_openai_config.api_version,
                azure_deployment=self._appconfig.get(f"{AZURE_OPENAI_KEY}DeploymentName", ""),
            )

    def _extract_openai_config(self) -> AzureOpenAIConfiguration:
        """
        Extract Azure OpenAI configuration from the configuration data.

        :param config_data: The configuration data from Azure App Configuration
        :return: An AzureOpenAIConfiguration object
        """
        return AzureOpenAIConfiguration(
            api_key=self._appconfig.get(f"{AZURE_OPENAI_KEY}ApiKey", ""),
            endpoint=self._appconfig.get(f"{AZURE_OPENAI_KEY}Endpoint", ""),
            api_version=self._appconfig.get(f"{AZURE_OPENAI_KEY}ApiVersion", "2023-05-15"),
        )

    def ask(self):
        """
        Ask a question to the chat application.
        """
        # Refresh the configuration from Azure App Configuration
        self._appconfig.refresh()

        # Get user input
        user_input = input("You: ")

        # Exit if user input is empty
        if not user_input.strip():
            print("Exiting chat. Goodbye!")
            return False  # Stop the conversation

        # Add user message to chat conversation
        self._chat_conversation.append({"role": "user", "content": user_input})

        # Get latest system message from AI configuration
        self._chat_messages.extend(self._chat_conversation)

        # Get AI response and add it to chat conversation
        response = self.azure_client.chat.completions.create(
            model=self.azure_openai_config.deployment_name,
            messages=self._chat_messages,
            max_tokens=self.chat_completion_config.max_tokens,
            temperature=self.chat_completion_config.temperature,
            top_p=self.chat_completion_config.top_p,
        )

        ai_response = response.choices[0].message.content
        self._chat_conversation.append({"role": "assistant", "content": ai_response})
        print(f"AI: {ai_response}")
        return True


def main():
    chat_app = ChatApp()

    print("Chat started! What's on your mind?")

    continue_chat = True
    while continue_chat:
        continue_chat = chat_app.ask()


if __name__ == "__main__":
    main()
