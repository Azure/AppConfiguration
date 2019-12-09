## Microsoft.Azure.AppConfiguration.AspNetCore
### [Package (NuGet)](https://www.nuget.org/packages/Microsoft.Azure.AppConfiguration.AspNetCore)

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
