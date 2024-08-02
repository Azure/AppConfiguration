from azure.appconfiguration.provider import (
    load,
    SettingSelector
)
import os

connection_string = os.environ.get("AZURE_APPCONFIG_CONNECTION_STRING")

# Connect to Azure App Configuration using a connection string.
config = load(connection_string=connection_string)

# Find the key "message" and print its value.
print(config["message"])