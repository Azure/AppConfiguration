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
    key_vault_credential=credential,  # Use the same credential for Key Vault references
    trim_prefixes=["ChatApp:"],
)

T = TypeVar('T')

def bind_config_section(section_prefix: str, model_class: Type[T]) -> T:
    """
    Bind configuration values to a model class.
    
    Args:
        section_prefix: The prefix for the configuration section (without trailing colon)
        model_class: The model class to bind to
        
    Returns:
        An instance of the model class with values from configuration
    """
    config_data: Dict[str, Any] = {}
    
    # Extract all keys for this section
    for key in config:
        if key.startswith(f"{section_prefix}:") or key == section_prefix:
            config_key = key[len(section_prefix):].strip(':').lower()
            if config_key:
                config_data[config_key] = config[key]
            elif key == section_prefix:  # Handle the case where the key is exactly the prefix
                config_data['value'] = config[key]
    
    # Handle nested structures like messages
    messages = []
    messages_prefix = f"{section_prefix}:messages:"
    
    # Group message configs by index
    message_configs: Dict[str, Dict[str, Any]] = {}
    for key in config:
        if key.startswith(messages_prefix):
            # Extract the index and property (e.g., "0:role" -> ("0", "role"))
            parts = key[len(messages_prefix):].split(':')
            if len(parts) == 2:
                index, prop = parts
                if index not in message_configs:
                    message_configs[index] = {}
                message_configs[index][prop.lower()] = config[key]
    
    # Create message list in the right order
    for index in sorted(message_configs.keys()):
        messages.append(Message.from_dict(message_configs[index]))
    
    # Add messages to config data if we found any
    if messages:
        config_data['messages'] = messages
    
    # Create and return the model instance using from_dict
    return model_class.from_dict(config_data)

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
        model_config = bind_config_section("Model", ModelConfiguration)
        
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