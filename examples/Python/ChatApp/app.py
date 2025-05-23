# Copyright (c) Microsoft Corporation.
# Licensed under the MIT license.
#
import os
import time
from azure.identity import DefaultAzureCredential
from azure.appconfiguration.provider import load, SettingSelector
from openai import AzureOpenAI
from models import ModelConfiguration, Message
from typing import Dict, List, Any, Optional, Type, TypeVar

# Get Azure App Configuration endpoint from environment variable
ENDPOINT = os.environ.get("AZURE_APPCONFIG_ENDPOINT")
if not ENDPOINT:
    raise ValueError("The environment variable 'AZURE_APPCONFIG_ENDPOINT' is not set or is empty.")

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

T = TypeVar('T')

# Get OpenAI configuration
def get_openai_client():
    """Create and return an Azure OpenAI client"""
    endpoint = config.get("AzureOpenAI:Endpoint")
    # Get API key from App Configuration or fall back to environment variable
    api_key = config.get("AzureOpenAI:ApiKey", os.environ.get("AZURE_OPENAI_API_KEY"))
    api_version = config.get("AzureOpenAI:ApiVersion", "2023-05-15")  # Read API version from config or use default
    
    # For DefaultAzureCredential auth if no API key is available
    if not api_key:
        return AzureOpenAI(
            azure_endpoint=endpoint, 
            api_version=api_version,
            azure_ad_token_provider=credential
        )
    
    # For API key auth
    return AzureOpenAI(
        azure_endpoint=endpoint,
        api_key=api_key,
        api_version=api_version
    )

def get_chat_messages(model_config):
    """Convert from model configuration messages to OpenAI messages format"""
    return [{"role": msg.role, "content": msg.content} for msg in model_config.messages]

def main():
    """Main entry point for the console app"""
    # Get OpenAI client
    client = get_openai_client()
    
    # Get deployment name
    deployment_name = config.get("AzureOpenAI:DeploymentName")
    
    while True:
        # Refresh configuration from Azure App Configuration
        config.refresh()
        
        # Get model configuration using data binding
        model_config = ModelConfiguration.from_dict( config.get("Model"))
        
        # Display messages from configuration
        for msg in model_config.messages:
            print(f"{msg.role}: {msg.content}")
        
        # Get chat messages for the API
        messages = get_chat_messages(model_config)
        
        # Get response from OpenAI
        response = client.chat.completions.create(
            model=deployment_name,
            messages=messages,
            max_tokens=model_config.max_tokens,
            temperature=model_config.temperature,
            top_p=model_config.top_p
        )
        
        # Extract assistant message
        assistant_message = response.choices[0].message.content
        
        # Display the response
        print(f"AI response: {assistant_message}")
        
        # Wait for user to continue or exit
        print("Press Enter to continue or 'exit' to quit...")
        user_input = input().strip().lower()
        if user_input == 'exit':
            print("Exiting application...")
            break

if __name__ == '__main__':
    main()