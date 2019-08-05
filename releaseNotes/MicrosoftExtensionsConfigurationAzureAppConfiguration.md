## Microsoft Extensions Azure App Configuration

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
