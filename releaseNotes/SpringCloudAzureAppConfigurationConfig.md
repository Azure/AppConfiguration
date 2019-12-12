# spring-cloud-azure-appconfiguration-config

[Source code][source_code] | [Package (Maven)][package] | [Product documentation][docs] | [Samples][samples]

## 1.1.0.M6 - December 09, 2019

* Updated managed identity support for both system-assigned managed identity and user-assigned managed identity.

    **Before**

  * The managed identity is assigned Contributor role to the App Configuration instance.

    **After**

  * The system-assigned managed identity also requires the client-id to be set in addition to the config store name, for example,

    ```properties
    spring.cloud.azure.appconfiguration.managed-identity.client-id=[client id]
    spring.cloud.azure.appconfiguration.stores[0].name=[config store name]
    ```

  * The managed identity should be assigned App Configuration Data Reader role to the App Configuration instance.

* Added support of generic Azure Active Directory (AAD) authentication. Users can use any of the Azure TokenCredentials from the [Azure Identity client library][token_credentials] to access the App Configuration and Key Vault.

  1. Set `spring.cloud.azure.appconfiguration.stores[0].name` to your App Configuration store name.
  1. Add a `@Bean` of `TokenCredentialProvider` to your application. For example, the following code snippet used the UsernamePasswordCredential for the authorization with both the App Configuration and Key Vault.

```java
public class MyCredentials implements TokenCredentialProvider {

    @Override
    public TokenCredential credentialForAppConfig() {
            return buildCredential();
    }

    @Override
    public TokenCredential credentialForKeyVault() {
            return buildCredential();
    }

    TokenCredential buildCredential() {
            return new UsernamePasswordCredentialBuilder()
                    .clientId("<YOUR_CLIENT_ID>")
                    .username("<YOUR_USERNAME>")
                    .password("<YOUR_PASSWORD>")
                    .build();
    }

}
```

* To prevent unintentional results, an IllegalArgumentException exception will be thrown if both `spring.cloud.azure.appconfiguration.stores[0].name` and `spring.cloud.azure.appconfiguration.stores[0].connection-string` are provided.
* Adds support for providing Token Credentials to the provider using the TokenCredentialProvider.
* Fix Bug where disabled feature filters would be processed incorrectly resulting in the feature filters still running.

## 1.1.0.M5 - October 28, 2019

* Switched to using Azure SDK.
* Added support for using Key Vault References.
* Auto retry on failure of up to 12 times in a 60 second window.
* Now watch only checks for updates when a ServletRequestHandlerEvent is triggered and a update hasn't been happened in the refresh window.

## 1.1.0.M4 - August 26, 2019

* Added Feature Management Support.

<!-- LINKS -->
[client_id]: https://github.com/microsoft/spring-cloud-azure
[docs]: https://docs.microsoft.com/azure/azure-app-configuration/quickstart-java-spring-app
[package]: https://mvnrepository.com/artifact/com.microsoft.azure/spring-cloud-azure-appconfiguration-config
[samples]: https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-samples
[source_code]: https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-appconfiguration-config
[token_credentials]: https://github.com/Azure/azure-sdk-for-java/blob/master/sdk/identity/azure-identity/README.md
