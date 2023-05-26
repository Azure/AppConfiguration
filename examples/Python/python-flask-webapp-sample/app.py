import os
from flask import Flask, render_template
from azure.appconfiguration.provider import load, AzureAppConfigurationKeyVaultOptions, SettingSelector, AzureAppConfigurationRefreshOptions
from azure.identity import DefaultAzureCredential

app = Flask(__name__)


ENDPOINT =  os.environ.get("AZURE_APPCONFIG_ENDPOINT_FLASK")

# Set up credentials and settings used in resolving key vault references.
credential = DefaultAzureCredential()

# Load app configuration key-values and resolved key vault reference values.
# Select only key-values that start with 'testapp_settings_' and trim the prefix
selects = SettingSelector(key_filter="testapp_settings_*")
selects_secret = SettingSelector(key_filter="secret_key")
keyvault_options = AzureAppConfigurationKeyVaultOptions(credential=credential)

# Setting up refresh options to refresh the configurations every 15 seconds if message has changed.
refresh_options = AzureAppConfigurationRefreshOptions()
refresh_options.refresh_interval = 15
refresh_options.register(key_filter="testapp_settings_message", refresh_all=True)
azure_app_config = load(endpoint=ENDPOINT,
                        key_vault_options=keyvault_options,
                        credential=credential,
                        selects=[selects, selects_secret],
                        trim_prefixes=["testapp_settings_"],
                        refresh_options=refresh_options)

# App Configuration provider implements the Mapping Type which is compatible with the existing Flask config.
# Update Flask config mapping with loaded values in the App Configuration provider.
app.config.update(azure_app_config)

@app.route('/')
@azure_app_config.drefresh
def index():
   app.config.update(azure_app_config)
   print('Request for index page received')
   context = {}
   context['message'] = app.config.get('message')
   context['font_size'] = app.config.get('font_size')
   context['color'] = app.config.get('color')
   context['key'] = app.config.get('secret_key') # This is a key vault reference. The corresponding secret in key vault is returned.
   return render_template('index.html', **context)


if __name__ == '__main__':
   app.run()
