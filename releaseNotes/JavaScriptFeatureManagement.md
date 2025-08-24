# Microsoft Feature Management for JavaScript

[Source code][source_code] | [Package (npm)][package] | [Samples][samples]

## 2.2.0 - Augest 21, 2025

### Enhancements

* Added a `Recurrence` option to the `TimeWindowFilter` to support recurring time window. This enables scenarios where a feature flag is activated based on a recurrence pattern, such as every day after 5 PM or every Friday. See the [how to Time Window Filter document](https://learn.microsoft.com/azure/azure-app-configuration/howto-timewindow-filter) for more details.

## 2.1.0 - May 20, 2025

### Enhancements

* Stablized the targeting context accessor. You can configure an `ITargetingContextAccessor` when initializing the `FeatureManager` through `FeatureManagerOptions.targetingContextAccessor`. [#93](https://github.com/microsoft/FeatureManagement-JavaScript/pull/93)

* Stablized the feature evaluation event properties which include:  `"Version"`, `"FeatureName"`, `"Enabled"`, `"TargetingId"`,  `"Variant`", `"VariantAssignmentReason"`, `"DefaultWhenEnabled"` and `"VariantAssignmentPercentage"`.

## 2.1.0-preview.1 - April 22, 2025

### Enhancements

* Introduced `ITargetingContextAccessor` to allow `FeatureManager` to get ambient targeting context in the server scenario. You can configure a targeting context accessor when initializing the `FeatureManager` through `FeatureManagerOptions.targetingContextAccessor`. [#93](https://github.com/microsoft/FeatureManagement-JavaScript/pull/93) 

## 2.0.0 - Jan 14, 2025

### Enhancements

This is the first stable release of the following features.

* __Variant Feature Flags__

  A variant feature flag is an enhanced feature flag that supports multiple states or variations. While it can still be toggled on or off, it also allows for different configurations, ranging from simple primitives to complex JSON objects. Variant feature flags are particularly useful for feature rollouts, configuration rollouts, and feature experimentation (also known as A/B testing).

  The new `getVariant` method of `FeatureManager` has been introduced to evaluate the assigned variant based on the variant feature flag configuration and targeting context. [#13](https://github.com/microsoft/FeatureManagement-JavaScript/pull/13)

* __Feature Evaluation Telemetry__

  Telemetry provides observability into flag evaluations, offering insights into which users received specific flag results. This enables more powerful metric analysis, such as experimentation.
  
  The new `onFeatureEvaluated` property of `FeatureManagerOptions` allows you to set a custom callback for all feature evaluations. This hook can be used to publish telemetry. You can call `createTelemetryPublisher` API from `@microsoft/feature-management-applicationinsights-browser` and `@microsoft/feature-management-applicationinsights-node` packages to publish feature evaluation event to Application Insights. [#36](https://github.com/microsoft/FeatureManagement-JavaScript/pull/36)

### Bug Fixes

* Fixed a bug that caused error when calling `getFeatureFlags` and `listFeatureNames`. [#74](https://github.com/microsoft/FeatureManagement-JavaScript/issues/74)

## 2.0.0-preview.3 - Oct 24, 2024

### Enhancements

* Added validation for feature flag properties. [#17](https://github.com/microsoft/FeatureManagement-JavaScript/pull/17)

## 2.0.0-preview.2 - Oct 24, 2024

* No changes in this release.

## 2.0.0-preview.1 - Oct 15, 2024

### Enhancements

* Added support for variant feature flags. A variant feature flag is an enhanced feature flag that supports multiple states or variations. While it can still be toggled on or off, it also allows for different configurations, ranging from simple primitives to complex JSON objects. Variant feature flags are particularly useful for feature rollouts, configuration rollouts, and feature experimentation (also known as A/B testing). [#13](https://github.com/microsoft/FeatureManagement-JavaScript/pull/13)

* Added support for telemetry in feature flags. Telemetry is a powerful feature that allows you to track how your feature flags are being used. It provides insights into the effectiveness of your feature flags and helps you make data-driven decisions. Telemetry is particularly useful for feature experimentation (also known as A/B testing) and feature rollouts. [#36](https://github.com/microsoft/FeatureManagement-JavaScript/pull/36)

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
