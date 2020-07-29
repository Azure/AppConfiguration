# Azure App Configuration

Azure App Configuration is an Azure service that allows users to manage configuration within the cloud. Users can create App Configuration stores to store key-value settings and consume stored settings from within applications, deployment pipelines, release processes, microservices, and other Azure resources.

You can open [issues](https://github.com/Azure/AppConfiguration/issues?utf8=%E2%9C%93&q=is%3Aissue) to ask questions or share feedback about the Azure App Configuration service. If you have a suggestion or a request for a feature, please share your idea with us at [Azure Feedback](https://feedback.azure.com/forums/920545-azure-app-configuration) so others can vote.

Documentation: https://aka.ms/AzureAppConfiguration

Slack: https://aka.ms/azconfig/slack

## Announcements
Subscribe to the following repo to be notified of announcements and updates about Azure App Configuration.
  * [Azure/AppConfiguration-Announcements](https://github.com/Azure/AppConfiguration-Announcements)

## REST API Reference

The following reference pages are available to describe the Azure App Configuration API surface in detail.

**Resources**
  * [Keys](./docs/REST/keys.md)
  * [Key-Values](./docs/REST/kv.md)
  * [Labels](./docs/REST/labels.md)
  * [Locks](./docs/REST/locks.md)
  * [Revisions](./docs/REST/revisions.md)

**Protocol**
  * [Authentication](./docs/REST/authentication/index.md)
  * [Authorization](./docs/REST/authorization/index.md)
  * [Consistency Model](./docs/REST/consistency.md)
  * [Common Headers](./docs/REST/headers.md)
  * [Throttling](./docs/REST/throttling.md)
  * [Versioning](./docs/REST/versioning.md)

**Development**
  * [Fiddler](./docs/Development/fiddler.md)
  * [Postman](./docs/Development/postman.md)

## Client Libraries

#### Configuration Providers

Load key-values in App Configuration to your application's existing configuration system with ease.

Module | Platform | Sample | Release Notes
------ | -------- | ------ | -------------
[Microsoft.Extensions.Configuration.AzureAppConfiguration](https://github.com/Azure/AppConfiguration-DotnetProvider)<br/>[![NuGet](https://img.shields.io/nuget/v/Microsoft.Extensions.Configuration.AzureAppConfiguration.svg?color=blue)](https://www.nuget.org/packages/Microsoft.Azure.AppConfiguration.AspNetCore/) | .Net Standard | [Sample](https://github.com/Azure/AppConfiguration/tree/master/examples/DotNetCore) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/master/releaseNotes/MicrosoftAzureAppConfigurationAspNetCore.md)
[Microsoft.Azure.AppConfiguration.AspNetCore](https://github.com/Azure/AppConfiguration-DotnetProvider)<br/>[![NuGet](https://img.shields.io/nuget/v/Microsoft.Azure.AppConfiguration.AspNetCore.svg?color=blue)](https://www.nuget.org/packages/Microsoft.Azure.AppConfiguration.AspNetCore/) | ASP&#46;NET Core | [Sample](https://github.com/Azure/AppConfiguration/tree/master/examples/DotNetCore) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/master/releaseNotes/MicrosoftExtensionsConfigurationAzureAppConfiguration.md)
[Microsoft.Configuration.ConfigurationBuilders.AzureAppConfiguration](https://github.com/aspnet/MicrosoftConfigurationBuilders/tree/master/src/AzureAppConfig)<br/>[![NuGet](https://img.shields.io/nuget/v/Microsoft.Configuration.ConfigurationBuilders.AzureAppConfiguration.svg?color=blue)](https://www.nuget.org/packages/Microsoft.Configuration.ConfigurationBuilders.AzureAppConfiguration/) | .NET Framework | [Sample](https://github.com/Azure/AppConfiguration/tree/master/examples/DotNetFramework/WebDemo) |
[spring-cloud-azure-appconfiguration-config](https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-appconfiguration-config)<br/>[![Maven Central](https://img.shields.io/maven-central/v/com.microsoft.azure/spring-cloud-azure-appconfiguration-config.svg?color=blue)](https://search.maven.org/artifact/com.microsoft.azure/spring-cloud-azure-appconfiguration-config) | Spring Boot | [Sample](https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-samples/azure-appconfiguration-sample) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/master/releaseNotes/SpringCloudAzureAppConfigurationConfig.md)
[spring-cloud-azure-appconfiguration-config-web](https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-appconfiguration-config-web)<br/>[![Maven Central](https://img.shields.io/maven-central/v/com.microsoft.azure/spring-cloud-azure-appconfiguration-config-web.svg?color=blue)](https://search.maven.org/artifact/com.microsoft.azure/spring-cloud-azure-appconfiguration-config-web) | Spring Boot | [Sample](https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-samples/azure-appconfiguration-sample) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/master/releaseNotes/SpringCloudAzureAppConfigurationConfig.md)
#### Feature Management Libraries

Use feature flags in your application and leverage App Configuration for dynamic feature management.

Module | Platform | Sample | Release Notes
------ | -------- | ------ | -------------
[Microsoft.FeatureManagement](https://github.com/microsoft/FeatureManagement-Dotnet)<br/>[![NuGet](https://img.shields.io/nuget/v/Microsoft.FeatureManagement.svg?color=blue)](https://www.nuget.org/packages/Microsoft.FeatureManagement)| .Net Standard | [Sample](https://github.com/microsoft/FeatureManagement-Dotnet/tree/master/examples) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/master/releaseNotes/Microsoft.Featuremanagement.md)
[Microsoft.FeatureManagement.AspNetCore](https://github.com/microsoft/FeatureManagement-Dotnet)<br/>[![NuGet](https://img.shields.io/nuget/v/Microsoft.FeatureManagement.AspNetCore.svg?color=blue)](https://www.nuget.org/packages/Microsoft.FeatureManagement.AspNetCore) | ASP&#46;NET Core | [Sample](https://github.com/microsoft/FeatureManagement-Dotnet/tree/master/examples) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/master/releaseNotes/Microsoft.Featuremanagement.md)
[spring-cloud-azure-feature-management](https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-feature-management)<br/>[![Maven Central](https://img.shields.io/maven-central/v/com.microsoft.azure/spring-cloud-azure-feature-management.svg?color=blue)](https://search.maven.org/artifact/com.microsoft.azure/spring-cloud-azure-feature-management) | Spring Boot | [Sample](https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-samples/feature-management-sample) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/master/releaseNotes/SpringCloudAzureFeatureManagement.md)
[spring-cloud-azure-feature-management-web](https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-feature-management-web)<br/>[![Maven Central](https://img.shields.io/maven-central/v/com.microsoft.azure/spring-cloud-azure-feature-management-web.svg?color=blue)](https://search.maven.org/artifact/com.microsoft.azure/spring-cloud-azure-feature-management-web) | Spring Boot | [Sample](https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-samples/feature-management-web-sample) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/master/releaseNotes/SpringCloudAzureFeatureManagement.md)

#### SDKs

Create, read, update and delete key-values in App Configuration programmatically.

Module | Platform | Sample | Release Notes
------ | -------- | ------ | -------------
[Azure.Data.AppConfiguration](https://github.com/Azure/azure-sdk-for-net/tree/master/sdk/appconfiguration/Azure.Data.AppConfiguration)<br/>[![NuGet](https://img.shields.io/nuget/v/Azure.Data.AppConfiguration.svg?color=blue)](https://www.nuget.org/packages/Azure.Data.AppConfiguration/) | .NET Standard| [Sample](https://github.com/Azure/azure-sdk-for-net/tree/master/sdk/appconfiguration/Azure.Data.AppConfiguration/samples) | [Release Notes](https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/appconfiguration/Azure.Data.AppConfiguration/CHANGELOG.md)
[azure-data-appconfiguration](https://github.com/Azure/azure-sdk-for-java/tree/master/sdk/appconfiguration/azure-data-appconfiguration)<br/>[![Maven Central](https://img.shields.io/maven-central/v/com.azure/azure-data-appconfiguration.svg?color=blue)](https://search.maven.org/artifact/com.azure/azure-data-appconfiguration) | Java | [Sample](https://github.com/Azure/azure-sdk-for-java/tree/master/sdk/appconfiguration/azure-data-appconfiguration/src/samples) | [Release Notes](https://github.com/Azure/azure-sdk-for-java/blob/master/sdk/appconfiguration/azure-data-appconfiguration/CHANGELOG.md)
[azure/app-configuration](https://github.com/Azure/azure-sdk-for-js/tree/master/sdk/appconfiguration/app-configuration)<br/>[![npm](https://img.shields.io/npm/v/@azure/app-configuration.svg?color=blue)](https://www.npmjs.com/package/@azure/app-configuration) | JavaScript | [Sample](https://github.com/Azure/azure-sdk-for-js/tree/master/sdk/appconfiguration/app-configuration/samples) | [Release Notes](https://github.com/Azure/azure-sdk-for-js/blob/master/sdk/appconfiguration/app-configuration/CHANGELOG.md)
[azure-appconfiguration](https://github.com/Azure/azure-sdk-for-python/tree/master/sdk/appconfiguration/azure-appconfiguration)<br/>[![pypi](https://img.shields.io/pypi/v/azure-appconfiguration.svg?color=blue)](https://pypi.org/project/azure-appconfiguration/) | Python | [Sample](https://github.com/Azure/azure-sdk-for-python/tree/master/sdk/appconfiguration/azure-appconfiguration/samples) | [Release Notes](https://github.com/Azure/azure-sdk-for-python/blob/master/sdk/appconfiguration/azure-appconfiguration/CHANGELOG.md)

## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
