# Azure App Configuration Kubernetes Provider

[Image][image] | [Sample][sample]

## 1.1.0 - xx xx, 2023

### Enhancements
* Added the `auth.workloadIdentity.managedIdentityClientIdReference` property for workloadIdentity authentication. It enables the retrieval of the client ID of a user-assigned managed identity from a ConfigMap. [#812](https://github.com/Azure/AppConfiguration/issues/812)
    ```
    auth:
      workloadIdentity:
        managedIdentityClientIdReference:  
          configMap: configmap-contains-managed-identity-client-id
          key: key-of-managed-identity-client-id
    ```
* Added the `configMapData.separator` property for generating hierarchical format of json/yaml type ConfigMap data. It defaults to empty and should be configured only if the configuration file loader used in your application can't load key-values without converting them to the hierarchical format. [#834](https://github.com/Azure/AppConfiguration/issues/834)

## 1.0.0 - November 15, 2023

**Release of the stable API version `v1`**.

### Breaking Changes
* Schema update (see [Azure App Configuration Kubernetes Provider reference](https://learn.microsoft.com/en-us/azure/azure-app-configuration/reference-kubernetes-provider?tabs=default) for the complete schema):
    * Renamed `keyValues` to `configuration`.
    * Renamed `keyVaults` to `secret` and moved it to the root, which is at the same level as `configuration`.
    * Renamed `secret.auth.vaults` to `secret.auth.keyVaults`.
* Added the `configuration.refresh.enabled` property for dynamic configuration. It defaults to `false` and must be set to `true` for dynamic configuration refresh.
* Added the `secret.refresh.enabled` property for periodically resolving Key Vault references. It defaults to `false` and must be set to `true` for associated Kubernetes secret update. 

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
