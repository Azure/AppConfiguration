# azure-appconfiguration-provider

[Source code][source_code] | [Package (Pypi)][package] | [Samples][samples]


## 1.0.0 - March 9, 2023

### Breaking Changes

* Renamed `load_provider` method to `load`
* Added `AzureAppConfigurationKeyVaultOptions` to take in a `client_configs` Mapping of endpoints to client `kwargs` instead of taking in the whole client.
* Removed `AzureAppConfigurationKeyVaultOptions` secret_clients, client_configs should be used instead.
* Made key_filter and label_filter kwargs for Setting Selector
* Renamed trimmed_key_prefixes to trim_prefixes

## 1.0.0b2 - February 15, 2023

### New Features

* Added Async Support
* Full support of Mapping API
* Made load method properties unordered

### Breaking Changes

* The `load` class method of `AzureAppConfigurationProvider` has been replaced with the module level method `load_provider`.
* All Feature Flags are added to there own key and have there prefix removed.

### Bug Fixes

* Fixed an issue where multiple key clients couldn't be provided.

## 1.0.0b1 - October 13, 2022

Initial Beta Release of the Azure App Configuration Provider for Python

### New Features

* Connecting to an App Configuration Store using a connection string or Azure Active Directory.
* Selecting multiple sets of configurations using SettingSelector.
* Trim prefixes off key names.
* Resolving Key Vault References, requires AAD.
* Secret Resolver, resolve Key Vault References locally without connecting to Key Vault.
* JSON content-type (e.g. MIME type application/json) support for key-values in App Configuration. This allows primitive types, arrays, and JSON objects to be loaded.

[package]: https://pypi.org/project/azure-appconfiguration-provider/
[samples]: https://github.com/Azure/azure-sdk-for-python/tree/main/sdk/appconfiguration/azure-appconfiguration-provider/samples
[source_code]: https://github.com/Azure/azure-sdk-for-python/tree/main/sdk/appconfiguration/azure-appconfiguration-provider
