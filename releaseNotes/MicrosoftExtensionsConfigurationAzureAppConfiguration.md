## Microsoft Extensions Azure App Configuration

### v2.0.0-preview-009200001-1437 - July 10, 2019
* Added `ConfigureRefresh` method to configure dynamic refresh for configuration settings
    * Added `GetRefresher` method to retrieve an `IConfigurationRefresher` to trigger manual refresh in code
    * Added an internal cache to limit calls to the configuration store when refresh is triggered too often
* Removed the Watch feature to refresh configuration at regular polling intervals
    * Reduces unnecessary outgoing network calls from client machine to the configuration store
    * Avoids unnecessary load on configuration store when the key-values remain unchanged
    * Avoids silent failure of configuration updates due to quota limit exceeded error

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
