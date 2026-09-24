# Microsoft.FeatureManagement.Telemetry.OpenTelemetry

[Source code][source_code] | [Package (NuGet)][package] | [Samples][samples] | [Product documentation][docs]

## 4.8.0 - September 24, 2026

### Enhancements

* Introduced the `Microsoft.FeatureManagement.Telemetry.OpenTelemetry` package to publish feature flag evaluation events to OpenTelemetry-compatible backends and enrich spans and logs with `TargetingId` for correlation with user telemetry. Call `AddFeatureManagementProcessors()` on the OpenTelemetry builder before configuring exporters, such as `UseAzureMonitor()`, and enable telemetry on the feature flags to publish their evaluations. [microsoft/FeatureManagement-Dotnet#617](https://github.com/microsoft/FeatureManagement-Dotnet/pull/617)

    ```csharp
    using Microsoft.FeatureManagement;
    using OpenTelemetry;

    builder.Services.AddFeatureManagement();

    builder.Services.AddOpenTelemetry()
        .AddFeatureManagementProcessors();
    ```

    For an example that includes targeting and Azure Monitor exporter configuration, see the [OpenTelemetry sample][samples].

<!-- LINKS -->
[docs]: https://github.com/microsoft/FeatureManagement-Dotnet
[package]: https://www.nuget.org/packages/Microsoft.FeatureManagement.Telemetry.OpenTelemetry
[samples]: https://github.com/microsoft/FeatureManagement-Dotnet/tree/main/examples/OpenTelemetryDemo
[source_code]: https://github.com/microsoft/FeatureManagement-Dotnet/tree/main/src/Microsoft.FeatureManagement.Telemetry.OpenTelemetry
