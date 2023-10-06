import os
import asyncio
from flask import Flask, render_template
from azure.appconfiguration.provider import SettingSelector, SentinelKey
from azure.appconfiguration.provider.aio import load
from azure.identity import DefaultAzureCredential

app = Flask(__name__)

ENDPOINT = os.environ.get("AZURE_APPCONFIG_ENDPOINT")
credential = DefaultAzureCredential()
selects = SettingSelector(key_filter="testapp_settings_*")
selects_secret = SettingSelector(key_filter="secret_key")

azure_app_config = None  # declare azure_app_config as a global variable

async def load_config():
   global azure_app_config
   async with await load(endpoint=ENDPOINT,
                           selects=[selects, selects_secret],
                           credential=credential,
                           trim_prefixes=["testapp_settings_"],
                           refresh_on=[SentinelKey("sentinel")],
                     ) as config:
      azure_app_config = config

      # App Configuration provider implements the Mapping Type which is compatible with the existing Flask config.
      # Update Flask config mapping with loaded values in the App Configuration provider.
      app.config.update(azure_app_config)

asyncio.run(load_config())


@app.route('/')
async def index():
   global azure_app_config
   # Refresh the configuration from App Configuration service.
   refresh = asyncio.get_event_loop().create_task(azure_app_config.refresh())

   # Update Flask config mapping with loaded values in the App Configuration provider.
   refresh.add_done_callback(lambda t: app.config.update(azure_app_config))
   await refresh

   print('Request for index page received')
   context = {}
   context['message'] = app.config.get('message')
   context['font_size'] = app.config.get('font_size')
   context['color'] = app.config.get('color')
   context['key'] = app.config.get('secret_key') # This is a key vault reference. The corresponding secret in key vault is returned.
   return render_template('index.html', **context)


if __name__ == '__main__':
   app.run()
