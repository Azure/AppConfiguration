# Microsoft Feature Management for JavaScript

[Source code][source_code] | [Package (npm)][package] | [Samples][samples]

## 2.0.0-preview.2 - Oct 24, 2024

* No changes in this release.

## 2.0.0-preview.1 - Oct 15, 2024

### Enhancements

* Added support for variant feature flags. A variant feature flag is an enhanced feature flag that supports multiple states or variations. While it can still be toggled on or off, it also allows for different configurations, ranging from simple primitives to complex JSON objects. Variant feature flags are particularly useful for feature rollouts, configuration rollouts, and feature experimentation (also known as A/B testing).

* Added support for telemetry in feature flags. Telemetry is a powerful feature that allows you to track how your feature flags are being used. It provides insights into the effectiveness of your feature flags and helps you make data-driven decisions. Telemetry is particularly useful for feature experimentation (also known as A/B testing) and feature rollouts.

## 1.0.0 - Sep 26, 2024

This is the first stable release of the following features.
- Loading of feature flags from object and map.
- Feature flag evaluation with ambient context or user-provided context.
- Built-in feature filters [`Microsoft.TimeWindow`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.TimeWindow.v1.0.0.schema.json) and [`Microsoft.Targeting`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.Targeting.v1.0.0.schema.json).

## 1.0.0-preview.1 - May 17, 2024

Added support for the following features.
- Loading of feature flags from object and map.
- Feature flag status evaluation with or without context.
- Feature filters including built-in filters [`Microsoft.TimeWindow`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.TimeWindow.v1.0.0.schema.json) and [`Microsoft.Targeting`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.Targeting.v1.0.0.schema.json).

[package]: https://www.npmjs.com/package/@microsoft/feature-management
[samples]: https://github.com/microsoft/FeatureManagement-JavaScript/tree/main/examples
[source_code]: https://github.com/microsoft/FeatureManagement-JavaScript
