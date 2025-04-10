# Azure App Configuration

Azure App Configuration is an Azure service that allows users to manage configuration within the cloud. Users can create App Configuration stores to store key-value settings and consume stored settings from applications, build pipelines, release processes, microservices, and other Azure resources. See the [documentation](https://aka.ms/AzureAppConfiguration) for more information.

You can open [issues](https://github.com/Azure/AppConfiguration/issues?utf8=%E2%9C%93&q=is%3Aissue) to ask questions or share feedback about Azure App Configuration.

## Announcements
Subscribe to the following repo to be notified of announcements and updates about Azure App Configuration.
  * [Azure/AppConfiguration-Announcements](https://github.com/Azure/AppConfiguration-Announcements)

## Roadmap
Check out what is on the [roadmap](https://github.com/Azure/AppConfiguration/projects/1) of Azure App Configuration and what the team is currently working on.

## Examples
Learn how to use Azure App Configuration in your app from [a variety of examples](./examples/README.md).

## REST API Reference

Follow the links below for the Azure App Configuration REST API reference.
  * [Key-value data operation](https://docs.microsoft.com/azure/azure-app-configuration/rest-api)
  * [Configuration store management](https://docs.microsoft.com/rest/api/appconfiguration/)

## Client Libraries

#### Configuration Providers

Load key-values in App Configuration to your application's existing configuration system with ease.

Module | Platform | Sample | Release Notes
------ | -------- | ------ | -------------
[Microsoft.Extensions.Configuration.AzureAppConfiguration](https://github.com/Azure/AppConfiguration-DotnetProvider)<br/>[![NuGet](https://img.shields.io/nuget/v/Microsoft.Extensions.Configuration.AzureAppConfiguration.svg?color=blue)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.AzureAppConfiguration/) | .NET Standard | [Sample](https://github.com/Azure/AppConfiguration/tree/main/examples/DotNetCore) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/MicrosoftExtensionsConfigurationAzureAppConfiguration.md)
[Microsoft.Azure.AppConfiguration.AspNetCore](https://github.com/Azure/AppConfiguration-DotnetProvider)<br/>[![NuGet](https://img.shields.io/nuget/v/Microsoft.Azure.AppConfiguration.AspNetCore.svg?color=blue)](https://www.nuget.org/packages/Microsoft.Azure.AppConfiguration.AspNetCore/) | ASP&#46;NET Core | [Sample](https://github.com/Azure/AppConfiguration/tree/main/examples/DotNetCore) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/MicrosoftAzureAppConfigurationAspNetCore.md)
[Microsoft.Azure.AppConfiguration.Functions.Worker](https://github.com/Azure/AppConfiguration-DotnetProvider)<br/>[![NuGet](https://img.shields.io/nuget/v/Microsoft.Azure.AppConfiguration.Functions.Worker.svg?color=blue)](https://www.nuget.org/packages/Microsoft.Azure.AppConfiguration.Functions.Worker/) | Azure Functions<br/>(Isolated process) | [Sample](https://github.com/Azure/AppConfiguration/tree/main/examples/DotNetCore/AzureFunction/FunctionAppIsolatedMode) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/MicrosoftAzureAppConfigurationFunctionsWorker.md)
[Microsoft.Configuration.ConfigurationBuilders.AzureAppConfiguration](https://github.com/aspnet/MicrosoftConfigurationBuilders/tree/main/src/AzureAppConfig)<br/>[![NuGet](https://img.shields.io/nuget/v/Microsoft.Configuration.ConfigurationBuilders.AzureAppConfiguration.svg?color=blue)](https://www.nuget.org/packages/Microsoft.Configuration.ConfigurationBuilders.AzureAppConfiguration/) | .NET Framework | [Sample](https://github.com/Azure/AppConfiguration/tree/main/examples/DotNetFramework/WebDemo) | [Release Notes](https://github.com/aspnet/MicrosoftConfigurationBuilders/releases)
[spring-cloud-azure-appconfiguration-config](https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/spring/spring-cloud-azure-appconfiguration-config)<br/>[![Maven Central](https://img.shields.io/maven-central/v/com.azure.spring/spring-cloud-azure-appconfiguration-config.svg?color=blue)](https://search.maven.org/artifact/com.azure.spring/spring-cloud-azure-appconfiguration-config) | Spring Boot | [Sample](https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/appconfiguration/spring-cloud-azure-starter-appconfiguration-config/spring-cloud-azure-starter-appconfiguration-config-sample) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/SpringCloudAzureAppConfigurationConfig.md)
[spring-cloud-azure-appconfiguration-config-web](https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/spring/spring-cloud-azure-appconfiguration-config-web)<br/>[![Maven Central](https://img.shields.io/maven-central/v/com.azure.spring/spring-cloud-azure-appconfiguration-config-web.svg?color=blue)](https://search.maven.org/artifact/com.azure.spring/spring-cloud-azure-appconfiguration-config-web) | Spring Boot | [Sample](https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/appconfiguration/spring-cloud-azure-starter-appconfiguration-config/spring-cloud-azure-starter-appconfiguration-config-sample) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/SpringCloudAzureAppConfigurationConfig.md)
[azure-appconfiguration-provider](https://github.com/Azure/azure-sdk-for-python/tree/main/sdk/appconfiguration/azure-appconfiguration-provider)<br/>[![Pypi](https://img.shields.io/pypi/v/azure-appconfiguration-provider.svg?color=blue)](https://pypi.org/project/azure-appconfiguration-provider/) | Python | [Sample](https://github.com/Azure/azure-sdk-for-python/tree/main/sdk/appconfiguration/azure-appconfiguration-provider/samples) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/AzureAppConfigurationProviderPython.md)
[@azure/app-configuration-provider](https://github.com/Azure/AppConfiguration-JavaScriptProvider)<br/>[![Npm](https://img.shields.io/npm/v/@azure/app-configuration-provider?color=blue)](https://www.npmjs.com/package/@azure/app-configuration-provider) | JavaScript | [Sample](https://github.com/Azure/AppConfiguration-JavaScriptProvider/tree/main/examples) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/JavaScriptProvider.md)
[azureappconfiguration](https://github.com/Azure/AppConfiguration-GoProvider)<br/>[![Go](https://pkg.go.dev/badge/github.com/gin-gonic/gin?status.svg)](https://pkg.go.dev/github.com/Azure/AppConfiguation-GoProvider/azureappconfiguration) | Go | [Sample](https://github.com/Azure/AppConfiguration-GoProvider/tree/main/examples) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/GoProvider.md)

#### Feature Management Libraries

Use feature flags in your application and leverage App Configuration for dynamic feature management.

Module | Platform | Sample | Release Notes
------ | -------- | ------ | -------------
[Microsoft.FeatureManagement](https://github.com/microsoft/FeatureManagement-Dotnet)<br/>[![NuGet](https://img.shields.io/nuget/v/Microsoft.FeatureManagement.svg?color=blue)](https://www.nuget.org/packages/Microsoft.FeatureManagement)| .NET Standard | [Sample](https://github.com/microsoft/FeatureManagement-Dotnet/tree/main/examples) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/Microsoft.Featuremanagement.md)
[Microsoft.FeatureManagement.AspNetCore](https://github.com/microsoft/FeatureManagement-Dotnet)<br/>[![NuGet](https://img.shields.io/nuget/v/Microsoft.FeatureManagement.AspNetCore.svg?color=blue)](https://www.nuget.org/packages/Microsoft.FeatureManagement.AspNetCore) | ASP&#46;NET Core | [Sample](https://github.com/microsoft/FeatureManagement-Dotnet/tree/main/examples) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/Microsoft.Featuremanagement.md)
[spring-cloud-azure-feature-management](https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/spring/spring-cloud-azure-feature-management)<br/>[![Maven Central](https://img.shields.io/maven-central/v/com.azure.spring/spring-cloud-azure-feature-management.svg?color=blue)](https://search.maven.org/artifact/com.azure.spring/spring-cloud-azure-feature-management) | Spring Boot | [Sample](https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/appconfiguration/spring-cloud-azure-feature-management/spring-cloud-azure-feature-management-sample) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/SpringCloudAzureFeatureManagement.md)
[spring-cloud-azure-feature-management-web](https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/spring/spring-cloud-azure-feature-management-web)<br/>[![Maven Central](https://img.shields.io/maven-central/v/com.azure.spring/spring-cloud-azure-feature-management-web.svg?color=blue)](https://search.maven.org/artifact/com.azure.spring/spring-cloud-azure-feature-management-web) | Spring Boot | [Sample](https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/appconfiguration/spring-cloud-azure-feature-management-web/spring-cloud-azure-feature-management-web-sample) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/SpringCloudAzureFeatureManagement.md)
[featuremanagement](https://github.com/microsoft/FeatureManagement-Python)<br/>[![PyPi](https://img.shields.io/pypi/v/FeatureManagement?color=blue)](https://pypi.org/project/FeatureManagement/) | Python | [Sample](https://github.com/microsoft/FeatureManagement-Python/tree/main/samples) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/PythonFeatureManagement.md)
[@microsoft/feature-management](https://github.com/microsoft/FeatureManagement-JavaScript)<br/>[![npm](https://img.shields.io/npm/v/@microsoft/feature-management?color=blue)](https://www.npmjs.com/package/@microsoft/feature-management) | JavaScript | [Sample](https://github.com/microsoft/FeatureManagement-JavaScript/tree/main/examples) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/JavaScriptFeatureManagement.md)

#### SDKs

Create, read, update and delete key-values, feature flags, and Key Vault references in App Configuration programmatically.

Module | Platform | Sample | Release Notes
------ | -------- | ------ | -------------
[Azure.Data.AppConfiguration](https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/appconfiguration/Azure.Data.AppConfiguration)<br/>[![NuGet](https://img.shields.io/nuget/v/Azure.Data.AppConfiguration.svg?color=blue)](https://www.nuget.org/packages/Azure.Data.AppConfiguration/) | .NET Standard| [Sample](https://github.com/Azure/azure-sdk-for-net/tree/main/sdk/appconfiguration/Azure.Data.AppConfiguration/samples) | [Release Notes](https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/appconfiguration/Azure.Data.AppConfiguration/CHANGELOG.md)
[azure-data-appconfiguration](https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/appconfiguration/azure-data-appconfiguration)<br/>[![Maven Central](https://img.shields.io/maven-central/v/com.azure/azure-data-appconfiguration.svg?color=blue)](https://search.maven.org/artifact/com.azure/azure-data-appconfiguration) | Java | [Sample](https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/appconfiguration/azure-data-appconfiguration/src/samples) | [Release Notes](https://github.com/Azure/azure-sdk-for-java/blob/main/sdk/appconfiguration/azure-data-appconfiguration/CHANGELOG.md)
[azure/app-configuration](https://github.com/Azure/azure-sdk-for-js/tree/main/sdk/appconfiguration/app-configuration)<br/>[![npm](https://img.shields.io/npm/v/@azure/app-configuration.svg?color=blue)](https://www.npmjs.com/package/@azure/app-configuration) | JavaScript | [Sample](https://github.com/Azure/azure-sdk-for-js/tree/main/sdk/appconfiguration/app-configuration/samples) | [Release Notes](https://github.com/Azure/azure-sdk-for-js/blob/main/sdk/appconfiguration/app-configuration/CHANGELOG.md)
[azure-appconfiguration](https://github.com/Azure/azure-sdk-for-python/tree/main/sdk/appconfiguration/azure-appconfiguration)<br/>[![pypi](https://img.shields.io/pypi/v/azure-appconfiguration.svg?color=blue)](https://pypi.org/project/azure-appconfiguration/) | Python | [Sample](https://github.com/Azure/azure-sdk-for-python/tree/main/sdk/appconfiguration/azure-appconfiguration/samples) | [Release Notes](https://github.com/Azure/azure-sdk-for-python/blob/main/sdk/appconfiguration/azure-appconfiguration/CHANGELOG.md)
[azappconfig](https://github.com/Azure/azure-sdk-for-go/tree/main/sdk/data/azappconfig)<br/>[![Go](https://pkg.go.dev/badge/github.com/gin-gonic/gin?status.svg)](https://pkg.go.dev/github.com/Azure/azure-sdk-for-go/sdk/data/azappconfig) | Go | [Sample](https://github.com/Azure/azure-sdk-for-go/blob/main/sdk/data/azappconfig/examples_test.go) | [Release Notes](https://github.com/Azure/azure-sdk-for-go/blob/main/sdk/data/azappconfig/CHANGELOG.md)

## Platform Integration

Leverage App Configuration from the platforms you are already using with minimum effort.

Name | Platform | Document | Release Notes
---- | -------- | -------- | -------------
[Azure App Configuration Kubernetes Provider](https://github.com/Azure/AppConfiguration-KubernetesProvider)<br/>[![MAR](https://img.shields.io/badge/dynamic/json?url=https%3A%2F%2Fraw.githubusercontent.com%2FAzure%2FAppConfiguration-KubernetesProvider%2Fmain%2Fversion.json&query=%24.version&prefix=v&label=mar)](https://mcr.microsoft.com/en-us/product/azure-app-configuration/kubernetes-provider/about) | Kubernetes | [Document](https://learn.microsoft.com/azure/azure-app-configuration/quickstart-azure-kubernetes-service) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/KubernetesProvider.md)
[@azure/app-configuration-importer](https://github.com/Azure/AppConfiguration-JavaScriptImporter)<br/>[![npm](https://img.shields.io/npm/v/@azure/app-configuration-importer?color=blue)](https://www.npmjs.com/package/@azure/app-configuration-importer) | JavaScript Importer | [Document](https://github.com/Azure/AppConfiguration-JavaScriptImporter) | 
App Configuration reference | App Service & Azure Functions | [Document](https://learn.microsoft.com/en-us/azure/app-service/app-service-configuration-references) |
Azure App Configuration | Azure Pipeline | [Document](https://learn.microsoft.com/en-us/azure/azure-app-configuration/pull-key-value-devops-pipeline) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/AzureDevOpsPipelineExtension.md)
Azure App Configuration Push | Azure Pipeline | [Document](https://learn.microsoft.com/en-us/azure/azure-app-configuration/push-kv-devops-pipeline) | [Release Notes](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/AzureDevOpsPushPipelineExtension.md)
[Github Action](https://github.com/Azure/AppConfiguration-Sync) | Github | [Document](https://learn.microsoft.com/en-us/azure/azure-app-configuration/concept-github-action) | [Release Notes](https://github.com/Azure/AppConfiguration-Sync/releases)

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
