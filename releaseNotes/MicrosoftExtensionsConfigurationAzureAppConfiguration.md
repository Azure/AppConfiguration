## Microsoft.Extensions.Configuration.AzureAppConfiguration
### [Package (NuGet)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.AzureAppConfiguration)

### 2.1.0-preview-010380001-1099 - November 04, 2019
* Updated code to use [`Azure.Data.AppConfiguration`](https://github.com/Azure/azure-sdk-for-net/tree/master/sdk/appconfiguration/Azure.Data.AppConfiguration) to improve compatibility with SDKs from other Azure services.
* Removed reference to the package `System.Interactive.Async` to avoid conflicts when using `IAsyncEnumerable` in ASP.NET Core 3.0. [#139](https://github.com/Azure/AppConfiguration/issues/139)
* Updated namespace for `AddAzureAppConfiguration` extension method from `Microsoft.Extensions.Configuration.AzureAppConfiguration` to `Microsoft.Extensions.Configuration`. [#140](https://github.com/Azure/AppConfiguration/issues/140)
* Improved validation for offline cache file path to avoid silent failures while writing to the cache.

### 2.0.0-preview-010050001-38 - October 03, 2019
* Added support for [Azure Key Vault references](https://docs.microsoft.com/en-us/azure/azure-app-configuration/use-key-vault-references-dotnet-core).
* Fixed the following issues when using `ConfigureRefresh` for dynamic configuration.
    * Configuration is no longer updated on each refresh invocation if `refreshAll` is `true` for a key that does not exist.
    * Key-values registered for refresh that were not included in `Use` might not always get loaded in the initial configuration load.
    * Key-values registered for refresh that were not included in `Use` might be loaded more than once if the client invoked `Use` more than once.
* Improved error message when using `ConnectWithManagedIdentity` when the required access to an App Configuration store is not set up properly.

### 2.0.0-preview-009470001-1371 - August 06, 2019
* Fixed a bug which caused some application frameworks which use a custom synchronization context, like ASP.NET, to hang when building the configuration provider.

### v2.0.0-preview-009200001-1437 - July 10, 2019
* Replaced watch feature with on-demand refresh mechanism to address the following fundamental design flaws.
    * Watch could not be invoked on-demand.
    * It continued to run in the background even in applications that could be considered inactive.
    * It promoted constant polling of configuration rather than a more intelligent approach of updating configuration when applications are active or need to ensure freshness.
* Added refresh mechanism to allow on-demand configuration updates with an internal cache.
* Removed `Watch` and `WatchAndReloadAll` methods.
* Added `ConfigureRefresh` method to configure dynamic refresh for configuration settings.
* Added `GetRefresher` method to retrieve an `IConfigurationRefresher` to trigger manual refresh in code.

### v1.0.0-preview-008920001-990 - June, 11 2019
* Added `TrimKeyPrefix` method to remove prefixes from the list of returned key-values from the config store.
* Improved durability on Watch, so that transient network errors do not cause the process to detach.
* Added additional retries on network error when using `ConnectWithManagedIdentity`.
* Enabled Correlation-Context for request-tracing.
    * ***Request-Tracing can be disabled by setting the environment variable `AZURE_APP_CONFIGURATION_TRACING_DISABLED` to `True` or `1`***
    * Added tracing options for the following:
        * Hosting platform.
        * Differentiating Initialization and Watch requests.
* Improved feature flag to prevent removal of default query with `null` label. This allows the default query to be used in conjunction with feature flags.
