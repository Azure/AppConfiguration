# Azure App Configuration Kubernetes Provider

[Image][image] | [Sample][sample]

## 2.1.0 - 

### Enhancements
* Added support for load balancing mode, which enables your workloads to distribute requests to App Configuration across all available replicas. This enhancement improves the scalability of applications that typically experience high request volumes to App Configuration, ensuring they remain within quota limits. Load balancing mode is disabled by default and can be activated by setting `loadBalancingEnabled` to `true`. [#55](https://github.com/Azure/AppConfiguration-KubernetesProvider/issues/55)
* Added support for refreshing configMap by watching all key-values. The Kubernetes provider will monitor the changes of selected key-values/feature flags and refresh the target ConfigMap only if the data updates. Watching sentinel key to refresh is still supported, it's optional now. [#40](https://github.com/Azure/AppConfiguration-KubernetesProvider/issues/40)
* Added support for refreshing ConfigMaps and Secrets only if their data is really updated.

### Bug fixes
* Fixed a bug where empty string label filter is not converted to a null label. [#76](https://github.com/Azure/AppConfiguration-KubernetesProvider/issues/76)

## 2.0.0 - September 11, 2024

### Breaking changes
* Starting with version 2.0.0, a user-provided service account is required for authenticating with Azure App Configuration using workload identity. This change enhances security through namespace isolation. Previously, a Kubernetes provider's service account was used for all namespaces. For updated instructions, see the documentation on [using workload identity](https://learn.microsoft.com/en-us/azure/azure-app-configuration/reference-kubernetes-provider?tabs=default#use-workload-identity). If you need time to migrate when upgrading to version 2.0.0, you can temporarily set `workloadIdentity.globalServiceAccountEnabled=true` during provider installation. Please note that support for using the provider's service account will be deprecated in a future release.

### Enhancements
* Added support for [multi-platform images](https://docs.docker.com/build/building/multi-platform/). The Kubernetes provider supports `linux/amd` and `linux/arm64` platforms. [#892](https://github.com/Azure/AppConfiguration/issues/892)

### Bug fixes
* Fixed a bug where a feature flag could be repeatedly added to a ConfigMap if included in multiple feature flag selectors or snapshots.

## 2.0.0-preview - May 15, 2024

### Enhancements
* Added support for [multi-platform images](https://docs.docker.com/build/building/multi-platform/). The Kubernetes provider supports `linux/amd` and `linux/arm64` platforms. [#892](https://github.com/Azure/AppConfiguration/issues/892)

## 1.3.1 - April 25, 2024

### Bug fixes
* Fixed a regression bug in version 1.3.0 where an empty Kubernetes Secret was not created as specified by the `spec.secret.target property` when no Key Vault references were loaded. [#32](https://github.com/Azure/AppConfiguration-KubernetesProvider/issues/32)

## 1.3.0 - April 17, 2024

### Enhancements
* Added support for replica auto-discovery. For App Configuration stores with geo-replication enabled, the Kubernetes provider will now automatically discover replicas and attempt to connect to them when it fails to connect to user-provided endpoint. This capability allows workloads to leverage geo-replication for enhanced resiliency without redeployment. Replica discovery is enabled by default and can be disabled by setting `replicaDiscoveryEnabled` to `false`.
* Added support for storing Key Vault references of TLS certificates to [Kubernetes TLS Secrets](https://kubernetes.io/docs/concepts/configuration/secret/#tls-secrets). [#821](https://github.com/Azure/AppConfiguration/issues/821)
* Added support for downloading snapshot's key-values from Azure App Configuration, which is useful to safely deploy configuration changes.

## 1.2.0 - March 1, 2024

### Enhancements
* Added support for downloading feature flags from Azure App Configuration.
* Added support for setting `nodeSelector`, `affinity` and `tolerations` when using helm to install Azure App Configuration Kubernetes Provider. It's useful for node assignment management. [#858](https://github.com/Azure/AppConfiguration/issues/858)
* Added support for setting `autoscaling` when using helm to install Azure App Configuration Kubernetes Provider. By default, `autoscaling` is disabled. However, if you have multiple `AzureAppConfigurationProvider` resources to produce multiple ConfigMaps/Secrets, you can enable horizontal pod autoscaling by setting `autoscaling.enabled` to `true`.

## 1.1.0 - December 22, 2023

### Enhancements
* Added the `auth.workloadIdentity.managedIdentityClientIdReference` property for workloadIdentity authentication. It enables the retrieval of the client ID of a user-assigned managed identity from a ConfigMap. [#812](https://github.com/Azure/AppConfiguration/issues/812)
* Added support for outputting the ConfigMap data in hierarchical format by specifying the new property `configMapData.separator` if the ConfigMap is consumed as a mounted file. This feature is useful if the configuration file loader used in your application can't load keys without converting them to the hierarchical format. [#834](https://github.com/Azure/AppConfiguration/issues/834)

### Bug fixes
* Fixed a bug that may cause the Kubernetes provider to crash with an `invalid memory address or nil pointer dereference` error when a key-value pulled from App Configuration has a `null` value. [#848](https://github.com/Azure/AppConfiguration/issues/848)

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
