# Azure App Configuration Go Provider

[Source code][source_code] | [Package][package] | [Samples][samples]

## v1.3.0 - September 29, 2025

### Enhancement

* Added support for downloading snapshot's key-values from Azure App Configuration, which is useful to safely deploy configuration changes. [#49](https://github.com/Azure/AppConfiguration-GoProvider/pull/49)
* Added time-based retry mechanism for the initial configuration load to handle transient failures. By default, the retry timeout is set to 100 seconds, which can be customized via the `Options.StartupOptions.Timeout` property. [#51](https://github.com/Azure/AppConfiguration-GoProvider/pull/51)
* Upgraded Go version requirement to `1.24.0`. [#53](https://github.com/Azure/AppConfiguration-GoProvider/pull/53)

## v1.2.0 - August 20, 2025

### Enhancement

* Added support for replica auto-discovery. For App Configuration stores with geo-replication enabled, the Go provider will now automatically discover replicas and attempt to connect to them when it fails to connect to user-provided endpoint. This capability allows workloads to leverage geo-replication for enhanced resiliency without redeployment. Replica discovery is enabled by default and can be disabled by setting `azureappconfiguration.Options.ReplicaDiscoveryEnabled` to false. [#39](https://github.com/Azure/AppConfiguration-GoProvider/pull/39)
* Added support for load balancing mode, which enables your workloads to distribute requests to App Configuration across all available replicas. This enhancement improves the scalability of applications that typically experience high request volumes to App Configuration, ensuring they remain within quota limits. Load balancing mode is disabled by default and can be activated by setting `azureappconfiguration.Options.LoadBalancingEnabled` to true. [#42](https://github.com/Azure/AppConfiguration-GoProvider/pull/42)
* Added support for parsing JSON values with comments. [#43](https://github.com/Azure/AppConfiguration-GoProvider/pull/43)
* Upgraded dependent packages. [#44](https://github.com/Azure/AppConfiguration-GoProvider/pull/44)

## v1.1.0 - August 01, 2025

This is the first stable release that includes support for loading feature flags from Azure App Configuration.

## v1.1.0-beta.1 - July 01, 2025

### Enhancement

* Added support for loading feature flags from Azure App Configuration.

## v1.0.0 - June 19, 2025

This is the first stable release of the following features:
- Authentication with connection string or Microsoft Entra ID
- Loading and composing configuration with key-value selectors
- Key prefix trimming
- Key Vault reference resolution
- Strongly-typed struct data binding 
- Returning key-values as raw json bytes
- Dynamic configuration refresh
- Periodically resolving Key Vault references to fetch the latest version of secret

## 1.0.0-beta.2 - May 29, 2025

### Enhancement

* Added support for sentinel key-based dynamic configuration refresh from Azure App Configuration.
* Added support for monitoring all selected key-values for dynamic configuration refresh from Azure App Configuration.
* Added support for periodically resolving Key Vault references to fetch the latest version of secret.

## 1.0.0-beta.1 - April 10, 2025

Added support for
- Authentication with connection string or Microsoft Entra ID
- Loading and composing configuration with key-value selectors
- Key prefix trimming
- Key Vault reference resolution
- Strongly-typed struct data binding 
- Returning key-values as raw json bytes

[source_code]: https://github.com/Azure/AppConfiguration-GoProvider
[package]: https://pkg.go.dev/github.com/Azure/AppConfiguration-GoProvider/azureappconfiguration
[samples]: https://github.com/Azure/AppConfiguration-GoProvider/tree/main/example