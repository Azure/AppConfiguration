# Copyright (c) Microsoft Corporation.
# Licensed under the MIT license.
#
import os
import time
from azure.identity import DefaultAzureCredential
from azure.appconfiguration.provider import load, SettingSelector
from openai import AzureOpenAI
from models import ModelConfiguration

# Get Azure App Configuration endpoint from environment variable
ENDPOINT = os.environ.get("AZURE_APPCONFIG_ENDPOINT")
if not ENDPOINT:
    raise ValueError("The environment variable 'AZURE_APPCONFIG_ENDPOINT' is not set or is empty.")

# Initialize Azure credentials
credential = DefaultAzureCredential()

# Create selector for ChatApp configuration
chat_app_selector = SettingSelector(key_filter="ChatApp:*")

# Load configuration from Azure App Configuration
def callback():
    """Callback function for configuration refresh"""
    print("Configuration refreshed successfully")

global config
config = load(
    endpoint=ENDPOINT,
    selects=[chat_app_selector],
    credential=credential,
    keyvault_credential=credential,
    on_refresh_success=callback,
)

# Get OpenAI configuration
def get_openai_client():
    """Create and return an Azure OpenAI client"""
    endpoint = config.get("ChatApp:AzureOpenAI:Endpoint")
    api_key = os.environ.get("AZURE_OPENAI_API_KEY") # Using environment variable for API key
    
    # For DefaultAzureCredential auth
    if not api_key:
        return AzureOpenAI(
            azure_endpoint=endpoint, 
            api_version="2023-05-15",
            azure_ad_token_provider=credential
        )
    
    # For API key auth
    return AzureOpenAI(
        azure_endpoint=endpoint,
        api_key=api_key,
        api_version="2023-05-15"
    )

def get_model_configuration():
    """Get model configuration from config"""
    model_config = {}
    
    # Extract model configuration from config
    prefix = "ChatApp:Model:"
    for key in config:
        if key.startswith(prefix):
            # Get the part of the key after the prefix
            config_key = key[len(prefix):].lower()
            model_config[config_key] = config[key]
    
    # Handle messages specially (they're nested)
    messages = []
    messages_prefix = "ChatApp:Model:messages:"
    
    # Group message configs by index
    message_configs = {}
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
        messages.append(message_configs[index])
    
    model_config['messages'] = messages
    
    return ModelConfiguration.from_dict(model_config)

def get_chat_messages(model_config):
    """Convert from model configuration messages to OpenAI messages format"""
    return [{"role": msg.role, "content": msg.content} for msg in model_config.messages]

def main():
    """Main entry point for the console app"""
    # Get OpenAI client
    client = get_openai_client()
    
    # Get deployment name
    deployment_name = config.get("ChatApp:AzureOpenAI:DeploymentName")
    
    while True:
        # Refresh configuration from Azure App Configuration
        config.refresh()
        
        # Get model configuration
        model_config = get_model_configuration()
        
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
        
        # Wait for user to continue
        print("Press Enter to continue...")
        input()

if __name__ == '__main__':
    main()