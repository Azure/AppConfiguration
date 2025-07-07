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


class ChatApp:

    def __init__(self):
        # Initialize credential and config
        self.credential = DefaultAzureCredential()
        self.app_config_endpoint = self._get_app_config_endpoint()

        # Initialize chat state
        self.chat_messages = []
        self.chat_conversation = []

        # Load configuration
        self.appconfig = self._load_config()
        self.configure_app()

    def _get_app_config_endpoint(self):
        app_config_endpoint = os.environ.get("AZURE_APPCONFIGURATION_ENDPOINT")
        if not app_config_endpoint:
            raise ValueError(
                "The environment variable 'AZURE_APPCONFIGURATION_ENDPOINT' is not set or is empty."
            )
        return app_config_endpoint

    def _load_config(self):
        return load(
            endpoint=self.app_config_endpoint,
            selects=[SettingSelector(key_filter="ChatApp:*")],
            credential=self.credential,
            keyvault_credential=self.credential,
            trim_prefixes=["ChatApp:"],
            refresh_on=[WatchKey(key="ChatApp:ChatCompletion")],
            on_refresh_success=self.configure_app,
        )

    def configure_app(self):
        self.azure_openai_config = self._extract_openai_config()
        self.azure_client = self._create_ai_client()

        # Configure chat completion with AI configuration
        self.chat_completion_config = ChatCompletionConfiguration(
            **self.appconfig["ChatCompletion"]
        )
        self.chat_messages = self.chat_completion_config.messages

    def _create_ai_client(self) -> AzureOpenAI:
        # Create an Azure OpenAI client
        if self.azure_openai_config.api_key:
            return AzureOpenAI(
                azure_endpoint=self.azure_openai_config.endpoint,
                api_key=self.azure_openai_config.api_key,
                api_version=self.azure_openai_config.api_version,
            )
        else:
            return AzureOpenAI(
                azure_endpoint=self.azure_openai_config.endpoint,
                azure_ad_token_provider=get_bearer_token_provider(
                    self.credential or DefaultAzureCredential(),
                    "https://cognitiveservices.azure.com/.default",
                ),
                api_version=self.azure_openai_config.api_version,
            )

    def _extract_openai_config(self) -> AzureOpenAIConfiguration:
        """
        Extract Azure OpenAI configuration from the configuration data.

        :param config_data: The configuration data from Azure App Configuration
        :return: An AzureOpenAIConfiguration object
        """
        prefix = "AzureOpenAI:"
        return AzureOpenAIConfiguration(
            api_key=self.appconfig.get(f"{prefix}ApiKey", ""),
            endpoint=self.appconfig.get(f"{prefix}Endpoint", ""),
            deployment_name=self.appconfig.get(f"{prefix}DeploymentName", ""),
            api_version=self.appconfig.get(f"{prefix}ApiVersion", "2023-05-15"),
        )

    def ask(self):
        # Get user input
        user_input = input("You: ")

        # Exit if user input is empty
        if not user_input.strip():
            print("Exiting chat. Goodbye!")
            exit()

        # Add user message to chat conversation
        self.chat_conversation.append({"role": "user", "content": user_input})

        # Get latest system message from AI configuration
        self.chat_messages.extend(self.chat_conversation)

        # Get AI response and add it to chat conversation
        response = self.azure_client.chat.completions.create(
            model=self.azure_openai_config.deployment_name,
            messages=self.chat_messages,
            max_tokens=self.chat_completion_config.max_tokens,
            temperature=self.chat_completion_config.temperature,
            top_p=self.chat_completion_config.top_p,
        )

        ai_response = response.choices[0].message.content
        self.chat_conversation.append({"role": "assistant", "content": ai_response})
        print(f"AI: {ai_response}")


def main():
    chat_app = ChatApp()

    print("Chat started! What's on your mind?")

    while True:
        # Refresh the configuration from Azure App Configuration
        chat_app.appconfig.refresh()

        chat_app.ask()


if __name__ == "__main__":
    main()
