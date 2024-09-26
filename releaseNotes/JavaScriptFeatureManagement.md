# Microsoft Feature Management for JavaScript

[Source code][source_code] | [Package (npm)][package] | [Samples][samples]

## 1.0.0 - Sep 26, 2024

This is the first stable release of the following features.
- Loading of feature flags from object and map.
- Feature flag status evaluation with or without context.
- Feature filters including built-in filters [`Microsoft.TimeWindow`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.TimeWindow.v1.0.0.schema.json) and [`Microsoft.Targeting`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.Targeting.v1.0.0.schema.json).

### Breaking Changes

* The feature flag provider handles duplicate flags by using the last flag instead of the first. [#40](https://github.com/microsoft/FeatureManagement-JavaScript/pull/40)

### Enhancements

* The targeting filter can work properly in non-Node.js environments. [#25](https://github.com/microsoft/FeatureManagement-JavaScript/pull/25)

## 1.0.0-preview.1 - May 17, 2024

Added support for the following features.
- Loading of feature flags from object and map.
- Feature flag status evaluation with or without context.
- Feature filters including built-in filters [`Microsoft.TimeWindow`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.TimeWindow.v1.0.0.schema.json) and [`Microsoft.Targeting`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.Targeting.v1.0.0.schema.json).

[package]: https://www.npmjs.com/package/@microsoft/feature-management
[samples]: https://github.com/microsoft/FeatureManagement-JavaScript/tree/main/examples
[source_code]: https://github.com/microsoft/FeatureManagement-JavaScript
