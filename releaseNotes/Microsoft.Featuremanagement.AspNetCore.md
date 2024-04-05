# Microsoft.FeatureManagement.AspNetCore

[Source code][source_code] | [Package (NuGet)][package] | [Samples][samples] | [Product documentation][docs]

## 4.0.0-preview2 - March 7, 2024

### Enhancements

* Added `TargetingHttpContextMiddleware`, which ensures TargetingContext is available for syncronous functions, like `TargetingTelemetryInitializer` in the `Microsoft.FeatureManagement.Telemetry.ApplicationInsights.AspNetCore` package. ([#409](https://github.com/microsoft/FeatureManagement-Dotnet/issues/409))
* Added support for .NET 8 target framework. ([#364](https://github.com/microsoft/FeatureManagement-Dotnet/issues/364))

### Breaking Changes

No breaking changes in this release.

<!-- LINKS -->
[package]: https://www.nuget.org/packages/Microsoft.FeatureManagement.AspNetCore
[samples]: https://github.com/microsoft/FeatureManagement-Dotnet/tree/master/examples/FeatureFlagDemo
[source_code]: https://github.com/microsoft/FeatureManagement-Dotnet/tree/master/src/Microsoft.FeatureManagement.AspNetCore