## Microsoft.Azure.AppConfiguration.AspNetCore
### [Package (NuGet)](https://www.nuget.org/packages/Microsoft.Azure.AppConfiguration.AspNetCore)

### 4.1.0 - December 15, 2020
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `4.1.0`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 4.0.0 - September 11, 2020
* **Breaking Change :** Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `4.0.0`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 4.0.0-preview - July 23, 2020
* Added multi-targeting support for .NET Core 3.1 besides .NET Standard 2.0. [#173](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/173)

* **Breaking Change :** To leverage the new feature of dependency injection support for obtaining `IConfigurationRefresher` instances introduced in the `4.0.0-preview` version of the `Microsoft.Extensions.Configuration.AzureAppConfiguration` package, the following changes are made.
    * Users must call `IServiceCollection.AddAzureAppConfiguration()` in `ConfigureServices(...)` to register the required services for configuration refresh before they can call `IApplicationBuilder.UseAzureAppConfiguration()`. This makes it easier to retrieve instances of `IConfigurationRefresher` through dependency injection in a controller or a middleware, and have better control of when and how configuration is refreshed.

    * An exception is thrown when the required services for configuration refresh could not be retrieved from the `IServiceCollection` instance. [#166](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/166)

* **Breaking Change :** Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `4.0.0-preview`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 3.0.2 - July 01, 2020
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `3.0.2`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 3.0.1 - April 02, 2020
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `3.0.1`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 3.0.0 - February 19, 2020
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `3.0.0`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 3.0.0-preview-011100002-1192 - January 16, 2020
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `3.0.0-preview-011100001-1152`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 3.0.0-preview-010560002-1165 - November 22, 2019
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `3.0.0-preview-010550001-251`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 2.1.0-preview-010380003-1338 - November 04, 2019
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `2.1.0-preview-010380001-1099`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 2.0.0-preview-010060003-1250 - October 03, 2019
* Removed reference to the package `Microsoft.FeatureManagement.AspNetCore`. This helps avoid importing MVC dependencies to applications that don't use MVC. ASP.NET Core applications that use feature management need to reference this package explicitly.
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `2.0.0-preview-010050001-38`. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more information on the changes.

### 2.0.0-preview-009470001-12 - August 06, 2019
* Updated `Microsoft.Extensions.Configuration.AzureAppConfiguration` reference to `2.0.0-preview-009470001-1371` to take in a bug fix. See the [release notes](./MicrosoftExtensionsConfigurationAzureAppConfiguration.md) for more info of the fix.

### 2.0.0-preview-009200001-7 - July, 10 2019
* Added middleware to support dynamic configuration in ASP.NET Core applications
