# spring-cloud-azure-appconfiguration-config

[Source code][source_code] | [Package (Maven)][package] | [Product documentation][docs] | [Samples][samples]

## 1.2.0 - January 13, 2020

* A number of configuration parameters have renamed/replaced/removed. See [starter](https://github.com/microsoft/spring-cloud-azure/blob/master/spring-cloud-azure-starters/spring-cloud-starter-azure-appconfiguration-config/README.md) for more info.

```properties
spring.cloud.azure.appconfiguration.watch.delay -> spring.cloud.azure.appconfiguration.cache.expiration

# Endpoint Requires URI value
spring.cloud.azure.appconfiguration.stores[0].name -> spring.cloud.azure.appconfiguration.stores[0].endpoint
```

* AzureCloudConfigWatch is now named AzureCloudConfigRefresh. Also AzureCloudConfigRefresh is always created and can be access via dependency injection to allow control over refreshes outside of ServletRequestHandledEvents.
* System Assigned Credential no longer needs client id set.
* TokenCredentialProvider has been split into two file AppConfigCredentialProvider and KeyVaultCredentialProvider.
* AzureCloudConfigRefresh is now non-blocking Async using Reactor.
* Bug fix, multiple refreshes no longer occur when a system that is watching multiple stores where two or more stores are updated before the local values are updated.
* Bug fix, Sentinels are update before refresh completes, resulting in a failed refresh not being reattempted.
* A new [sample](https://github.com/microsoft/spring-cloud-azure/blob/master/spring-cloud-azure-samples/azure-appconfiguration-conversion-sample-initial/README.md) has been added showing how to convert an application to use App Configuration with Key Vault References. There is also a [completed](https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-samples/azure-appconfiguration-conversion-sample-complete) version to show how the code should now look.

## 1.1.0.M6 - December 09, 2019

* Updated managed identity support for both system-assigned managed identity and user-assigned managed identity.

  **Before**

  The managed identity is assigned *Contributor* role to the App Configuration instance.

  **After**

  * The system-assigned managed identity also requires the client-id to be set in addition to the config store name, for example,

    ```properties
    spring.cloud.azure.appconfiguration.managed-identity.client-id=[client id]
    spring.cloud.azure.appconfiguration.stores[0].name=[config store name]
    ```

  * The managed identity should be assigned *App Configuration Data Reader* role to the App Configuration instance.
* To prevent unintentional results, an IllegalArgumentException exception will be thrown if both `spring.cloud.azure.appconfiguration.stores[0].name` and `spring.cloud.azure.appconfiguration.stores[0].connection-string` are provided.

## 1.1.0.M5 - October 28, 2019

* Switched to using Azure SDK.
* Added support for using Key Vault References.
* Auto retry on failure of up to 12 times in a 60 second window.
* Now watch only checks for updates when a ServletRequestHandlerEvent is triggered and a update hasn't been happened in the refresh window.

## 1.1.0.M4 - August 26, 2019

* Added Feature Management Support.

<!-- LINKS -->
[docs]: https://docs.microsoft.com/azure/azure-app-configuration/quickstart-java-spring-app
[package]: https://mvnrepository.com/artifact/com.microsoft.azure/spring-cloud-azure-appconfiguration-config
[samples]: https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-samples
[source_code]: https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-appconfiguration-config
[token_credentials]: https://github.com/Azure/azure-sdk-for-java/blob/master/sdk/identity/azure-identity/README.md
