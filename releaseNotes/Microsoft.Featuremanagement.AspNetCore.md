# Microsoft.FeatureManagement.AspNetCore

[Source code][source_code] | [Package (NuGet)][package] | [Samples][samples] | [Product documentation][docs]

## 4.3.0 - August 27, 2025

* Updated `Microsoft.FeatureManagement` reference to `4.3.0`. See the [release notes](./Microsoft.Featuremanagement.md) for more information on the changes.

## 4.2.1 - July 9, 2025 (Delisted)

This release has been delisted due to the enhancement below resulting in an unintended breaking change. For more information, please go to [#550](https://github.com/microsoft/FeatureManagement-Dotnet/issues/550).

* Updated `Microsoft.FeatureManagement` reference to `4.2.1`. See the [release notes](./Microsoft.Featuremanagement.md) for more information on the changes.

## 4.1.0 - May 22, 2025

### Enhancements

* Updated `Microsoft.FeatureManagement` reference to `4.1.0`. See the [release notes](./Microsoft.Featuremanagement.md) for more information on the changes.

### Bug Fix

* Fixed a bug that caused duplicated dimensions in Application Insights telemetry. [#542](https://github.com/microsoft/FeatureManagement-Dotnet/pull/542)

## 4.0.0 - November 1, 2024

### Enhancements

* Adjusted the `.WithTargeting()` builder method to automatically add `HttpContextAccessor` if it's not already added.
* Added `TargetingHttpContextMiddleware` which persists targeting context in the current activity. This is used when setting up [Telemetry](https://learn.microsoft.com/en-us/azure/azure-app-configuration/feature-management-dotnet-reference#telemetry).
* Added support for variants in `FeatureTagHelper`. This allows MVC views to use the `<feature>` tag to conditionally render content based on whether a specific variant of a feature is assigned.

    ``` HTML+Razor
    <feature name="FeatureX" variant="Alpha">
      <p>This can only be seen if variant 'Alpha' of 'FeatureX' is assigned.</p>
    </feature>
    ```

    For more details on ASP.NET views and variants, see [here](https://learn.microsoft.com/en-us/azure/azure-app-configuration/feature-management-dotnet-reference#view).
* Updated `Microsoft.FeatureManagement` reference to `4.0.0`. See the [release notes](./Microsoft.Featuremanagement.md) for more information on the changes.
* Added support for .NET 9 as a target framework.

### Breaking Change

* The `FeatureTagHelper` constructor now requires an `IVariantFeatureManager` to support new variant functionality. While this class is typically not instantiated directly, any direct instantiation will need to be updated.
* Removed .NET 7 as a target framework as .NET 7 is out of support.

## 4.0.0-preview5 - Oct 24, 2024

* Updated `Microsoft.FeatureManagement` reference to `4.0.0-preview5`. See the [release notes](./Microsoft.Featuremanagement.md) for more information on the changes.

## 4.0.0-preview4 - Jul 19, 2024

* Updated `Microsoft.FeatureManagement` reference to `4.0.0-preview4`. See the [release notes](./Microsoft.Featuremanagement.md) for more information on the changes.

## 3.5.0 - Jul 19, 2024

### Enhancements

* Introduced an overloaded extension method `WithTargeting()` for ASP.NET Core applications. It registers a default `ITargetingContextAccessor` that constructs user context based on the authenticated user of a request. [#466](https://github.com/microsoft/FeatureManagement-Dotnet/pull/466)

## 3.4.0 - Jun 24, 2024

* Updated `Microsoft.FeatureManagement` reference to `3.4.0`. See the [release notes](./Microsoft.Featuremanagement.md) for more information on the changes.

## 3.3.1 - May 23, 2024

* Updated `Microsoft.FeatureManagement` reference to `3.3.1`. See the [release notes](./Microsoft.Featuremanagement.md) for more information on the changes.

## 3.3.0 - May 8, 2024

* Updated `Microsoft.FeatureManagement` reference to `3.3.0`. See the [release notes](./Microsoft.Featuremanagement.md) for more information on the changes.

## 4.0.0-preview3 - April 10, 2024

* Updated `Microsoft.FeatureManagement` reference to `4.0.0-preview3`. See the [release notes](./Microsoft.Featuremanagement.md) for more information on the changes.

## 4.0.0-preview2 - March 7, 2024

### Enhancements

* Introduced a new ASP.NET Core middleware called `TargetingHttpContextMiddleware`. It makes targeting information available from `HttpContext` on each request. ([#409](https://github.com/microsoft/FeatureManagement-Dotnet/issues/409))
* Added support for .NET 8 target framework. ([#364](https://github.com/microsoft/FeatureManagement-Dotnet/issues/364))

<!-- LINKS -->
[docs]: https://github.com/microsoft/FeatureManagement-Dotnet
[package]: https://www.nuget.org/packages/Microsoft.FeatureManagement.AspNetCore
[samples]: https://github.com/microsoft/FeatureManagement-Dotnet/tree/master/examples/FeatureFlagDemo
[source_code]: https://github.com/microsoft/FeatureManagement-Dotnet/tree/master/src/Microsoft.FeatureManagement.AspNetCore