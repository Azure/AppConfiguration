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

# Connect to Azure App Configuration using SettingSelector.
selects = {SettingSelector(key_filter="message*", label_filter="\0")}
config = load(connection_string=connection_string, selects=selects)

# Print True or False to indicate if "message" is found in Azure App Configuration.
print("message found: " + str("message" in config))