# azure-appconfiguration-provider

[Source code][source_code] | [Package (Pypi)][package] | [Samples][samples]

## 1.2.0 - May 23, 2024

### Features

* Added support for the App Configuration Feature Management V2.0.0 schema. This allows for the loading of feature flags from an App Configuration provider. For more information see [here](https://github.com/Azure/AppConfiguration/blob/main/docs/FeatureManagement/FeatureManagement.v2.0.0.schema.json)
  * Added `feature_flags_enabled` method to the `load` method, to enable loading of feature flags, by default when enabled all feature flags with no label are loaded.
  * Added `feature_flag_selectors` which works similar to `SettingSelector` but for feature flags.
  * Added `feature_flag_refresh_enabled` to the `load` method, to enable the refreshing of feature flags. Feature flags are refreshed with the same refresh method as configuration settings, though they are refreshed separatetly, so a change in a feature flag will not cause a refresh of configuration settings.

### Enhancement

* Updated the reterival of configuration settings from provider to be more efficient.

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
