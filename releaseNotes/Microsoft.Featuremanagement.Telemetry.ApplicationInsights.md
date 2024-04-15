# Microsoft.FeatureManagement.Telemetry.ApplicationInsights

[Source code][source_code] | [Package (NuGet)][package] | [Samples][samples] | [Product documentation][docs]

## 4.0.0-preview3 - April 10, 2024

### Breaking Changes

* Updated the namespace for `ApplicationInsightsTelemetryPublisher` to `Microsoft.FeatureManagement.Telemetry`. In the future, developers using any of our offered telemetry publishers will no longer need to specify the service specific namespaces like `using Microsoft.FeatureManagement.Telemetry.ApplicationInsights`.

* Updated the namespace for `TrackEvent` extension method of `TelemetryClient` to `Microsoft.ApplicationInsights`. The previous directive `using Microsoft.FeatureManagement.Telemetry.ApplicationInsights` is no longer required when calling the `TrackEvent` method.

## 4.0.0-preview2 - March 7, 2024

### Enhancements

* Added a `TargetingId` property to the feature evaluation events sent to Application Insights. The `TargetingId` is the identifier of a targeted user during feature evaluation. This new property allows you to correlate feature evaluation events with other telemetry data your application sends to Application Insights, as long as they share the same `TargetingId`. ([#409](https://github.com/microsoft/FeatureManagement-Dotnet/issues/409))

<!-- LINKS -->
[docs]: https://github.com/microsoft/FeatureManagement-Dotnet
[package]: https://www.nuget.org/packages/Microsoft.FeatureManagement.Telemetry.ApplicationInsights
[samples]: https://github.com/microsoft/FeatureManagement-Dotnet/tree/preview/examples/EvaluationDataToApplicationInsights
[source_code]: https://github.com/microsoft/FeatureManagement-Dotnet/tree/preview/src/Microsoft.FeatureManagement.Telemetry.ApplicationInsights
