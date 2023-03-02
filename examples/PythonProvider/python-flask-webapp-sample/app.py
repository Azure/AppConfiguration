import os
from flask import Flask, render_template, send_from_directory
from azure.appconfiguration.provider import load_provider, AzureAppConfigurationKeyVaultOptions
from azure.identity import DefaultAzureCredential

app = Flask(__name__)

ENDPOINT =  os.environ.get("AZURE_APPCONFIG_ENDPOINT")
credential = DefaultAzureCredential()
keyvault_options = AzureAppConfigurationKeyVaultOptions(credential=credential)
azure_app_config = load_provider(endpoint=ENDPOINT, key_vault_options=keyvault_options, credential=credential)
app.config.update(azure_app_config)

@app.route('/')
def index():
   print('Request for index page received')
   context = {}
   context['name'] = app.config.get('name')
   context['language'] = app.config.get('language_code')
   context['key'] = app.config.get('secret_key')
   return render_template('index.html', **context)

@app.route('/favicon.ico')
def favicon():
    return send_from_directory(os.path.join(app.root_path, 'static'),
                               'favicon.ico', mimetype='image/vnd.microsoft.icon')


if __name__ == '__main__':
   app.run()