# azure-spring-cloud-appconfiguration-config

[Source code][source_code] | [Package (Maven)][package] | [README][readme] | [Product documentation][docs] | [Samples][samples]

# azure-spring-cloud-appconfiguration-config-web

[Source code web][source_code_web] | [Package (Maven) web][package_web] | [Product documentation][docs]

## 2.1.1 - September 29, 2021

* Fixed a bug where a watch key may not work as expected. If you use a key with no label as the watch key and delete the key in App Configuration, if the key has values with other labels, the key with another label could be used as the watch key.
* Fixed a bug where adding or deleting a feature flag in App Configuration wouldn't cause the feature flag to be added or deleted in users' applications. [#24049](https://github.com/Azure/azure-sdk-for-java/issues/24049)

## 2.1.0 - September 06, 2021

* Added Health Monitor support with Spring Actuator. [#21982](https://github.com/Azure/azure-sdk-for-java/issues/21982)
* Added new API which allows users to configure the behavior when a Key Vault reference which cannot be resolved. `KeyVaultSecretProvider` can be used to resolve the references using its `getSecret` method, which provides a URI to the requested secret.

    ```java
    @Bean
    public KeyVaultSecretProvider keyVaultSecretProvider() {
        return new KeyVaultSecretProvider() {
            
            @Override
            public String getSecret(String uri) {
    
                ...
    
                return mySecret;
            }
        };
    }
    ```

## 2.0.0 - July 22, 2021

* Enables loading from multiple App Configuration stores.
* Each store can load configurations by using key/label `selects`. Many `selects` can be used, default is key = `/applicaiton/*` with label = `${spring.profiles.active}` or if null `\0`. See [README][readme] for full configuration.
* Authentication via Connection String, Managed Identity, or any method supported by Azure Identity SDK, see Token Credential Provider in README.
* App Configuration stores can be monitored for changes. This is done by a specified configuration know as a trigger being checked in the App Configuration store on a set interval. Each individual App Configuration store can be enabled/disabled for monitoring and have a different interval in which they are checked. Monitoring can be done through either a Push or Pull model.
  * Pull Monitoring checks the config store for changes strictly based on how long it has been since the last check and activity in the application.
  * Push Monitoring has the App Configuration store notify the Application that configurations have changed through a web-hook.
* Feature Flag loading can be enabled per config store `spring.cloud.azure.appconfiguration.stores[0].feature-flags.enable`
  * A single label can be used to load feature flags, default `\0` i.e. (No Label).
  * A cache expiration can be set for feature flags.
* Configurations can be set using: Key-Value, Key Vault Reference, Placeholder Values i.e. `${my.config}`, [Json](https://docs.microsoft.com/azure/azure-app-configuration/howto-leverage-json-content-type)

<!-- LINKS -->
[docs]: https://docs.microsoft.com/azure/azure-app-configuration/quickstart-java-spring-app
[package]: https://mvnrepository.com/artifact/com.azure.spring/azure-spring-cloud-appconfiguration-config
[samples]: https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/appconfiguration
[source_code]: https://github.com/Azure/azure-sdk-for-java/tree/master/sdk/appconfiguration/azure-spring-cloud-appconfiguration-config
[token_credentials]: https://github.com/Azure/azure-sdk-for-java/blob/master/sdk/identity/azure-identity/README.md
[readme]: https://github.com/Azure/azure-sdk-for-java/tree/master/sdk/appconfiguration/azure-spring-cloud-starter-appconfiguration-config

[package_web]: https://mvnrepository.com/artifact/com.azure.spring/azure-spring-cloud-appconfiguration-config-web
[source_code_web]: https://github.com/Azure/azure-sdk-for-java/tree/master/sdk/appconfiguration/azure-spring-cloud-appconfiguration-config-web