# azure-spring-cloud-appconfiguration-config

[Source code][source_code] | [Package (Maven)][package] | [Product documentation][docs] | [Samples][samples]

# azure-spring-cloud-appconfiguration-config-web

[Source code web ][source_code_web] | [Package (Maven) web][package_web] | [Product documentation][docs]

## 2.0.0-beta.1 - May 5, 2021

* Enables loading of configurations from multiple config stores.
  * Can load configurations by using key/label `selects`. Many `selects` can be used, default is key = `/applicaiton/*` with label = `${spring.profiles.active}` or if null `\0`
  * Authentication vis Connection String, Managed Identity, or any method supported by Azure Identity SDK, see Token Credential Provider bellow.
  * Each individual store can have monitoring enabled and disabled and have a cache-expiration time set.
    * Monitoring requires at least one trigger to be set. `spring.cloud.azure.appconfiguration.stores[0].monitoring.triggers[0].key` is required `spring.cloud.azure.appconfiguration.stores[0].monitoring.triggers[0].label` is optional.
    * Monitoring has both Pull and Push model options.
      * Pull checks the config store for changes based on Activity when the cache has expired.
      * Push involves setting up a Azure Event Grid Web Hook to notify the client application that a change has occurred. Two Spring Actuator Endpoints exist to connect to. `appconfiguration-refresh` triggers the cache to reset on configurations on an application. `appconfiguration-refresh-bus` triggers a refresh on all instances subscribed to the same Service Bus.
        * Push refresh requires a token name and value to be set for verification. The application needs to be running when creating the Web Hook as a response needs to be returned to setup a Web Hook.
  * Feature Flag loading can be enabled per config store `spring.cloud.azure.appconfiguration.stores[0].feature-flags.enable`
    * A single label can be used to load feature flags, default `\0` i.e. (No Label).
    * A cache expiration can be set for feature flags.
* Configuration Support
  * Key-Value
  * Key Vault Reference
  * Placeholder Values i.e. `${my.config}`
  * [Json](https://docs.microsoft.com/azure/azure-app-configuration/howto-leverage-json-content-type)

<!-- LINKS -->
[docs]: https://docs.microsoft.com/azure/azure-app-configuration/quickstart-java-spring-app
[package]: https://mvnrepository.com/artifact/com.azure.spring/azure-spring-cloud-appconfiguration-config
[samples]: https://github.com/Azure/azure-sdk-for-java/tree/master/sdk/spring/azure-spring-boot-samples/feature-management-web-sample
[source_code]: https://github.com/Azure/azure-sdk-for-java/tree/master/sdk/appconfiguration/azure-spring-cloud-appconfiguration-config
[token_credentials]: https://github.com/Azure/azure-sdk-for-java/blob/master/sdk/identity/azure-identity/README.md

[package_web]: https://mvnrepository.com/artifact/com.azure.azure/azure-spring-cloud-appconfiguration-config-web
[source_code_web]: https://github.com/Azure/azure-sdk-for-java/tree/master/sdk/appconfiguration/azure-spring-cloud-appconfiguration-config-web