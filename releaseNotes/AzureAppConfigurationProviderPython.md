# azure-appconfiguration-provider

[Source code][source_code] | [Package (Pypi)][package] | [Samples][samples]

## 2.1.0 - April 29, 2025

### Features

* Added support for including AllocationId to feature flag telemetry metadata when telemetry is enabled.

## 2.0.0 - January 6, 2025

* Added support for load balancing mode, which enables your workloads to distribute requests to App Configuration across all available replicas. This enhancement improves the scalability of applications that typically experience high request volumes to App Configuration, ensuring they remain within quota limits. Load balancing mode is disabled by default and can be activated by setting `load_balancing_enabled` to `true`.
* Added support for including FeatureFlagReference, FeatureFlagId, Etag to feature flag telemetry metadata when telemetry is enabled.

## 2.0.0b3 - November 14, 2024

### Bug Fixes

* Updated the method for generating allocation IDs for feature flag telemetry to ensure consistency across different languages of App Configuration providers.

## 2.0.0b2 - October 14, 2024

### Enhancements

* Added support for including AllocationId to feature flag telemetry metadata when telemetry is enabled.

### Bug Fixes

* Fixed an issue where snake case was used for telemetry metadata instead of pascal case.

## 2.0.0b1 - Sepeter 12, 2024

### Features

* Added support for Azure App Configuration Feature Flag Telemetry.

## 1.3.0 - September 09, 2024

### Features

* Added support for auto discovery of Azure App Configuration store replicas.
  * Enabled by default, can be disabled by setting `replica_discovery_enabled` to `False`.
* Added support for auto failover between replicas.

From more information see [Geo-Replication](https://learn.microsoft.com/azure/azure-app-configuration/howto-geo-replication).

## 1.2.0 - May 24, 2024

### Features

* Added support for loading feature flags from Azure App Configuration.

### Enhancement

* Improved the performance of data loading from Azure App Configuration, especially when you have a large set of key-values.

## 1.1.0 - January 29, 2024

### Features

* Added support for dynamically refreshing configuration values with the new `refresh` method on `AzureAppConfigurationProvider`. This enables the runtime change of configuration values without restarting the application. A `WatchKey` is a configuration setting, that is checked for changes. When a change in it occurs all configuration settings are refreshed. For more information see [here](https://learn.microsoft.com/azure/azure-app-configuration/enable-dynamic-configuration-python).
  * Added `refresh_on` parameter to the load method, selects which key(s) changing should cause a refresh.
  * Added `refresh_interval` parameter to the load method, the minimum time between refreshes.
  * Added `on_refresh_success` and `on_refresh_error` parameter to the load method, callbacks for when a refresh is successful/failes.
* Added support for `keyvault_credential`, `keyvault_client_configs`, and `secret_resolver` as `kwargs` to use Key Vault references.

## 1.0.0 - March 9, 2023

### Features

* Connecting to an App Configuration Store using a connection string or Azure Active Directory
* Selecting multiple sets of configurations using SettingSelector
* Trim prefixes off key names
* Resolving Key Vault References, requires AAD
* Secret Resolver, provides a way for a custom implementation of resolving Key Vault references.
* JSON content-type (e.g. MIME type application/json) support for key-values in App Configuration. This allows primitive types, arrays, and JSON objects to be loaded.
* Async Support
* Full support of Mapping API, allowing for `dict` like access to the loaded configuration

### Breaking Changes

* Renamed `load_provider` method to `load`
* Added `AzureAppConfigurationKeyVaultOptions` to take in a `client_configs` Mapping of endpoints to client `kwargs` instead of taking in the whole client
* Removed `AzureAppConfigurationKeyVaultOptions` secret_clients, client_configs should be used instead
* Made `key_filter` and `label_filter` kwargs for Setting Selector
* Renamed `trimmed_key_prefixes` to `trim_prefixes`

## 1.0.0b2 - February 15, 2023

### New Features

* Added Async Support
* Full support of Mapping API, allowing for `dict` like access to the loaded configuration
* Made `load` method properties unordered

### Breaking Changes

* The `load` class method of `AzureAppConfigurationProvider` has been replaced with the module level method `load_provider`
* All Feature Flags are added to their own key and have there prefix removed

### Bug Fixes

* Fixed an issue where multiple secret clients couldn't be provided

## 1.0.0b1 - October 13, 2022

Initial Beta Release of the Azure App Configuration Provider for Python

### New Features

* Connecting to an App Configuration Store using a connection string or Azure Active Directory
* Selecting multiple sets of configurations using SettingSelector
* Trim prefixes off key names
* Resolving Key Vault References, requires AAD
* Secret Resolver, resolve Key Vault References locally without connecting to Key Vault
* JSON content-type (e.g. MIME type application/json) support for key-values in App Configuration. This allows primitive types, arrays, and JSON objects to be loaded.

[package]: https://pypi.org/project/azure-appconfiguration-provider/
[samples]: https://github.com/Azure/azure-sdk-for-python/tree/main/sdk/appconfiguration/azure-appconfiguration-provider/samples
[source_code]: https://github.com/Azure/azure-sdk-for-python/tree/main/sdk/appconfiguration/azure-appconfiguration-provider
