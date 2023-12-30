import os
from flask import Flask, render_template
from azure.appconfiguration.provider import load, SettingSelector, WatchKey
from azure.identity import DefaultAzureCredential

app = Flask(__name__)

ENDPOINT = os.environ.get("AZURE_APPCONFIG_ENDPOINT")
credential = DefaultAzureCredential()
selects = SettingSelector(key_filter="testapp_settings_*")
selects_secret = SettingSelector(key_filter="secret_key")


def callback():
    app.config.update(azure_app_config)


global azure_app_config
azure_app_config = load(
    endpoint=ENDPOINT,
    selects=[selects, selects_secret],
    credential=credential,
    keyvault_credential=credential,
    trim_prefixes=["testapp_settings_"],
    refresh_on=[WatchKey("sentinel")],
    on_refresh_success=callback,
)
app.config.update(azure_app_config)


@app.route("/")
def index():
    global azure_app_config
    # Refresh the configuration from App Configuration service.
    azure_app_config.refresh()

    print("Request for index page received")
    context = {}
    context["message"] = app.config.get("message")
    context["font_size"] = app.config.get("font_size")
    context["color"] = app.config.get("color")
    context["key"] = app.config.get(
        "secret_key"
    )  # This is a key vault reference. The corresponding secret in key vault is returned.
    return render_template("index.html", **context)


if __name__ == "__main__":
    app.run()
