# Microsoft.FeatureManagement.Telemetry.ApplicationInsights

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

* Fixed a bug that caused duplicated dimension in Application Insights telemetry. [#542](https://github.com/microsoft/FeatureManagement-Dotnet/pull/542)

## 4.0.0 - November 1, 2024

### Enhancements

* Added support for Application Insights telemetry. To publish feature flag evaluation data and tag outgoing events with targeting information, register the Application Insights telemetry publisher as shown below.

    ```csharp
    builder.Services.AddFeatureManagement()
        .AddApplicationInsightsTelemetry();
    ```

    For more details on Application Insights Publishing, see [here](https://learn.microsoft.com/en-us/azure/azure-app-configuration/feature-management-dotnet-reference#application-insights-telemetry-publisher)

## 4.0.0-preview5 - Oct 24, 2024

* Updated `Microsoft.FeatureManagement` reference to `4.0.0-preview5`. See the [release notes](./Microsoft.Featuremanagement.md) for more information on the changes.

## 4.0.0-preview4 - Jul 19, 2024

* Updated `Microsoft.FeatureManagement` reference to `4.0.0-preview4`. See the [release notes](./Microsoft.Featuremanagement.md) for more information on the changes.

### Enhancements

* Introduced a new API `AddApplicationInsightsTelemetryPublisher` to register a feature flag telemetry publisher for Application Insights. [#455](https://github.com/microsoft/FeatureManagement-Dotnet/pull/455)

  ``` C#
  builder.Services.AddFeatureManagement()
                  .WithTargeting()
                  .AddApplicationInsightsTelemetryPublisher();
  ```

## Breaking Changes

* The `TargetingTelemetryInitializer` type has been moved to this package from the now-deprecated `Microsoft.FeatureManagement.Telemetry.ApplicationInsights.AspNetCore` package. This change simplifies the utilization of feature flag telemetry.

* The type `ApplicationInsightsTelemetryPublisher` has been removed as its functionality has been replaced with the new API `AddApplicationInsightsTelemetryPublisher` for publishing feature flag telemetry to Application Insights. [#455](https://github.com/microsoft/FeatureManagement-Dotnet/pull/455)

## 4.0.0-preview3 - April 10, 2024

* Updated `Microsoft.FeatureManagement` reference to `4.0.0-preview3`. See the [release notes](./Microsoft.Featuremanagement.md) for more information on the changes.

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
