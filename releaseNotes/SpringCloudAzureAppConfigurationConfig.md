# spring-cloud-azure-appconfiguration-config

[Source code][source_code] | [Package (Maven)][package] | [Product documentation][docs] | [Samples][samples]

## 1.1.0.M6 - December 09, 2019

* Updated support for AAD. System Assigned identity previously required; config store name, and the Contributor role. Now it requires; config store config store name, [client id][client_id], and the App Configuration Data Reader role.

```properties
spring.cloud.azure.appconfiguration.managed-identity.client-id=[client id]
spring.cloud.azure.appconfiguration.stores[0].name=[config store name]
```

* Updated support for AAD. User Assigned identity previously required; config store name, client id, object id, and the Contributor role. Now it required; config store name, client id, and the App Configuration Data Reader role.
* Added the following Provider to allow users to provide any [Azure TokenCredentials][token_credentials] to the provider for authenticating with App Configuration & Key Vault. When a `TokenCredentialProvider` `@Bean` is made it will be used for authentication.

**Note:** Null is valid value to be returned if only one of the authentication methods needs to be used.

**Note 2:** When using TokenCredentialProvider `spring.cloud.azure.appconfiguration.stores[0].name` still needs to be specified in a configuration file.

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

* An error is now thrown when more than 1 authentication method is provided.
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
