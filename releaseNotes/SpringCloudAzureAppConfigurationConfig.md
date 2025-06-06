# spring-cloud-azure-starter-appconfiguration-config

[Source code][source_code_starter] | [Package (Maven)][package_starter] | [Product documentation][docs] | [Samples][samples]

[CHANGELOG](https://github.com/Azure/azure-sdk-for-java/blob/main/sdk/spring/spring-cloud-azure-starter-appconfiguration-config/CHANGELOG.md)

# spring-cloud-azure-appconfiguration-config

[Source code][source_code] | [Package (Maven)][package]

[CHANGELOG](https://github.com/Azure/azure-sdk-for-java/blob/main/sdk/spring/spring-cloud-azure-appconfiguration-config/CHANGELOG.md)

# spring-cloud-azure-appconfiguration-config-web

[Source code][source_code_web] | [Package (Maven)][package_web]

## 6.0.0-beta.1 - June 04, 2025

### Enhancements

* Added support for new Spring configuration system. Now uses `application.properties` and `application.yml` files instead of `bootstrap.properties` and `bootstrap.yml`.
  * Now requires `spring.config.import=azureAppConfiguration` to be set to load Azure App Configuration.
* Added `DefaultAzureCredential` as the default authentication method instead of `ManagedIdentityCredential`.
* Removed `fail-fast` property. Replicas should be used to ensure high availability.

## 5.16.0 - September 09, 2024

### Bug Fixes

* Fixed missing "additional-spring-configuration-metadata.json" in spring-cloud-azure-starter-appconfiguration-config. [#41502](https://github.com/Azure/azure-sdk-for-java/pull/41502).

## 5.13.0 - June 06, 2024

### Bug Fixes

* Fixed a bug where App Configuration exposed the value of key in error message when parsing invalid JSON. [#40132](https://github.com/Azure/azure-sdk-for-java/pull/40132).
* Fixed a bug where final modifier on App Configuration refresh endpoints caused errors when creating Spring AOP Aspects. [#40452](https://github.com/Azure/azure-sdk-for-java/pull/40452).

## 5.11.0 - March 29, 2024

### Enhancements

* Add telemetry schema [#38933](https://github.com/Azure/azure-sdk-for-java/pull/38933).
* Added Auto fail over support. Will automatically find Azure App Configuration replica stores for provided store. The found replica stores will be used as fail over stores after all provided replicas have failed [#38534](https://github.com/Azure/azure-sdk-for-java/pull/38534).
* Added property to disable auto fail over support `spring.cloud.azure.appconfiguration.stores[0].replica-discovery-enabled` [#38534](https://github.com/Azure/azure-sdk-for-java/pull/38534).

## 5.9.0-beta.1 - January 11, 2024

### Enhancements

* Added support for loading Feature Variants from Azure App Configuration. [#38293](https://github.com/Azure/azure-sdk-for-java/pull/38293)

## 5.8.0 - December 13, 2023

### Enhancements

* Added support for Azure App Configuration Snapshots. Snapshots can be loaded by setting `spring.cloud.azure.appconfiguration.stores[0].snapshot-name` to the snapshot name. Snapshots can be created using the Azure CLI or the Azure Portal. See [here](https://docs.microsoft.com/azure/azure-app-configuration/concept-snapshots) for more information on snapshots. [#7598](https://github.com/Azure/azure-sdk-for-java/pull/37598)
* Added support for trimming prefixes from keys,the default value is the key-filter when key-filter is used. The configuration is `spring.cloud.azure.appconfiguration.stores[0].trim-key-prefix` and `spring.cloud.azure.appconfiguration.stores[0].trim-key-prefixes`.

## 4.14.0 - December 14, 2023

### Enhancements

* Added support for Azure App Configuration Snapshots. Snapshots can be loaded by setting `spring.cloud.azure.appconfiguration.stores[0].snapshot-name` to the snapshot name. Snapshots can be created using the Azure CLI or the Azure Portal. See [here](https://docs.microsoft.com/azure/azure-app-configuration/concept-snapshots) for more information on snapshots. [#7598](https://github.com/Azure/azure-sdk-for-java/pull/37598)
* Added support for trimming prefixes from keys,the default value is the key-filter when key-filter is used. The configuration is `spring.cloud.azure.appconfiguration.stores[0].trim-key-prefix` and `spring.cloud.azure.appconfiguration.stores[0].trim-key-prefixes`.

## 5.6.0 - October 24, 2023

## Bug Fixes

* Fixed a bug where Web Hook authorization was validated incorrectly, resulting in an Unauthorized error [#37141](https://github.com/Azure/azure-sdk-for-java/pull/37141).

## 4.12.0 - October 23, 2023

## Bug Fixes

* Fixed a bug where Web Hook authorization was validated incorrectly, resulting in an Unauthorized error[#37141](https://github.com/Azure/azure-sdk-for-java/pull/37141).

## 5.2.0 - June 01, 2023

### Bug Fixes

* Fixed a bug where where credentials from Azure Spring global properties were being ignored [#34694](https://github.com/Azure/azure-sdk-for-java/pull/34694).
* Fixed a bug where a `NullPointerException` exception was thrown when Azure App Configuration returned an Exception [#35086](https://github.com/Azure/azure-sdk-for-java/pull/35086).

## 4.8.0 - May 25, 2023

### Bug Fixes

* Fixed a bug where where credentials from Azure Spring global properties were being ignored [#34694](https://github.com/Azure/azure-sdk-for-java/pull/34694).
* Fixed a bug where a `NullPointerException` exception was thrown when Azure App Configuration returned an Exception [#35086](https://github.com/Azure/azure-sdk-for-java/pull/35086).

## 4.7.0 - April 06, 2023

### Features Added

* Added support for [Azure Spring common configuration properties][azure_spring_common_configuration_properties].
* Added failover support for App Configuration stores with geo-replication enabled. Configuration is done through the `spring.cloud.azure.appconfiguration.stores[0].endpoints` property or `spring.cloud.azure.appconfiguration.stores[0].connection-strings`, where a list of endpoints/connection-strings can be provided. The endpoints/connection-strings will be attempted to connect in priority order.
* Feature Flags can now be selected using a key and label filter, instead of just a label filter. The configurations are now `spring.cloud.azure.appconfiguration.stores[0].feature-flags.selects[0].key-filter` and `spring.cloud.azure.appconfiguration.stores[0].feature-flags.selects[0].label-filter`.

### Breaking Changes

* Libraries and namespaces have been renamed to `spring-cloud-azure-appconfiguration-config` and `com.azure.spring.cloud.appconfiguration.config`.
* Renamed `ConfigurationClientBuilderSetup` to `ConfigurationClientCustomizer`.
* Renamed `SecretClientBuilderSetup` to `SecretClientCustomizer`.
* Removed `AppConfigurationCredentialProvider` and `KeyVaultCredentialProvider`, instead you can use [Azure Spring common configuration properties][azure_spring_common_configuration_properties] or modify the credentials using `ConfigurationClientCustomizer`/`SecretClientCustomizer`.
* Feature Flags are now merged when loaded from multiple stores, if duplicate keys are found the highest priority store wins.

## 1.3.0 - April 21, 2021

* Updated to newer versions of dependencies. Spring Boot 2.4.3 and Spring Cloud 3.0.1.

## 1.2.9 - March 18, 2021

* Updated to newer versions of dependencies. Spring Boot 2.3.5.RELEASE and Spring Cloud 2.2.5.RELEASE.

## 1.2.8 - March 9, 2021

* Should not be used, released with incorrect version of Spring Boot.

## 1.2.7 - July 14, 2020

* Fixed the bug where setting `spring.cloud.azure.appconfiguration.enabled` to false while using `spring-cloud-azure-appconfiguration-config-web`, resulted in an error on startup. [#332](https://github.com/Azure/AppConfiguration/issues/332)
* Fixed the bug where a 400 Error would be returned from App Configuration when multiple labels and profiles are used at the same time. [#675](https://github.com/microsoft/spring-cloud-azure/issues/675)

## 1.1.5/1.2.6 - May 28, 2020

* Fixed the bug where the configuration cannot be refreshed successfully when this library is used in a Java 11 container running with more than 2 CPUs.

## 1.1.4/1.2.5 - April 27, 2020

* Introduced new interfaces, which allow users to customize client builders for connecting to App Configuration and Key Vault. See this [document](https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-starters/spring-cloud-starter-azure-appconfiguration-config#client-builder-customization) for more details and examples. [#656](https://github.com/microsoft/spring-cloud-azure/issues/656)
  * `ConfigurationClientBuilderSetup`
  * `SecretClientBuilderSetup`
* When fail-fast is disabled, an application may only load configuration under one context but not the other from an App Configuration store. The bug is fixed so either all configuration is loaded successfully or nothing is loaded to ensure the consistency of configuration in the application.
* Fixed the bug that more than necessary requests may be made for configuration change detection especially when an App Configuration store has many keys or many change revisions. [#672](https://github.com/microsoft/spring-cloud-azure/issues/672)
* Fixed the bug that `%00` was not working when it's used to indicate keys with no labels explicitly. Changed to use `\0` to indicate keys with no labels to be consistent with the service. For example, a property setting below means loading keys with no labels and then overwritten by keys with label `dev`. [#655](https://github.com/microsoft/spring-cloud-azure/issues/655)

  ```java
  spring.cloud.azure.appconfiguration.stores[0].label = \0,dev
  ```

## 1.1.3/1.2.3 - April 06, 2020

* Fixed the bug that caused the configurations to refresh extra times when the config store didn't use feature flags. [#298](https://github.com/Azure/AppConfiguration/issues/298)
* Updated to version 1.3.0 of the Azure Core library fixing compatibility with some other Azure libraries. [#659](https://github.com/microsoft/spring-cloud-azure/issues/659)
* Fixed the bug where the number of labels that could be used was maxed at 5.
* Reduced the number of requests made when using multiple labels.

## 1.1.2/1.2.2 - February 25, 2020

* Credentials for authentication can now be provided in code via `AppConfigurationCredentialProvider` and `KeyVaultCredentialProvider`.
  * This method allows for different authentication methods/credentials to be used when connecting with multiple App Configuration instances or Key Vaults.
* The `spring.cloud.azure.appconfiguration.stores[0].fail-fast` setting has been updated to be per store.
  * Previously this setting controlled the error handling for all App Configuration instances, now this setting allows for different error handling to be configured per App Configuration instance.
  * The error handling specified by fail-fast is now limited to when settings are loaded on application startup. This setting does not affect error handling for configuration refresh. However, if there is an error loading from the App Configuration instance on startup and fail-fast is false for that instance, it will not be included in refresh attempts.
* Failed refreshes will now be automatically retried, if not completely successful.
* spring-cloud-azure-appconfiguration-config has been split into two packages: spring-cloud-azure-appconfiguration-config and spring-cloud-azure-appconfiguration-config-web. The web provider take on the spring-web dependency used for automated refresh. In the spring-cloud-azure-appconfiguration-config provider, refresh needs to be manually triggered.
  * To continue using automated refresh, the "spring-cloud-azure-appconfiguration-config" dependency should be updated to "spring-cloud-azure-appconfiguration-config-web".
* `AppConfigurationRefresh`'s `refreshConfigurations()` can be called to check if the settings for each App Configuration instance are up to date and, if the cache has expired, trigger a refresh event.

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
[docs]: https://learn.microsoft.com/azure/azure-app-configuration/quickstart-java-spring-app
[package]: https://mvnrepository.com/artifact/com.azure.spring/spring-cloud-azure-appconfiguration-config
[samples]: https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/appconfiguration
[source_code]: https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/spring/spring-cloud-azure-appconfiguration-config

[package_web]: https://mvnrepository.com/artifact/com.azure.spring/spring-cloud-azure-appconfiguration-config-web
[source_code_web]: https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/spring/spring-cloud-azure-appconfiguration-config-web

[package_starter]: https://mvnrepository.com/artifact/com.azure.spring/spring-cloud-azure-starter-appconfiguration-config
[source_code_starter]: https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/spring/spring-cloud-azure-starter-appconfiguration-config

[azure_spring_common_configuration_properties]: https://learn.microsoft.com/azure/developer/java/spring-framework/configuration
