## Microsoft.Extensions.Configuration.AzureAppConfiguration
### [Package (NuGet)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.AzureAppConfiguration)

### 3.0.1 - April 02, 2020
* Improved refresh for feature flags to reduce the number of calls made to App Configuration if no change is detected.
* Fixed the issue that caused `TryRefreshAsync` to throw an `Azure.Identity.AuthenticationFailedException` when the `TokenCredential` used to fetch a key vault reference failed to authenticate.
* Fixed the following issues when `optional` parameter is set to `true` in the method `AddAzureAppConfiguration`
    * An exception might be thrown if the configuration store could not be accessed.
    * `RefreshAsync` might ignore exceptions if configuration fails to load after a change is detected in a key with `refreshAll: true`.
    * `TryRefreshAsync` would throw a `NullReferenceException` if the initial attempt to load the configuration in `IConfiguration.Build` failed.

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
