# spring-cloud-azure-appconfiguration-config

[Source code][source_code] | [Package (Maven)][package] | [Product documentation][docs] | [Samples][samples]

# spring-cloud-azure-appconfiguration-config-web

[Source code web ][source_code_web] | [Package (Maven) web][package_web] | [Product documentation][docs]

## 1.1.2/1.2.2 - February 25, 2020

* Credentials for authentication can now be provided in code via ```AppConfigurationCredentialProvider``` and ```KeyVaultCredentialProvider```.
  * This method allows for different authentication methods/credentials to be used when connecting with multiple App Configuration instances or Key Vaults.
* The ```spring.cloud.azure.appconfiguration.stores[0].fail-fast``` setting has been updated to be per store.
  * Previously this setting controlled the error handling for all App Configuration instances, now this setting allows for different error handling to be configured per App Configuration instance.
  * The error handling specified by fail-fast is now limited to when settings are loaded on application startup. This setting does not affect error handling for configuration refresh. However, if there is an error loading from the App Configuration instance on startup, it will not be included in refresh attempts.
* Failed refreshes will now be automatically retried, if not completely successful.
* spring-cloud-azure-appconfiguration-config has been split into two packages: spring-cloud-azure-appconfiguration-config and spring-cloud-azure-appconfiguration-config-web. The web provider take on the spring-web dependency used for automated refresh. In the spring-cloud-azure-appconfiguration-config provider, refresh needs to be manually triggered.
  * To continue using automated refresh, the "spring-cloud-azure-appconfiguration-config" dependency should be updated to "spring-cloud-azure-appconfiguration-config-web".
* ```AppConfigurationRefresh```'s ```refreshConfigurations()``` can be called to check if the settings for each App Configuration instance are up to date and, if the cache has expired, trigger a refresh event.

## 1.1.1/1.2.1 - January 13, 2020

* Added Support for Spring Boot 2.2.x Hoxton with version 1.2.1. Spring Boot 2.1.x Greenwich is still supported with 1.1.1.

### Configuration

* The property below has been removed as the timer-based configuration watch has been deprecated.

  ```properties
  spring.cloud.azure.appconfiguration.watch.enabled
  ```

* The configuration is refreshed based on application activities.
  * The configuration is cached and the default cache expiration time is 30 seconds. The configuration won't be refreshed until the cache expiration time window is reached. This value can be modified, see example below.

   ```properties
  spring.cloud.azure.appconfiguration.cache-expiration = 60
  ```
  
  * In a web application, this library will signal configuration refresh automatically as long as there are incoming requests to the application. This is done by listening for servlet requests.

* Fixed the bug that configuration refresh occurred multiple times unnecessarily when an application loads configuration from more than one App Configuration store.
* Fixed the bug that failed configuration refresh may not be reattempted.

### Authentication

* The property below has been removed as the object ID is not needed for managed identity authentication.

  ```properties
  spring.cloud.azure.appconfiguration.managed-identity.object-id
  ```

* With the generic AAD support by the App Configuration, the property name has been renamed to endpoint.

  **Before**

  ```properties
  spring.cloud.azure.appconfiguration.stores[0].name={my-configstore-name}
  ```

  **After**

  ```properties
  spring.cloud.azure.appconfiguration.stores[0].endpoint= https://{my-configstore-name}.azconfig.io
  ```

* Users can now implement `AppConfigCredentialProvider` and/or `KeyBaultCredentialProvider` to use any of the `TokenCredential` access types that are supported by [Azure Identity][token_credentials] to authenticate with App Configuration or Key Vault respectively. Please see [Starter](https://github.com/mrm9084/spring-cloud-azure/tree/master/spring-cloud-azure-starters/spring-cloud-starter-azure-appconfiguration-config) for more details.
* System-assigned managed identity does not needs client id to be set.

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

[package_web]: https://mvnrepository.com/artifact/com.microsoft.azure/spring-cloud-azure-appconfiguration-config-web
[source_code_web]: https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-appconfiguration-config-web