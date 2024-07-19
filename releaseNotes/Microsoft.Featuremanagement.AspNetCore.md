# Microsoft.FeatureManagement.AspNetCore

[Source code][source_code] | [Package (NuGet)][package] | [Samples][samples] | [Product documentation][docs]

## 4.0.0-preview4 - Jul 19, 2024

* Updated `Microsoft.FeatureManagement` reference to `4.0.0-preview4`. See the [release notes](./Microsoft.Featuremanagement.md) for more information on the changes.

### Breaking Changes

* `TargetingHttpContextMiddleware` now uses `Activity.Baggage` to store targeting information. Previously, it used `HttpContext.Items`. [#467](https://github.com/microsoft/FeatureManagement-Dotnet/pull/467)

## 3.5.0 - Jul 19, 2024

### Enhancements

* Developers using ASP.NET Core will now have a new extension method `WithTargeting()` which registers a default `ITargetingContextAccessor`. This default accessor will extract the targeting info from `HttpContext.User`. `UserId` will be taken from the Identity.Name field. `Groups` will be extracted from claims of type `Role`. [#466](https://github.com/microsoft/FeatureManagement-Dotnet/pull/466)

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