# Sample Python Flask Application using Azure App Configuration

This is the sample Flask  application that uses the Azure App Configuration Service [Deploy a Python (Django or Flask) web app to Azure App Service](https://docs.microsoft.com/en-us/azure/app-service/quickstart-python).  For instructions on how to create the Azure resources and deploy the application to Azure, refer to the Quickstart article.

A [sample Django application](../python-django-webapp-sample) is also available.

If you need an Azure account, you can [create one for free](https://azure.microsoft.com/en-us/free/).

## Prerequisites

You must have an [Azure subscription][azure_sub], and a [Configuration Store][configuration_store] to use this package.

To create a Configuration Store, you can either use [Azure Portal](https://ms.portal.azure.com/#create/Microsoft.Azconfig) or if you are using [Azure CLI][azure_cli] you can simply run the following snippet in your console:

```Powershell
az appconfig create --name <config-store-name> --resource-group <resource-group-name> --location eastus
```

### Create Keys

```Powershell
az appconfig kv set --name <config-store-name> --key testapp_settings_message --value "Hello from Azure App Configuration"
az appconfig kv set --name <config-store-name> --key testapp_settings_font_size --value "30px"
az appconfig kv set --name <config-store-name> --key testapp_settings_color --value "azure"
```

### Create Key Vault Reference

Requires Key Vault with Secret already created.

```Powershell
az appconfig kv set-keyvault --name <config-store-name> --key secret_key --secret-identifier <key-vault-reference>
```

## Setup

Install the Azure App Configuration Provider client library for Python and other dependencies with pip:

```commandline
pip install -r requirements.txt
```

Set your App Configuration store endpoint as an environment variable.

### Command Prompt

```commandline
setx AZURE_APPCONFIG_ENDPOINT "your-store-endpoint"
```

### PowerShell

```Powershell
$Env:AZURE_APPCONFIG_ENDPOINT="your-store-endpoint"
```

### Linux/ MacOS

```Bash
export AZURE_APPCONFIG_ENDPOINT="your-store-enpoint"
```

Start the flask application using the flask command:
```commandline
flask run
```

<!-- LINKS -->
[azure_sub]: https://azure.microsoft.com/free/
[azure_cli]: https://docs.microsoft.com/cli/azure
[configuration_store]: https://azure.microsoft.com/services/app-configuration/
