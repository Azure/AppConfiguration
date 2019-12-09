# spring-cloud-azure-appconfiguration-config

[Source code][source_code] | [Package (Maven)][package] | [Product documentation][docs] | [Samples][samples]

## 1.1.0.M6 - December 09, 2019

* Added support for AAD, users can now [Azure TokenCredentials][token_credentials]. Token Credential support allows setting Azure Client ID, Azure Tenant ID, and Azure Client Secret through Environment Variables.
* Adds support for providing Token Credentials to the provider using the TokenCredentialProvider.
* Fix Bug where off feature filters would be processed incorrectly resulting in the feature filters still running.

### Breaking Change

* The Role Contributor is no longer used. Two new roles have been added App Configuration Data Owner and App Configuration Data Reader. The member added to the role only needs App Configuration Data Reader for the provider.

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
