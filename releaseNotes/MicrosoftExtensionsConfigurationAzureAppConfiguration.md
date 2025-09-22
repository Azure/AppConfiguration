# Microsoft.Extensions.Configuration.AzureAppConfiguration
[Source code][source_code] | [Package (NuGet)][package]

## 8.4.0 - September 19th, 2025
### Enhancements
* Added support for accepting JSON values with comments. [#685](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/685)

* Added support for snapshot references. If a configuration setting's value is a snapshot reference, loading it automatically loads all key-values from the referenced snapshot. This enables dynamically switching between snapshots at runtime. [#689](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/689)

## 8.3.0 - July 24th, 2025
### Enhancements
* Added health check integration for `Microsoft.Extensions.Diagnostic.HealthChecks`. You can call `AddAzureAppConfiguration` on `IHealthCheckBuilder` to register a health check for the Azure App Configuration provider. [#644](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/644)

   ```cs
   builder.Services
      .AddHealthChecks()
      .AddAzureAppConfiguration();
   ```

* Introduced a new environment variable `AZURE_APP_CONFIGURATION_FM_SCHEMA_COMPATIBILITY_DISABLED` to force processing and outputting feature flags using the [Microsoft schema](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFlag.v2.0.0.schema.json). [#676](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/676)

## 8.2.0 - May 14th, 2025
### Enhancements
* Updated the existing `Select` APIs with the new parameter `tagFilters` to support filtering key-values and feature flags by tags.

   ```cs
   public AzureAppConfigurationOptions Select(string keyFilter, string labelFilter = LabelFilter.Null, IEnumerable<string> tagFilters = null)
   ```

   ```cs
   public FeatureFlagOptions Select(string featureFlagFilter, string labelFilter = LabelFilter.Null, IEnumerable<string> tagFilters = null)
   ```
* Added an `ActivitySource` called `Microsoft.Extensions.Configuration.AzureAppConfiguration` to support instrumentation. A `Load` activity will start when configuration is initially built and the `Refresh` activity will start when a refresh is triggered. [#645](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/645)
* This is the first stable release of the `AzureAppConfigurationOptions.SetClientFactory` API introduced in 8.2.0-preview. [#380](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/380)

   ```cs
   public AzureAppConfigurationOptions SetClientFactory(IAzureClientFactory<ConfigurationClient> factory)
   ```

### Other Changes
* Removed the `FeatureFlagId` property from feature flag telemetry. [#655](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/655)
* Shortened default network timeout for requests to App Configuration to improve failover speed and retry responsiveness. [#657](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/657)
* This is the first stable release of `AllocationId` from feature flag telemetry metadata, which was introduced in 8.1.0-preview. [#600](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/600)

## 8.1.2 - April 22nd, 2025
### Bug Fixes
* Calling `IServiceCollection.AddAzureAppConfiguration` now correctly adds an instance of `IConfigurationRefresherProvider` to the service collection only the first time it is called instead of adding an instance on each invocation. [#611](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/611)

## 8.2.0-preview - March 21st, 2025
### Enhancements
* Added the ability to set a client factory used for creating ConfigurationClient instances. [#380](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/380)

   ```cs
   public AzureAppConfigurationOptions SetClientFactory(IAzureClientFactory<ConfigurationClient> factory)
   ```

## 8.1.1 - February 26th, 2025
### Bug Fixes
* Fixed a bug introduced in the 8.1.0 release where calls to `FeatureFlagOptions.Select` were ignored if they were followed by a call to either `AzureAppConfigurationOptions.Select` or `AzureAppConfigurationOptions.SelectSnapshot`. [#628](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/628)

## 8.1.0 - February 24th, 2025
### Enhancements
* Added the ability to monitor all selected key-values for refresh with the following new API. [#536](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/536)

   ```cs
   public AzureAppConfigurationRefreshOptions RegisterAll()
   ```

   When this API is called, changes to any selected key-values will cause a reload of the configuration.
* Added the ability to check if the type of an `IConfigurationSource` is `AzureAppConfigurationSource` with the following new extension. [#456](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/456)

   ```cs
   public static bool IsAzureAppConfigurationSource(this IConfigurationSource source)
   ```

### Bug Fixes
* Fixed a bug where changing a feature flag would always override an already loaded feature flag with the same key but different label on refresh. As part of this change, feature flags are now always refreshed as a collection, meaning a change in any selected flag will result in a reload of all selected feature flags on refresh. [#317](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/317)
* Fixed a bug where an unnecessary refresh could occur when a request to App Configuration was served from the cache. [#588](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/588)

### Other Changes
* Removed support for the `configuration_reference` property in feature flags. [#614](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/614)
* `AllocationId` from feature flag telemetry metadata, which was introduced in 8.1.0-preview version, has been removed in this stable version. [#600](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/600)

## 8.1.0-preview - October 24th, 2024
### Enhancements
* Added support for injecting additional telemetry metadata to feature flags if telemetry is enabled
  * `AllocationId` represents the version of an allocation on a feature flag. The `AllocationId` changes when there are essential modifications to the allocation, such as adjustments to the percentile or variant configuration, but not for non-essential changes like feature flag descriptions.

## 8.0.0 - October 3rd, 2024
### Breaking Changes
* Increased the default maximum retry count when resolving Key Vault references and increased the minimum refresh interval to 1 minute. This helps prevent requests to Key Vault from being throttled and will reduce the chance of failing to resolve a secret due to momentary failures. [#589](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/589)
* Removed .NET 7 as a target framework as .NET 7 is out of support. [#567](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/567)
* The APIs `AzureAppConfigurationRefreshOptions.SetCacheExpiration` and `FeatureFlagOptions.CacheExpirationInterval` have been deprecated and will be removed in a future release. They are replaced with `AzureAppConfigurationRefreshOptions.SetRefreshInterval` and `FeatureFlagOptions.SetRefreshInterval`, respectively. This change does not affect functionality but aims to eliminate confusion regarding cache expiration implied by the previous API names. [#350](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/350)

### Enhancements
* **Variant Feature Flags and Telemetry**: This is the first stable release with support for the `variants`, `allocation`, and `telemetry` properties for feature flags added by version 4.0.0 of the [Microsoft.FeatureManagement.*](https://github.com/microsoft/FeatureManagement-Dotnet) libraries. While variant feature flags can still be toggled on or off, they also allow for different configurations, ranging from simple primitives to complex JSON objects. Variant feature flags are particularly useful for feature rollouts, configuration rollouts, and feature experimentation (also known as A/B testing).
* This is the first stable release of the load balancing mode introduced in 8.0.0-preview.3. Load balancing enables your application to distribute requests to App Configuration across all available replicas. This enhancement improves the scalability of applications that typically experience high request volumes to App Configuration, ensuring they remain within quota limits. Load balancing mode is off by default and can be activated by setting the new `AzureAppConfigurationOptions.LoadBalancingEnabled` property to `true`. [#535](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/535)
* Added the ability to configure options used to connect to Key Vault resources that have no registered `SecretClient` with the following new API. [#274](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/274)

   ```cs
   public AzureAppConfigurationKeyVaultOptions ConfigureClientOptions(Action<SecretClientOptions> configure)
   ```

   This API can be called from within the `AzureAppConfigurationOptions.ConfigureKeyVault` method.

## 8.0.0-preview.3 - July 18th, 2024
### Breaking Changes
* Removed .NET 7 as a target framework as .NET 7 is out of support. [#567](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/567)
* The APIs `AzureAppConfigurationRefreshOptions.SetCacheExpiration` and `FeatureFlagOptions.CacheExpirationInterval` have been deprecated and will be removed in a future release. They are replaced with `AzureAppConfigurationRefreshOptions.SetRefreshInterval` and `FeatureFlagOptions.SetRefreshInterval`, respectively. This change does not affect functionality but aims to eliminate confusion regarding cache expiration implied by the previous API names. [#350](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/350)

### Enhancements
* A load balancing mode has been introduced, enabling your application to distribute requests to App Configuration across all available replicas. This enhancement improves the scalability of applications that typically experience high request volumes to App Configuration, ensuring they remain within quota limits. Load balancing mode is off by default and can be activated by setting the new `AzureAppConfigurationOptions.LoadBalancingEnabled` property to `true`. [#535](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/535)
* Variant feature flags are now output using the [Microsoft feature flag schema v2.0.0](https://github.com/Azure/AppConfiguration/blob/main/docs/FeatureManagement/FeatureFlag.v2.0.0.schema.json), while the output of configuration for classic feature flags remains unchanged. [#543](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/543)

## 7.3.0 - July 16th, 2024
### Enhancements
* Upgraded the package reference for `Azure.Security.KeyVault.Secrets` to `4.6.0`. [#572](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/572)

### Bug Fixes
* Loading invalid feature flags should no longer throw an error when `TryRefreshAsync` is invoked or when the `optional` argument is true in the `AddAzureAppConfiguration` method. [#551](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/551)

## 7.2.0 - May 2, 2024
### Enhancements
* Added support for .NET 8 as a target framework. [#530](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/530)
* Improved JSON key-value deserialization. [#537](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/537)
* Updated `Azure.Data.AppConfiguration` reference to `1.4.1`. [#514](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/514).

## 8.0.0-preview.2 - March 18, 2024
### Bug Fixes
* Fixed a bug where a `FormatException` may be thrown for a variant feature flag when the configuration value was not set to a string or `null`. With this fix, the configuration value of a variant can be a string, number, boolean, array, or JSON object. [#531](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/531)

## 8.0.0-preview - February 28, 2024
### Enhancements
* Added new fields to feature flag telemetry metadata, including `FeatureFlagId`, `FeatureFlagReference`, and `ETag`. [#517](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/517)

### Bug Fixes
* Fixed a bug where a feature flag would not be parsed correctly by the `Microsoft.FeatureManagement` library if the flag was enabled but had no filters. [#525](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/525)

## 7.1.0 - February 26, 2024
### Enhancements
* This is the first stable release of the **Replica Auto-Discovery** enhancement that was introduced in 7.1.0-preview.

### Bug Fixes
* Fixed a bug where the provider would throw a `KeyVaultReferenceException` instead of retrying for transient errors while connecting to Key Vault during startup, even if the startup timeout had not elapsed. [#516](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/516)

## 7.1.0-preview - January 19, 2024
### Enhancements
* **Replica Auto-Discovery**: For App Configuration stores with geo-replication enabled, the provider will now automatically discover any additional replicas and attempt to connect to them when it fails to connect to user-provided endpoints in code. This capability allows applications to leverage geo-replication for enhanced resiliency without code change and redeployment. Replica discovery is enabled by default and can be disabled by setting the new `AzureAppConfigurationOptions.ReplicaDiscoveryEnabled` property equal to `false`. [#442](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/442).

* Added support for the `variants`, `allocation`, and `telemetry` properties for feature flags introduced in version 4.0.0-preview of the [Microsoft.FeatureManagement](https://github.com/microsoft/FeatureManagement-Dotnet/releases/tag/4.0.0-preview) library. [#476](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/476)

### Bug Fixes
* Fixed a bug where passing a null value for the `key` parameter to `AzureAppConfigurationRefreshOptions.Register` would throw a `NullReferenceException`. [#503](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/503)
* Fixed a bug with `AzureAppConfigurationOptions.Select`, `FeatureFlagOptions.Select`, and `AzureAppConfigurationOptions.SelectSnapshot` where additional calls with the same filters after the first call were ignored. The provider will now correctly use only the last call with duplicate filters to preserve the expected precedence. [#490](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/490)

## 7.0.0 - November 21, 2023
### Breaking Changes
* `AddAzureAppConfiguration` now throws `ArgumentException` for invalid inputs even if the `optional` parameter is equal to `true`. [#318](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/318)

### Enhancements
* This is the first stable release of the following API introduced in the 7.0.0-preview release. [#387](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/387)

   ```cs
   public AzureAppConfigurationOptions SelectSnapshot(string name)
   ```
* Added support for the `requirement_type` property for feature flags introduced in version 2.6.0-preview of the Microsoft.FeatureManagement library. [#406](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/406)
* The initial configuration load now uses time-based retries instead of count-based retries in case of failures. The time-based retry often works more effectively in real-world scenarios, making applications more resilient to transient errors during startup. By default, the retry timeout is set to 100 seconds in `AzureAppConfigurationOptions.StartupOptions.Timeout`, but you can customize it using the following new API. [#458](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/458)

   ```cs
   public AzureAppConfigurationOptions ConfigureStartupOptions(Action<StartupOptions> configure)
   ```
* Added netstandard2.1 as a target framework. [#482](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/482)

## 6.1.1 - October 25, 2023
* Added improvements for request tracing.

## 7.0.0-preview.2 - August 24, 2023
* Includes all changes made in `6.1.0`.

## 6.1.0 - August 17, 2023
* Fixed a bug where `TryRefreshAsync` could throw an `AggregateException` after a network timeout while trying to get key-values from Azure App Configuration. [#440](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/440)
* Designated a new environment variable `AZURE_APP_CONFIGURATION_PROVIDER_DISABLED` that when equal to "true" will disable Azure App Configuration in your application. [#429](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/429)
* Added a new warning log that indicates when the provider fails over to a different endpoint. [#431](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/431)

## 7.0.0-preview - July 20, 2023
### Breaking Changes:
* Feature flags are now parsed and transformed into feature management configuration even if `AzureAppConfigurationOptions.UseFeatureFlags` is not called. `AzureAppConfigurationOptions.UseFeatureFlags` is still used to configure feature flag options. [#422](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/422)

### Enhancements
* Added the following new API for adding a snapshot's key-values to the configuration. [#422](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/422)

   ```cs
   public AzureAppConfigurationOptions SelectSnapshot(string name)
   ```

## 6.0.1 - May 3, 2023
* Fixed a bug where ASP.NET (.NET Framework) applications would fail to refresh the configuration. [#410](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/410)

## 6.0.0 - March 28, 2023
### Breaking Changes:
* Removed `IConfigurationRefresher.SetDirty` API in favor of `IConfigurationRefresher.ProcessPushNotification` API for push-model based configuration refresh. Unlike the `SetDirty` method, the `ProcessPushNotification` method guarantees that all configuration changes up to the triggering event are loaded in the following configuration refresh. For more details on the `ProcessPushNotification` API, refer to [this tutorial](https://docs.microsoft.com/en-us/azure/azure-app-configuration/enable-dynamic-configuration-dotnet-core-push-refresh?tabs=windowscommandprompt). [#357](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/357)
* Removed .NET 5 as a target framework as .NET 5 is out of support. [#391](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/391)
* Feature Management V2 schema support, which was introduced in 5.2.0-preview release, has been removed from this stable release. [#315](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/315)
* Removed `IConfigurationRefresher.LoggerFactory` API, but refresh logs are still available through standard ASP.NET Core logging if `services.AddAzureAppConfiguration()` is invoked in your `ConfigureServices` method. [#367](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/367)
* `IConfigurationRefresher.ProcessPushNotification` now validates that the push notification was triggered for one of the registered App Configuration stores. If no matching App Configuration store is registered, the push notification is ignored and the refresh operation will not be triggered. [#319](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/319)

### Enhancements
* This is the first stable release of the following API introduced in 5.3.0-preview release. [#178](https://github.com/Azure/AppConfiguration/issues/178)

   ```cs
   public AzureAppConfigurationOptions Connect(IEnumerable<Uri> endpoints, TokenCredential credential)
   ```

* Added the following new API for additional App Configuration geo-replication support. [#385](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/385)

   ```cs
   public AzureAppConfigurationOptions Connect(IEnumerable<string> connectionStrings)
   ```

   The new API allows you to provide an ordered list of connection strings of your App Configuration store and its replicas.

* Added the following new API for performing custom transformations on App Configuration settings. [#157](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/157)

   ```cs
   public AzureAppConfigurationOptions Map(Func<ConfigurationSetting, ValueTask<ConfigurationSetting>> mapper)
   ```

* For .NET 7 or later, added support for refreshing configuration when `AzureAppConfigurationProvider` is nested under `ChainedConfigurationProvider`. [#168](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/168)
* Added support for Azure SDK logging in addition to standard ASP.NET Core logging. Refresh logs are available under the "Microsoft-Extensions-Configuration-AzureAppConfiguration-Refresh" category. To enable Azure SDK logs, refer to [these instructions](https://learn.microsoft.com/en-us/dotnet/azure/sdk/logging). [#367](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/367)

## 5.2.0 - November 29, 2022
* Added support for .NET 7 as a target framework. [#366](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/366)
* Fixed a bug where passing an empty string for the label filter to `AzureAppConfigurationOptions.Select` would select key-values with all labels. [#311](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/311)
* Fixed a bug where calls to `AzureAppConfigurationOptions.Select` would be incorrectly ignored if the key-values were already selected by a previous call. [#349](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/349)
* Fixed a bug where `AddAzureAppConfiguration` could throw an exception on invalid feature flags even with the `optional` parameter set to `true`. [#348](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/348)
* Upgraded the package references `Azure.Security.KeyVault.Secrets` to `4.3.0` and `System.Text.Json` to `4.7.2`. [#351](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/351)

## 5.3.0-preview - July 27, 2022
* Added the following new API for the App Configuration geo-replication support. [#178](https://github.com/Azure/AppConfiguration/issues/178)

   ```cs
   public AzureAppConfigurationOptions Connect(IEnumerable<Uri> endpoints, TokenCredential credential)
   ```

   The new API allows you to provide an ordered list of replica endpoints of your App Configuration store. The App Configuration provider will fail over to other replicas when a replica is not accessible (either the service is unavailable or there are networking issues). A sample application can be found [here](https://github.com/Azure/AppConfiguration-DotnetProvider/tree/release/preview/v5/examples/ConsoleAppWithFailOver).

## 5.2.0-preview - July 18, 2022
* Added support for Feature Management V2 schema introduced in Microsoft.FeatureManagement 3.0.0-preview library. [#315](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/315)

## 5.1.0 - July 18, 2022
* Added support for .NET 6 as a target framework. [#290](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/290)
* Removed support for Feature Management V2 schema introduced in Microsoft.FeatureManagement 3.0.0-preview library. [#315](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/315)

## 5.1.0-preview - May 20, 2022
* Added support for Feature Management V2 schema introduced in Microsoft.FeatureManagement 3.0.0-preview library. [#315](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/315)

## 5.0.0 - Feb 02, 2022
* Added deprecation warning to the `IConfigurationRefresher.SetDirty` API. `IConfigurationRefresher.ProcessPushNotification` API should be used instead for push-model based configuration refresh. Refer to [this tutorial](https://docs.microsoft.com/en-us/azure/azure-app-configuration/enable-dynamic-configuration-dotnet-core-push-refresh?tabs=windowscommandprompt) for more details about the `ProcessPushNotification` API. [#301](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/301)

## 5.0.0-preview - December 16, 2021
### Breaking Changes:
* Removed all offline caching capabilities. [#135](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/135)
* Added support for parsing and using sync-token from push notifications received from Event Grid. Using sync-token ensures that users get the latest key-values from App Configuration on any subsequent request. The following new APIs were added:
   ```csharp
   EventGridEventExtensions.TryCreatePushNotification(this EventGridEvent eventGridEvent, out PushNotification pushNotification)
   IConfigurationRefresher.ProcessPushNotification(PushNotification pushNotification, TimeSpan? maxDelay = null)
   ```
   To use sync-token in a [push refresh enabled application](https://docs.microsoft.com/en-us/azure/azure-app-configuration/enable-dynamic-configuration-dotnet-core-push-refresh), the existing `SetDirty()` call can be replaced by the following code. Depending on the [event handler](https://docs.microsoft.com/en-us/azure/event-grid/event-handlers) you're using, you may need to convert the received event to an `EventGridEvent` object. For example, if you are using Service Bus as the event handler, the code will look like this:
   ```csharp
   serviceBusClient.RegisterMessageHandler(
                  handler: (message, cancellationToken) =>
                  {
                     EventGridEvent eventGridEvent = EventGridEvent.Parse(BinaryData.FromBytes(message.Body));
   
                     if (eventGridEvent.TryCreatePushNotification(out PushNotification pushNotification))
                     {
                        _refresher.ProcessPushNotification(pushNotification, maxDelay); 
                     }
   
                     return Task.CompletedTask;
                  },
                  exceptionReceivedHandler: (exceptionargs) =>
                  {
                        Console.WriteLine($"{exceptionargs.Exception}");
                        return Task.CompletedTask;
                  });
   ```
   The next call to `RefreshAsync()` or `TryRefreshAsync()` will get the latest key-values from your App Config store. [#278](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/278)

* Added support for `CancellationToken` during refresh operations. The following APIs were updated in `IConfigurationRefresher` interface: [#281](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/281)
   ```csharp
   Task RefreshAsync(CancellationToken cancellationToken = default);
   Task<bool> TryRefreshAsync(CancellationToken cancellationToken = default);
   ```
* Added support for logging errors during refresh operations. [#273](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/273)
* Ensured that Key Vault secret refresh interval cannot be less than 1 second. [#284](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/284)
* Upgraded Microsoft.Extensions packages from version 2.1.1 to 3.1.18. [#272](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/272)


## 4.5.1 - November 8, 2021
* Fixed a bug where the cache expiration time was not being updated after failed refresh operations. [#283](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/283)

## 4.5.0 - August 12, 2021
* Added deprecation warning to all Offline Cache APIs. The current implementation of offline cache will be removed in the next major release. 

## 4.4.0 - May 7,2021
* Added two new APIs which allow users to opt-in for periodically reloading secrets and certificates from Key Vault. [#249](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/249)

   * **Set refresh interval for individual keys of Key Vault references in App Config:**

    ```cs
    AzureAppConfigurationKeyVaultOptions SetSecretRefreshInterval(string secretReferenceKey, TimeSpan refreshInterval)
    ```

    This method allows users to set a refresh interval per key of Key Vault references. The API can be called multiple times to register multiple keys of Key Vault references for refresh.

   * **Set refresh interval for all Key Vault references in App Config:**

    ```cs
    AzureAppConfigurationKeyVaultOptions SetSecretRefreshInterval(TimeSpan refreshInterval)
    ```

    This method allows users to set a refresh interval for all Key Vault references which do not have individual refresh intervals.

* This is the first stable release of the `FeatureFlagOptions.Select` API introduced in 4.3.0-preview release.

* `FeatureFlagOptions.TrimFeatureFlagPrefix` API, which was introduced in 4.3.0-preview release, has been removed from this stable release.

## 4.3.0-preview - April 14,2021
* Added two new APIs for filtering and trimming feature flag by a prefix. [#234](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/234)

   * Selectively load feature flags based on key and label filters:
   ```csharp 
   FeatureFlagOptions Select(string featureFlagFilter, string labelFilter)
   ```

   * Trim a prefix from the id of all feature flags retrieved from Azure App Configuration:
   ```csharp
   FeatureFlagOptions TrimFeatureFlagPrefix(string prefix)
   ```

## 4.2.1 - March 25,2021
* Updated package license information.

## 4.2.0 - March 19, 2021
* Added support for .NET 5 as a target framework.

## 4.1.0 - December 15, 2020
* Added `SetSecretResolver` API to allow users to configure the behavior when a Key Vault reference cannot be resolved. [#209](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/209)
* Added support for registering key-values with same key but different labels for refresh. [#156](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/156)
* Fixed an issue that caused cache expiration to be ignored on first call to RefreshAsync. [#172](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/172)

## 4.0.0 - September 11, 2020
* **Breaking Change :** Updated `ConfigureRefresh` to throw when it is passed a callback that does not register any key-value for refresh using the `Register` method. [#162](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/162)
* Fixed the issue that caused `KeyVaultReferenceException` to be thrown when the `optional` parameter is set to `true` in the method `AddAzureAppConfiguration` and a key vault reference could not be resolved. [#136](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/136)

## 4.0.0-preview - July 23, 2020
* **Breaking Change :** Added enhanced support for applications that leverage Event Grid integration in App Configuration for configuration refresh. The following new API is introduced in `IConfigurationRefresher` interface, which can be called when an application responds to push notifications from Event Grid. This signals the application to reassess whether configuration should be updated on the next call to `RefreshAsync()` or `TryRefreshAsync()`. [#133](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/133)
   ````csharp
   void SetDirty(TimeSpan? maxDelay = null)
   ````
* **Breaking Change :** Added JSON content-type (e.g. MIME type `application/json`) support for key-values in App Configuration. This allows primitive types, arrays, and JSON objects to be loaded properly to `IConfiguration`. Existing applications that use key-values with a valid JSON content-type may need to be updated. [#191](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/191)
* **Breaking Change :** Added the following property to `IConfigurationRefresher` to allow users to disambiguate instances of the interface when using multiple Azure App Configuration providers.
   ````csharp
   Uri AppConfigurationEndpoint { get; }
   ````
* Added support for applications to obtain `IConfigurationRefresher` instances through dependency injection (DI). This allows better control of when to call `RefreshAsync()/TryRefreshAsync()` or whether to `await` the call. The following two APIs can be used to take advantage of this feature. [#167](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/167)

    * Call `IServiceCollection.AddAzureAppConfiguration()` first to add required services to the DI container.
      ````csharp
      public static IServiceCollection AddAzureAppConfiguration(this IServiceCollection services)
      ````
    * Retrieve `IConfigurationRefresher` instances via `IConfigurationRefresherProvider` interface obtained through DI.
      ````csharp
      public interface IConfigurationRefresherProvider
      {
         IEnumerable<IConfigurationRefresher> Refreshers { get; }
      }
      ````

## 3.0.2 - July 01, 2020
* Fixed an issue that may cause configuration refresh to be ignored when the key registered to refresh all configuration has changed. [#178](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/178)

## 3.0.1 - April 02, 2020
* Improved refresh for feature flags to reduce the number of calls made to App Configuration if no change is detected. [#138](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/138)
* Fixed the issue that caused `TryRefreshAsync` to throw an `Azure.Identity.AuthenticationFailedException` when the `TokenCredential` used to fetch a key vault reference failed to authenticate. [#149](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/149)
* Fixed the following issues when `optional` parameter is set to `true` in the method `AddAzureAppConfiguration`
    * An exception might be thrown if the configuration store could not be accessed. [#136](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/136)
    * `RefreshAsync` might ignore exceptions if configuration fails to load after a change is detected in a key with `refreshAll: true`.
    * `TryRefreshAsync` would throw a `NullReferenceException` if the initial attempt to load the configuration in `IConfiguration.Build` failed.
* Updated `TryRefreshAsync` or `RefreshAsync` to auto-recover from failures during initial configuration load when the `optional` parameter is set to `true` in the method `AddAzureAppConfiguration`. [#145](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/145)

## 3.0.0 - February 19, 2020
* Added the following method to allow users to override `ConfigurationClientOptions`. This enables customization on the underlying App Configuration client that includes modifying retry options and configuring proxy settings. [#106](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/106)

   ````csharp
   public AzureAppConfigurationOptions ConfigureClientOptions(Action<ConfigurationClientOptions> options)
   ````
* Added `IConfigurationRefresher.TryRefreshAsync` method, which will not throw exceptions on transient errors during configuration refresh. [#113](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/113)
* Renamed the `IConfigurationRefresher.Refresh` method to `IConfigurationRefresher.RefreshAsync`.
* Reduced maximum number of retries when querying App Configuration to prevent blocking the application for long periods of time during startup or configuration refresh. [#255](https://github.com/Azure/AppConfiguration/issues/255)

## 3.0.0-preview-011100001-1152 - January 16, 2020
* Updated `Azure.Data.AppConfiguration` reference to `1.0.0`. See the [release notes](https://github.com/Azure/azure-sdk-for-net/blob/94fdb6ba5719daa4d8d63b226c61064b2f52c085/sdk/appconfiguration/Azure.Data.AppConfiguration/CHANGELOG.md) for more information on the changes.
* Replaced `Newtonsoft.Json` with `System.Text.Json` to improve performance and increase compatibility with ASP.NET Core 3.0 and SDKs from other Azure services. See [this blog post](https://devblogs.microsoft.com/dotnet/try-the-new-system-text-json-apis/) for more information.
* Updated `Microsoft.Azure.KeyVault` with its successor [Azure.Security.KeyVault.Secrets](https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/keyvault/Azure.Security.KeyVault.Secrets/README.md) to resolve key vault references.
* Replaced the `UseAzureKeyVault` method used to configure Key Vault references with `ConfigureKeyVault` method.
    * A `TokenCredential` can be provided to be used for authentication with referenced Key Vaults.
    * One or more instances of `SecretClient` can be registered to override the provided `TokenCredential`, if any, for individual Key Vaults.
    * The configuration provider no longer uses any *default identity* to try to resolve Key Vault references. A `TokenCredential` or `SecretClient` must be configured using `ConfigureKeyVault` to successfully resolve a reference.

   #### Example 1 : Before
   ````csharp
   IConfigurationBuilder configBuilder = new ConfigurationBuilder();
   IConfiguration configuration = configBuilder.AddAzureAppConfiguration(options => {
      options.Connect(endpoint, new DefaultAzureCredential());
   }).Build();
   ````
   #### Example 1 : After
   ````csharp
   IConfigurationBuilder configBuilder = new ConfigurationBuilder();
   IConfiguration configuration = configBuilder.AddAzureAppConfiguration(options => {
      options.Connect(endpoint, new DefaultAzureCredential());
      options.ConfigureKeyVault(kv => {
         kv.SetCredential(new DefaultAzureCredential())
      });
   }).Build();
   ````

   #### Example 2 : Before
   ````csharp
   IKeyVaultClient keyVaultClient = CreateKeyVaultClient();
   IConfigurationBuilder configBuilder = new ConfigurationBuilder();
   IConfiguration configuration = configBuilder.AddAzureAppConfiguration(options => {
      options.Connect(endpoint, new ManagedIdentityCredential());
      options.UseAzureKeyVault(keyVaultClient);
   }).Build();
   ````
   #### Example 2 : After
   ````csharp
   SecretClient secretClient = CreateSecretClient();
   IConfigurationBuilder configBuilder = new ConfigurationBuilder();
   IConfiguration configuration = configBuilder.AddAzureAppConfiguration(options => {
      options.Connect(endpoint, new ManagedIdentityCredential());
      options.ConfigureKeyVault(kv => {
         kv.Register(secretClient);
      });
   }).Build();
   ````

* Fixed a bug which caused some application frameworks which use a custom synchronization context, like ASP.NET, to hang when building the configuration provider.

## 3.0.0-preview-010550001-251 - November 22, 2019
* Added the following method to connect to App Configuration store using Azure Active Directory. This allows authentication using any of [these](https://docs.microsoft.com/en-us/dotnet/api/azure.core.tokencredential) derived types of `TokenCredential` and require the identity to be assigned to the `App Configuration Data Reader` role on the App Configuration store.
   ````csharp
   public AzureAppConfigurationOptions Connect(Uri endpoint, TokenCredential credential)
   ````
* Removed the `ConnectWithManagedIdentity` method to connect to the App Configuration store using system-assigned managed identity. This now requires the managed identity to be assigned to the `App Configuration Data Reader` role, adding a reference to the package [`Azure.Identity`](https://github.com/Azure/azure-sdk-for-net/tree/master/sdk/identity/Azure.Identity) and updating the code to use the `Connect` method.

   #### Before
   ````csharp
   IConfigurationBuilder configBuilder = new ConfigurationBuilder();
   IConfiguration configuration = configBuilder.AddAzureAppConfiguration(options => {
      options.ConnectWithManagedIdentity(endpoint);
   }).Build();
   ````
   #### After
   ````csharp
   IConfigurationBuilder configBuilder = new ConfigurationBuilder();
   IConfiguration configuration = configBuilder.AddAzureAppConfiguration(options => {
      options.Connect(endpoint, new ManagedIdentityCredential());
   }).Build();
   ````
* Renamed the `Use` method in `AzureAppConfigurationOptions` to `Select` to improve the understanding that the input to the method takes a key filter and is not limited to the name of a specific key. Also removed the `preferredDateTime` parameter.

   #### Before
   ````csharp
   public AzureAppConfigurationOptions Use(string keyFilter, string labelFilter = LabelFilter.Null, DateTimeOffset? preferredDateTime = null)
   ````
   #### After
   ````csharp
   public AzureAppConfigurationOptions Select(string keyFilter, string labelFilter = LabelFilter.Null)
   ````
* Removed the following extension method to add Azure App Configuration to your application.
   ````csharp
   public static IConfigurationBuilder AddAzureAppConfiguration(
               this IConfigurationBuilder configurationBuilder,
               AzureAppConfigurationOptions options,
               bool optional = false)
   ````
* Removed the `ConnectionString` property from `AzureAppConfigurationOptions`.

## 2.1.0-preview-010380001-1099 - November 04, 2019
* Updated code to use [`Azure.Data.AppConfiguration`](https://github.com/Azure/azure-sdk-for-net/tree/master/sdk/appconfiguration/Azure.Data.AppConfiguration) to improve compatibility with SDKs from other Azure services.
* Removed reference to the package `System.Interactive.Async` to avoid conflicts when using `IAsyncEnumerable` in ASP.NET Core 3.0. [#139](https://github.com/Azure/AppConfiguration/issues/139)
* Updated namespace for `AddAzureAppConfiguration` extension method from `Microsoft.Extensions.Configuration.AzureAppConfiguration` to `Microsoft.Extensions.Configuration`. [#140](https://github.com/Azure/AppConfiguration/issues/140)
* Improved validation for offline cache file path to avoid silent failures while writing to the cache.

## 2.0.0-preview-010050001-38 - October 03, 2019
* Added support for [Azure Key Vault references](https://docs.microsoft.com/en-us/azure/azure-app-configuration/use-key-vault-references-dotnet-core).
* Fixed the following issues when using `ConfigureRefresh` for dynamic configuration.
    * Configuration is no longer updated on each refresh invocation if `refreshAll` is `true` for a key that does not exist.
    * Key-values registered for refresh that were not included in `Use` might not always get loaded in the initial configuration load.
    * Key-values registered for refresh that were not included in `Use` might be loaded more than once if the client invoked `Use` more than once.
* Improved error message when using `ConnectWithManagedIdentity` when the required access to an App Configuration store is not set up properly.

## 2.0.0-preview-009470001-1371 - August 06, 2019
* Fixed a bug which caused some application frameworks which use a custom synchronization context, like ASP.NET, to hang when building the configuration provider.

## v2.0.0-preview-009200001-1437 - July 10, 2019
* Replaced watch feature with on-demand refresh mechanism to address the following fundamental design flaws.
    * Watch could not be invoked on-demand.
    * It continued to run in the background even in applications that could be considered inactive.
    * It promoted constant polling of configuration rather than a more intelligent approach of updating configuration when applications are active or need to ensure freshness.
* Added refresh mechanism to allow on-demand configuration updates with an internal cache.
* Removed `Watch` and `WatchAndReloadAll` methods.
* Added `ConfigureRefresh` method to configure dynamic refresh for configuration settings.
* Added `GetRefresher` method to retrieve an `IConfigurationRefresher` to trigger manual refresh in code.

## v1.0.0-preview-008920001-990 - June, 11 2019
* Added `TrimKeyPrefix` method to remove prefixes from the list of returned key-values from the config store.
* Improved durability on Watch, so that transient network errors do not cause the process to detach.
* Added additional retries on network error when using `ConnectWithManagedIdentity`.
* Enabled Correlation-Context for request-tracing.
    * Request-Tracing can be disabled by setting the environment variable `AZURE_APP_CONFIGURATION_TRACING_DISABLED` to `True` or `1`
    * Added tracing options for the following:
        * Hosting platform.
        * Differentiating Initialization and Watch requests.
* Improved feature flag to prevent removal of default query with `null` label. This allows the default query to be used in conjunction with feature flags.

<!-- LINKS -->
[package]: https://www.nuget.org/packages/Microsoft.Extensions.Configuration.AzureAppConfiguration
[source_code]: https://github.com/Azure/AppConfiguration-DotnetProvider/tree/main/src/Microsoft.Extensions.Configuration.AzureAppConfiguration