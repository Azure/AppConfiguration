# Microsoft.FeatureManagement.Telemetry.ApplicationInsights

[Source code][source_code] | [Package (NuGet)][package] | [Samples][samples] | [Product documentation][docs]

## 4.0.0 - September 24, 2024

### Enhancements

#### Application Insights

```csharp
builder.Services.AddFeatureManagement()
    .AddApplicationInsightsTelemetry();
```

To export telemetry to Application Insights as custom events, we offer a new builder method `IFeatureManagementBuilder.AddApplicationInsightsTelemetry()`. This causes outgoing Application Insights telemetry to be tagged with TargetingId and will emit evaluation events to the custom events table- using the `TelemetryClient` if available in DI.

For more details on Application Insights Publishing, see [here](https://learn.microsoft.com/en-us/azure/azure-app-configuration/feature-management-dotnet-reference#application-insights-telemetry-publisher)

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
