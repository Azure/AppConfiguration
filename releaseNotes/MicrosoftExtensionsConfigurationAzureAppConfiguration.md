## Microsoft.Extensions.Configuration.AzureAppConfiguration
### [Package (NuGet)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.AzureAppConfiguration)


### 5.0.0 - Feb 02, 2022
* Added deprecation warning to the `SetDirty` API. `ProcessPushNotification` API should be used instead for push-model based configuration refresh. Refer to [this tutorial](https://docs.microsoft.com/en-us/azure/azure-app-configuration/enable-dynamic-configuration-dotnet-core-push-refresh?tabs=windowscommandprompt) for more details about the `ProcessPushNotification` API. [#301](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/301)

### 5.0.0-preview - December 16, 2021
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


### 4.5.1 - November 8, 2021
* Fixed a bug where the cache expiration time was not being updated after failed refresh operations. [#283](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/283)

### 4.5.0 - August 12, 2021
* Added deprecation warning to all Offline Cache APIs. The current implementation of offline cache will be removed in the next major release. 

### 4.4.0 - May 7,2021
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

### 4.3.0-preview - April 14,2021
* Added two new APIs for filtering and trimming feature flag by a prefix. [#234](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/234)

   * Selectively load feature flags based on key and label filters:
   ```csharp 
   FeatureFlagOptions Select(string featureFlagFilter, string labelFilter)
   ```

   * Trim a prefix from the id of all feature flags retrieved from Azure App Configuration:
   ```csharp
   FeatureFlagOptions TrimFeatureFlagPrefix(string prefix)
   ```

### 4.2.1 - March 25,2021
* Updated package license information.

### 4.2.0 - March 19, 2021
* Added support for .NET 5 as a target framework.

### 4.1.0 - December 15, 2020
* Added `SetSecretResolver` API to allow users to configure the behavior when a Key Vault reference cannot be resolved. [#209](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/209)
* Added support for registering key-values with same key but different labels for refresh. [#156](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/156)
* Fixed an issue that caused cache expiration to be ignored on first call to RefreshAsync. [#172](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/172)

### 4.0.0 - September 11, 2020
* **Breaking Change :** Updated `ConfigureRefresh` to throw when it is passed a callback that does not register any key-value for refresh using the `Register` method. [#162](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/162)
* Fixed the issue that caused `KeyVaultReferenceException` to be thrown when the `optional` parameter is set to `true` in the method `AddAzureAppConfiguration` and a key vault reference could not be resolved. [#136](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/136)

### 4.0.0-preview - July 23, 2020
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

### 3.0.2 - July 01, 2020
* Fixed an issue that may cause configuration refresh to be ignored when the key registered to refresh all configuration has changed. [#178](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/178)

### 3.0.1 - April 02, 2020
* Improved refresh for feature flags to reduce the number of calls made to App Configuration if no change is detected. [#138](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/138)
* Fixed the issue that caused `TryRefreshAsync` to throw an `Azure.Identity.AuthenticationFailedException` when the `TokenCredential` used to fetch a key vault reference failed to authenticate. [#149](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/149)
* Fixed the following issues when `optional` parameter is set to `true` in the method `AddAzureAppConfiguration`
    * An exception might be thrown if the configuration store could not be accessed. [#136](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/136)
    * `RefreshAsync` might ignore exceptions if configuration fails to load after a change is detected in a key with `refreshAll: true`.
    * `TryRefreshAsync` would throw a `NullReferenceException` if the initial attempt to load the configuration in `IConfiguration.Build` failed.
* Updated `TryRefreshAsync` or `RefreshAsync` to auto-recover from failures during initial configuration load when the `optional` parameter is set to `true` in the method `AddAzureAppConfiguration`. [#145](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/145)

### 3.0.0 - February 19, 2020
* Added the following method to allow users to override `ConfigurationClientOptions`. This enables customization on the underlying App Configuration client that includes modifying retry options and configuring proxy settings. [#106](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/106)

   ````csharp
   public AzureAppConfigurationOptions ConfigureClientOptions(Action<ConfigurationClientOptions> options)
   ````
* Added `IConfigurationRefresher.TryRefreshAsync` method, which will not throw exceptions on transient errors during configuration refresh. [#113](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/113)
* Renamed the `IConfigurationRefresher.Refresh` method to `IConfigurationRefresher.RefreshAsync`.
* Reduced maximum number of retries when querying App Configuration to prevent blocking the application for long periods of time during startup or configuration refresh. [#255](https://github.com/Azure/AppConfiguration/issues/255)

### 3.0.0-preview-011100001-1152 - January 16, 2020
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

### 3.0.0-preview-010550001-251 - November 22, 2019
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
    * Request-Tracing can be disabled by setting the environment variable `AZURE_APP_CONFIGURATION_TRACING_DISABLED` to `True` or `1`
    * Added tracing options for the following:
        * Hosting platform.
        * Differentiating Initialization and Watch requests.
* Improved feature flag to prevent removal of default query with `null` label. This allows the default query to be used in conjunction with feature flags.
