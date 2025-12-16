from azure.appconfiguration.provider import (
    load,
    SettingSelector
)
from azure.identity import DefaultAzureCredential
import os

endpoint = os.environ.get("AZURE_APPCONFIGURATION_ENDPOINT")

# Connect to Azure App Configuration using Microsoft Entra ID authentication.
config = load(endpoint=endpoint, credential=DefaultAzureCredential())

# Find the key "message" and print its value.
print(config["message"])
