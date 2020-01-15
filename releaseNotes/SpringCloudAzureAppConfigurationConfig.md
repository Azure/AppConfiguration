# spring-cloud-azure-appconfiguration-config

[Source code][source_code] | [Package (Maven)][package] | [Product documentation][docs] | [Samples][samples]

## 1.1.1/1.2.1 - January 13, 2020

* Added Support for Spring Boot 2.2.x Hoxton with version 1.2.1. Spring Boot 2.1.x Greenwich is still supported with 1.1.1.

### Configuration

* 4 configuration parameters have renamed/replaced/removed. See [starter](https://github.com/microsoft/spring-cloud-azure/blob/master/spring-cloud-azure-starters/spring-cloud-starter-azure-appconfiguration-config/README.md) for updated descriptions.
* The properties below are removed as the timer-based configuration watch has been deprecated. 

```properties
spring.cloud.azure.appconfiguration.watch.enabled
spring.cloud.azure.appconfiguration.managed-identity.object-id
```

* The configuration is refreshed based on application activities.
  * The configuration is cached and the default cache expiration time is 30 seconds. The configuration won't be refreshed until the cache expiration time window is reached. This value can be modified, for example,
  * In a web application, this library will signal configuration refresh automatically as long as there are incoming requests to the application.
  * In a non-web application, the user needs to call AzureCloudConfigRefresh's refreshConfigurations method to signal configuration refresh at places where application activities occur.

```properties
spring.cloud.azure.appconfiguration.cache-expiration = 60
```

* For clarity the name configuration has been changed to endpoint. Instead of my-configstore-name use `https://my-configstore-name.azconfig.io`

```properties
spring.cloud.azure.appconfiguration.stores[0].name -> spring.cloud.azure.appconfiguration.stores[0].endpoint
```

### Authentication

* A new authentication method has been added allowing users to provide there own credentials. Users can create a AppConfigCredentialProvider and KeyVaultCredentialProvider @Bean to provide credentials to App Configuration for connection to Azure App Configuration and Azure Key Vault respectively. Both objects define a method that is given a uri value and returns a [TokenCredential][token_credentials].

```java
public class MyCredentials implements AppConfigCredentialProvider, KeyVaultCredentialProvider {

    @Override
    public TokenCredential credentialForAppConfig(String uri) {
            return buildCredential();
    }

    @Override
    public TokenCredential credentialForKeyVault(String uri) {
            return buildCredential();
    }

    TokenCredential buildCredential() {
            return new DefaultAzureCredentialBuilder().build();
    }

}
```

* Bug fix, system-assigned managed identity no longer needs client id to be set.

### Refresh

* AzureCloudConfigRefresh can be accessed via dependency injection to allow control over refreshes outside of ServletRequestHandledEvents. The refresh will only update after the cache expires based on the configured value.
* AzureCloudConfigRefresh is now non-blocking Async using Reactor.
* Bug fix, configuration refresh occurred multiple times unnecessarily when an application loads configuration from more than one App Configuration store.
* Bug fix, failed configuration refresh may not be reattempted.

### Samples

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
