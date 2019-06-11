## Microsoft Extensions Azure App Configuration

The Azure App Configuration Extension in the Azure DevOps pipeline has been published to [Azure DevOps marketplace](https://marketplace.visualstudio.com/items?itemName=AzureAppConfiguration.azure-app-configuration-task&ssr=false#overview).


### v1.0.0-preview-008920001-990

* Added `TrimKeyPrefix` method to remove prefixes from the list of returned key-values from the config store.
* Improved durability on Watch, so that transient network errors do not cause the process to detach.
* Retries on network error when listing keys in Managed Identity Connector.
* Enabled Correlation-Context for telemetry/request-tracing.
    * ***Telemetry can be disabled by setting the environment variable `AZURE_APP_CONFIGURATION_TRACING_DISABLED` to `True` or `1`***
    * Added telemetry for the following:
        * Hosting platform.
        * Differentiating Initialization and Watch/Observe requests.
* Adding feature flags has been improved, and prevented removal of default query with `null` label.
    