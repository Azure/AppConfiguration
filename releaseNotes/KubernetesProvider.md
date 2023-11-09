# Azure App Configuration Kubernetes Provider

[Image][image] | [Sample][sample]

## 1.0.0 - xx xx, 2023

### Breaking Changes
* Schema (more information in [Azure App Configuration Kubernetes Provider reference](https://learn.microsoft.com/en-us/azure/azure-app-configuration/reference-kubernetes-provider?tabs=default)):
    * Rename `keyValues` to `configuration`.
    * Rename `keyVaults` to `secret` and put it in the root which is at the same level as `configuration`.
    * Rename `secret.auth.vaults` to `secret.auth.keyVaults`.
* Upgrade API version from `v1beta1` to `v1`.
* Add `*.refresh.enabled` property for dynamic configuration. It's required if data is expected to be refreshed.

## 1.0.0-preview4 - September 14, 2023

### New Features
* Added support for periodically resolving Key Vault references to fetch latest version secret and update Kubernetes secret accordingly.
* Added support for consuming the generated ConfigMap as a mounted file besides as environment variables. [#775](https://github.com/Azure/AppConfiguration/issues/775)
* Added support for workload identity authentication. [#795](https://github.com/Azure/AppConfiguration/issues/795)

## 1.0.0-preview3 - July 31, 2023

### New Features

* Added support for dynamic configuration so the corresponding ConfigMap and Secret will be automatically updated when data is changed in Azure App Configuration.
* Added support for access key (aka. connection string) based authentication.

### Bug Fixes

* Fixed an issue that happens while multiple selectors are used.
* Fixed an issue that resolved secrets are not saved in the selected order.

## 1.0.0-preview2 - July 07, 2023

### Bug Fixes

* Fixed a race condition issue that happens while resolving Key Vault references.

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
