import os
from azure.appconfiguration.provider import load, WatchKey
from azure.identity import AzureCliCredential
from featuremanagement import FeatureManager
from featuremanagement.appinsights import send_telemetry
from azure.monitor.opentelemetry import configure_azure_monitor
from opentelemetry import trace
from opentelemetry.trace import get_tracer_provider
from flask_bcrypt import Bcrypt

from flask_sqlalchemy import SQLAlchemy
from flask_login import LoginManager

configure_azure_monitor(connection_string=os.getenv("APPLICATIONINSIGHTS_CONNECTION_STRING"))

from flask import Flask

app = Flask(__name__)

bcrypt = Bcrypt(app)

tracer = trace.get_tracer(__name__, tracer_provider=get_tracer_provider())


ENDPOINT = os.getenv("AZURE_APPCONFIG_ENDPOINT")
credential = AzureCliCredential()


def callback():
    app.config.update(azure_app_config)


global azure_app_config
azure_app_config = load(
    endpoint=ENDPOINT,
    credential=credential,
    keyvault_credential=credential,
    trim_prefixes=["testapp_settings_"],
    refresh_on=[WatchKey("sentinel")],
    on_refresh_success=callback,
    feature_flag_enabled=True,
    feature_flag_refresh_enabled=True,
)
app.config.update(azure_app_config)
feature_manager = FeatureManager(azure_app_config, telemetry=send_telemetry)

db = SQLAlchemy()
db.init_app(app)

login_manager = LoginManager()
login_manager.init_app(app)


from model import Users

@login_manager.user_loader
def loader_user(user_id):
    return Users.query.get(user_id)

with app.app_context():
    db.create_all()

if __name__ == "__main__":
    app.run()

import routes
