## Microsoft.Azure.AppConfiguration.Functions.Worker
### [Package (NuGet)](https://www.nuget.org/packages/Microsoft.Azure.AppConfiguration.Functions.Worker)

### 6.1.0 - August 17, 2023
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `6.1.0`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.
* Fixed an issue where concurrent access to HttpContext could cause a null reference exception in middleware used alongside Azure App Configuration middleware.

### 7.0.0-preview - July 20, 2023
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `7.0.0-preview`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 6.0.1 - May 3, 2023
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `6.0.1`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 6.0.0 - March 28, 2023
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `6.0.0`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 5.2.0 - November 29, 2022
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `5.2.0`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 5.3.0-preview - July 27, 2022
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `5.3.0-preview`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 5.2.0-preview - July 18, 2022
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `5.2.0-preview`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 5.1.0 - July 18, 2022
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `5.1.0`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 5.1.0-preview - May 20, 2022
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `5.1.0-preview`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 5.0.1 - Feb 03, 2022
* Moved the `UseAzureAppConfiguration` extension method to `Microsoft.Extensions.Hosting` namespace.  [#299](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/299)
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `5.0.0`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 5.0.0-preview - Dec 16, 2021
* Added middleware to support dynamic configuration in Azure Functions running in an isolated process. Refer to [Azure Functions documentation](https://docs.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide) for more details about running C# Azure Functions in an isolated process. [#287](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/287)