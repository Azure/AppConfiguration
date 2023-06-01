# Azure App Configuration Kubernetes Provider

[Image][image] | [Sample][sample]

## 1.1.0-preview - Jun XX, 2023

### New Features

* Dynamic configuration support, support refreshing the data of ConfigMap or Secret by watching the sentinel key periodically.
* HMAC authentication support.

### Bug Fixes

* Fixed an issue happens while multiple selectors are used.

## 1.0.0-preview - April 07, 2023

### New Features

* Added support for Key Vault references, which allows resolved secrets to be saved to a Kubernetes Secret.

## 1.0.0-alpha - March 8, 2023

Initial Alpha Release of the Azure App Configuration Kubernetes Provider

### New Features

* Authenticate Azure App Configuration with AAD Service Principal and Managed Identity
* Download key-values from App Configuration to Kubernetes ConfigMap
* Key filtering and label filtering
* Trim prefixes of key names

[image]: https://mcr.microsoft.com/product/azure-app-configuration/kubernetes-provider/about
[sample]: https://learn.microsoft.com/azure/azure-app-configuration/quickstart-azure-kubernetes-service
