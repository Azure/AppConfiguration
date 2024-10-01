# Microsoft.FeatureManagement.Telemetry.ApplicationInsights.AspNetCore

[Source code][source_code] | [Package (NuGet)][package] | [Samples][samples] | [Product documentation][docs]

## 4.0.0 - September 24, 2024

### Enhancements

#### Application Insights

### Deprecated

This package has been deprecated and it's functionality is now available as a part of the [Microsoft.FeatureManagement.ApplicationInsights](https://github.com/Azure/AppConfiguration/blob/main/releaseNotes/Microsoft.Featuremanagement.Telemetry.ApplicationInsights.md) package.

## 4.0.0-preview3 - April 10, 2024

* Updated `Microsoft.FeatureManagement` reference to `4.0.0-preview3`. See the [release notes](./Microsoft.Featuremanagement.md) for more information on the changes.

## 4.0.0-preview2 - March 7, 2024

### Enhancements

* Introduced a telemetry initializer named `TargetingTelemetryInitializer`. It automatically adds targeting information to telemetry data your application sends to Application Insights. This can be used to correlate your telemetry data with feature evaluation events based on the targeting information during your telemetry analysis. ([#409](https://github.com/microsoft/FeatureManagement-Dotnet/issues/409))

<!-- LINKS -->
[docs]: https://github.com/microsoft/FeatureManagement-Dotnet
[package]: https://www.nuget.org/packages/Microsoft.FeatureManagement.Telemetry.ApplicationInsights.AspNetCore
[samples]: https://github.com/microsoft/FeatureManagement-Dotnet/tree/preview/examples/EvaluationDataToApplicationInsights
[source_code]: https://github.com/microsoft/FeatureManagement-Dotnet/tree/preview/src/Microsoft.FeatureManagement.Telemetry.ApplicationInsights.AspNetCore