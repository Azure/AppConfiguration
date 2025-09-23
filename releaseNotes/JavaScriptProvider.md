# Azure App Configuration JavaScript Provider

[Source code][source_code] | [Package (npm)][package] | [Samples][samples]

## 2.2.0 - August 7, 2025

### Enhancements

* Added tag filter selector which enables the configuration provider to load configuration settings based on tags. [#188](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/188)

* Added a new property `secretRefreshInterval` under `AzureAppConfigurationOptions.keyVaultOptions`, which enables the configuration provider to periodically reload secrets and certificates from Key Vault. [#175](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/175)

* Added support for accepting JSON values with comments. [#205](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/205)

### Bug fix

* Fixed a bug that the configuration provider startup will keep retrying on unfailoverable `RestError`. [#206](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/206)

## 2.1.0 - May 22, 2025

### Enhancements

* Added snapshot selector which enables the configuration provider to load configuration snapshots. [#140](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/140)

* Added time-based retry mechanism for the `load` function to handle transient failures. By default, the retry timeout is set to 100 seconds, which can be customized via the `AzureAppConfigurationOptions.startupOptions.timeoutInMs` property. [#166](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/166)

* Added support for configuring `SecretClientOptions` used to connect to an Azure Key Vault that has no registered `SecretClient` via the new `AzureAppConfigurationOptions.keyVaultOptions.clientOptions` property. [#194](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/194)

* Added support for resolving Azure Key Vault secrets in parallel by setting the new `parallelSecretResolutionEnabled` property under `AzureAppConfigurationOptions.keyVaultOptions` to true.  [#192](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/192)

## 2.0.2 - April 22, 2025

* `FeatureFlagId` is no longer added to telemetry metadata of a feature flag with `telemetry` enabled. [#183](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/183)

## 2.0.1 - February 27, 2025

### Bug Fixes

* Fixed a bug where the load operation would be interrupted if searching for replicas failed. [#170](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/170)

## 2.0.0 - February 13, 2025

### Enhancements

* Added support for automatic replica discovery for geo-replication enabled App Configuration stores, enhancing resiliency and scalability for non-browser-based applications. The feature is not available for browser-based applications due to the restriction of browser security sandbox. [#98](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/98)

* Added support for load balancing mode which enables your application to distribute requests to App Configuration across all available replicas. This enhancement improves the scalability of applications that typically experience high request volumes to App Configuration, ensuring they remain within quota limits. Load balancing mode is disabled by default and can be activated by setting the new `AzureAppConfigurationOptions.loadBalancingEnabled` property to true. [#135](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/135)

* Added support for monitoring all selected key-values. Configuration will be refreshed if any of key-values are updated. Watching the sentinel key for refresh helps ensure data integrity of configuration changes, but it is now optional. This behavior is activated when you enable the refresh but do not specify any watched keys in `AzureAppConfigurationOptions.refreshOptions`. [#133](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/133)

* Added support for loading all feature flags with no label when no selector is specified under `AzureAppConfigurationOptions.featureFlagOptions`. [#158](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/158)

* Added support for injecting additional telemetry metadata `FeatureFlagId`, `FeatureFlagReference` and `ETag` to feature flags if telemetry is enabled.

## 1.1.3 - January 8, 2025

### Bug Fixes

* Fixed a bug that could trigger concurrent data refresh. [#136](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/136)

## 2.0.0-preview.2 - January 8, 2025

### Enhancements

* Added support for automatic replica discovery for geo-replication enabled App Configuration stores, enhancing resiliency and scalability for non-browser-based applications. The feature is not available for browser-based applications due to the restriction of browser security sandbox. [#98](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/98)

* Added support for load balancing mode which enables your application to distribute requests to App Configuration across all available replicas. This enhancement improves the scalability of applications that typically experience high request volumes to App Configuration, ensuring they remain within quota limits. Load balancing mode is disabled by default and can be activated by setting the new `AzureAppConfigurationOptions.loadBalancingEnabled` property to true. [#135](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/135)

* Added support for monitoring all selected key-values. Configuration will be refreshed if any of key-values are updated. Watching the sentinel key for refresh helps ensure data integrity of configuration changes, but it is now optional. This behavior is activated when you enable the refresh but do not specify any watched keys in `AzureAppConfigurationOptions.refreshOptions`. [#133](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/133)

## 2.0.0-preview.1 - November 8, 2024

### Enhancements

* Added support for injecting additional telemetry metadata `FeatureFlagId`, `FeatureFlagReference`,`ETag` and `AllocationId` to feature flags if telemetry is enabled. [#101](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/101), [#111](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/111)

## 1.1.2 - November 5, 2024

### Bug Fixes

* Fixed a bug that caused `ReferenceError` of optional chaining when `process` is undefined. [#104](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/104)

## 1.1.0 - August 13, 2024

### Enhancements

- Added support for loading feature flags from Azure App Configuration. [#65](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/65)

## 1.0.1 - July 24, 2024

### Bug Fixes

- Fixed a bug that caused the error ‘ReferenceError: WorkerNavigator is not defined’ when running the application in a browser. [#81](https://github.com/Azure/AppConfiguration-JavaScriptProvider/issues/81)

## 1.0.0 - June 6, 2024

This is the first stable release of the following features.
- Loading and composing configuration with key-value selectors.
- Authentication with connection string or Microsoft Entra Id.
- Loading key-values as either a `Map` or a configuration object.
- Trimming prefixes from keys.
- Loading key-values with JSON content type as configuration objects.
- Key Vault reference resolution.
- Support for dynamic configuration refresh.

Get started with the [quickstart](https://learn.microsoft.com/en-us/azure/azure-app-configuration/quickstart-javascript-provider).

## 1.0.0-preview.4 - April 11, 2024

### Breaking Changes

- Excluded feature flags from loaded settings. For example, if you attempt to load all settings by specifying a selector with `keyFilter: "*"`, only configuration settings are included now, whereas previously feature flags were also loaded. [#55](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/55)

## 1.0.0-preview.3 - March 21, 2024

### Enhancements

- Added support for dynamic configuration refresh. See an example [here](https://github.com/Azure/AppConfiguration-JavaScriptProvider/blob/main/examples/refresh.mjs). [#21](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/21)

- Added support for consuming configuration as an object. A new API, `constructConfigurationObject`, has been added to construct a configuration object based on the key-values loaded from Azure App Configuration. It minimizes necessary code changes for applications that were using JSON configuration file to adopt Azure App Configuration. [#49](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/49)

## 1.0.0-preview.2 - December 15, 2023

### Breaking Changes

- The label filter in a selector is restricted to a single label. An error will be thrown if a label filter contains `*` or `,`. This change is to avoid the ambiguity when multiple values are loaded for the same key. Key-values with different labels can still be loaded using separate selectors for proper configuration composition. [#22](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/22)

- Fixed a bug where the last selector may not take precedence if multiple selectors with the same key and label filters are provided. After applying the fix, the resulting configuration of an application may change. To ensure the intended configuration composition, remove any duplicated selectors.[#23](https://github.com/Azure/AppConfiguration-JavaScriptProvider/issues/23)

### Bug Fixes

- Updated the reference of `@azure/core-rest-pipeline` to `1.12.2`, which added retry for DNS resolution of App Configuration endpoints. [Azure/azure-sdk-for-js#27037](https://github.com/Azure/azure-sdk-for-js/issues/27037)

## 1.0.0-preview.1 - October 24, 2023

### Bug Fixes

- Updated the reference of `@azure/identity` to `3.3.2`, addressing [CVE-2023-36415](https://msrc.microsoft.com/update-guide/en-US/vulnerability/CVE-2023-36415)

## 1.0.0-preview - October 11, 2023

Added support for
- authentication with connection string or Microsoft Entra
- loading and composing configuration with key-value selectors
- key prefix trimming
- Key Vault reference resolution
- Key-value with JSON content-type

[package]: https://www.npmjs.com/package/@azure/app-configuration-provider
[samples]: https://github.com/Azure/AppConfiguration-JavaScriptProvider/tree/main/examples
[source_code]: https://github.com/Azure/AppConfiguration-JavaScriptProvider

