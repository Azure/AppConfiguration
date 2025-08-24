# Microsoft JavaScript Feature Management for Application Insights in Node.js

[Source code][source_code] | [Package (npm)][package] | [Samples][samples]

## 2.2.0 - Augest 21, 2025

### Enhancements

* Updated `@microsoft/feature-management` dependency to `2.2.0`.

## 2.1.0 - May 20, 2025

### Enhancements

* Updated `@microsoft/feature-management` dependency to `2.1.0`.

* Stablized `createTargetingTelemetryProcessor` function to enable targeting id to be attached to Application Insights telemetry. [#98](https://github.com/microsoft/FeatureManagement-JavaScript/pull/98) 


## 2.1.0-preview.1 - April 22, 2025

* Updated `@microsoft/feature-management` dependency to `2.1.0-preview.1`. See the [release notes](./JavaScriptFeatureManagement.md) for more information on the changes.

### Enhancements

* Added `createTargetingTelemetryProcessor` function to enable targeting id to be attached to Application Insights telemetry. [#98](https://github.com/microsoft/FeatureManagement-JavaScript/pull/98)

## 2.0.0 - Jan 14, 2025

### Enhancements

This is the first stable release of the following features.

* A `createTelemetryPublisher` function that has been added that enables feature evaluation telemetry to be published to Application Insights.

* A `TrackEvent` function has been added to enable sending custom events embedded with targeting information to Application Insights. This function is meant to be used in place of the Application Insights package's standard `TrackEvent` call.

## 2.0.0-preview.3 - Nov 8, 2024

### Enhancements

* Added support for publishing telemetry to Application Insights.

* Introduced a `TrackEvent` API, allowing users to replace existing Application Insights `TrackEvent` calls to include targeting information in custom events sent to Application Insights. [#64](https://github.com/microsoft/FeatureManagement-JavaScript/pull/64)

[package]: https://www.npmjs.com/package/@microsoft/feature-management-applicationinsights-node
[samples]: https://github.com/microsoft/FeatureManagement-JavaScript/tree/main/examples
[source_code]: https://github.com/microsoft/FeatureManagement-JavaScript