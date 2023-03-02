import os
from flask import Flask, render_template, send_from_directory
from azure.appconfiguration.provider import load_provider, AzureAppConfigurationKeyVaultOptions
from azure.identity import DefaultAzureCredential

app = Flask(__name__)


ENDPOINT =  os.environ.get("AZURE_APPCONFIG_ENDPOINT")

# Set up credentials and settings used in resolving key vault references.
credential = DefaultAzureCredential()
keyvault_options = AzureAppConfigurationKeyVaultOptions(credential=credential)

# Load app configuration key-values and resolved key vault reference values.
azure_app_config = load_provider(endpoint=ENDPOINT, key_vault_options=keyvault_options, credential=credential)

# App Configuration provider implements the Mapping Type which is compatible with the existing Flask config.
# Update Flask config mapping with loaded values in the App Configuration provider.
app.config.update(azure_app_config)

@app.route('/')
def index():
   print('Request for index page received')
   context = {}
   context['name'] = app.config.get('name')
   context['font_size'] = app.config.get('font_size')
   context['color'] = app.config.get('color')
   context['key'] = app.config.get('secret_key') # This is a key vault reference. The corresponding secret in key vault is returned.
   return render_template('index.html', **context)

@app.route('/favicon.ico')
def favicon():
    return send_from_directory(os.path.join(app.root_path, 'static'),
                               'favicon.ico', mimetype='image/vnd.microsoft.icon')


if __name__ == '__main__':
   app.run()
