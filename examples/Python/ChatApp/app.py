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


app_config_endpoint_key = "AZURE_APPCONFIGURATION_ENDPOINT"
chat_app_key = "ChatApp:"
azure_openai_key = "AzureOpenAI:"
chat_completion_key = "ChatCompletion"
bearer_scope =  "https://cognitiveservices.azure.com/.default"

class ChatApp:

    def __init__(self):
        # Initialize credential and config
        self._credential = DefaultAzureCredential()

        # Initialize chat state
        self._chat_messages = []
        self._chat_conversation = []

        # Load configuration
        self.appconfig = self._load_config()
        self.configure_app()

    def _get_app_config_endpoint(self):
        app_config_endpoint = os.environ.get(app_config_endpoint_key)
        if not app_config_endpoint:
            raise ValueError(
                f"The environment variable '{app_config_endpoint_key}' is not set or is empty."
            )
        return app_config_endpoint

    def _load_config(self):
        return load(
            endpoint=self._get_app_config_endpoint(),
            selects=[SettingSelector(key_filter=f"{chat_app_key}:*")],
            credential=self._credential,
            keyvault_credential=self._credential,
            trim_prefixes=[chat_app_key],
            refresh_on=[WatchKey(key=f"{chat_app_key}{chat_completion_key}")],
            on_refresh_success=self.configure_app,
        )

    def configure_app(self):
        self.azure_openai_config = self._extract_openai_config()
        self.azure_client = self._create_ai_client()

        # Configure chat completion with AI configuration
        self.chat_completion_config = ChatCompletionConfiguration(
            **self.appconfig[chat_completion_key]
        )
        self._chat_messages = self.chat_completion_config.messages

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
                    self._credential or DefaultAzureCredential(),
                    bearer_scope,
                ),
                api_version=self.azure_openai_config.api_version,
            )

    def _extract_openai_config(self) -> AzureOpenAIConfiguration:
        """
        Extract Azure OpenAI configuration from the configuration data.

        :param config_data: The configuration data from Azure App Configuration
        :return: An AzureOpenAIConfiguration object
        """
        return AzureOpenAIConfiguration(
            api_key=self.appconfig.get(f"{azure_openai_key}ApiKey", ""),
            endpoint=self.appconfig.get(f"{azure_openai_key}Endpoint", ""),
            deployment_name=self.appconfig.get(f"{azure_openai_key}DeploymentName", ""),
            api_version=self.appconfig.get(f"{azure_openai_key}ApiVersion", "2023-05-15"),
        )

    def ask(self):
        # Get user input
        user_input = input("You: ")

        # Exit if user input is empty
        if not user_input.strip():
            print("Exiting chat. Goodbye!")
            exit()

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


def main():
    chat_app = ChatApp()

    print("Chat started! What's on your mind?")

    while True:
        # Refresh the configuration from Azure App Configuration
        chat_app.appconfig.refresh()

        chat_app.ask()


if __name__ == "__main__":
    main()
