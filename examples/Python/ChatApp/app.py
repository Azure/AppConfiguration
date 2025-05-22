# Copyright (c) Microsoft Corporation.
# Licensed under the MIT license.
#
import os
import time
from flask import Flask, render_template, request
from azure.identity import DefaultAzureCredential
from azure.appconfiguration.provider import load, SettingSelector
from openai import AzureOpenAI
from models import ModelConfiguration

# Initialize Flask app
app = Flask(__name__)

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
    app.config.update(azure_app_config)
    print("Configuration refreshed successfully")

global azure_app_config
azure_app_config = load(
    endpoint=ENDPOINT,
    selects=[chat_app_selector],
    credential=credential,
    keyvault_credential=credential,
    on_refresh_success=callback,
)

# Update Flask app config with Azure App Configuration
app.config.update(azure_app_config)

# Get OpenAI configuration
def get_openai_client():
    """Create and return an Azure OpenAI client"""
    endpoint = app.config.get("ChatApp:AzureOpenAI:Endpoint")
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
    """Get model configuration from app config"""
    model_config = {}
    
    # Extract model configuration from app.config
    prefix = "ChatApp:Model:"
    for key in app.config:
        if key.startswith(prefix):
            # Get the part of the key after the prefix
            config_key = key[len(prefix):].lower()
            model_config[config_key] = app.config[key]
    
    # Handle messages specially (they're nested)
    messages = []
    messages_prefix = "ChatApp:Model:messages:"
    
    # Group message configs by index
    message_configs = {}
    for key in app.config:
        if key.startswith(messages_prefix):
            # Extract the index and property (e.g., "0:role" -> ("0", "role"))
            parts = key[len(messages_prefix):].split(':')
            if len(parts) == 2:
                index, prop = parts
                if index not in message_configs:
                    message_configs[index] = {}
                message_configs[index][prop.lower()] = app.config[key]
    
    # Create message list in the right order
    for index in sorted(message_configs.keys()):
        messages.append(message_configs[index])
    
    model_config['messages'] = messages
    
    return ModelConfiguration.from_dict(model_config)

def get_chat_messages(model_config):
    """Convert from model configuration messages to OpenAI messages format"""
    return [{"role": msg.role, "content": msg.content} for msg in model_config.messages]

@app.route('/')
def index():
    """Main route to display and handle chat"""
    # Refresh configuration from Azure App Configuration
    azure_app_config.refresh()
    
    # Get model configuration
    model_config = get_model_configuration()
    
    # Get OpenAI client
    client = get_openai_client()
    
    # Get deployment name
    deployment_name = app.config.get("ChatApp:AzureOpenAI:DeploymentName")
    
    # Get chat history from model config for display
    chat_history = [{"role": msg.role, "content": msg.content} for msg in model_config.messages]
    
    # Check if a new message was submitted
    if request.args.get('message'):
        user_message = request.args.get('message')
        
        # Add user message to chat messages
        messages = get_chat_messages(model_config)
        messages.append({"role": "user", "content": user_message})
        
        # Add user message to chat history
        chat_history.append({"role": "user", "content": user_message})
        
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
        
        # Add assistant message to chat history
        chat_history.append({"role": "assistant", "content": assistant_message})
    
    return render_template('index.html', 
                           chat_history=chat_history,
                           model_config=model_config)

@app.route('/console')
def console():
    """Console mode similar to the .NET Core example"""
    # Refresh configuration from Azure App Configuration
    azure_app_config.refresh()
    
    # Get model configuration
    model_config = get_model_configuration()
    
    # Get OpenAI client
    client = get_openai_client()
    
    # Get deployment name
    deployment_name = app.config.get("ChatApp:AzureOpenAI:DeploymentName")
    
    # Display messages from configuration
    messages_display = ""
    for msg in model_config.messages:
        messages_display += f"{msg.role}: {msg.content}\n"
    
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
    
    return render_template('console.html', 
                           messages=messages_display,
                           response=assistant_message)

if __name__ == '__main__':
    app.run(debug=True)