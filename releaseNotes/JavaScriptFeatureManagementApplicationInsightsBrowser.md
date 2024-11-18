# Microsoft JavaScript Feature Management for Application Insights in Browsers

[Source code][source_code] | [Package (npm)][package] | [Samples][samples]

## 2.0.0-preview.3 - Nov 8, 2024

* Updated `@microsoft/feature-management` dependency to `2.0.0-preview.3`. See the [release notes](./JavaScriptFeatureManagement.md) for more information on the changes.

## 2.0.0-preview.2 - Oct 24, 2024

### Bug Fix

* Fixed a bug that caused `TargetingId` in telemetry to be "undefined". [#59](https://github.com/microsoft/FeatureManagement-JavaScript/pull/59)

## 2.0.0-preview.1 - Oct 15, 2024

### Enhancements

* Added support for publishing telemetry to Application Insights.

* Introduced a `TrackEvent` API, allowing users to replace existing Application Insights `TrackEvent` calls to include targeting information in custom events sent to Application Insights. [#39](https://github.com/microsoft/FeatureManagement-JavaScript/pull/39)

[package]: https://www.npmjs.com/package/@microsoft/feature-management-applicationinsights-browser
[samples]: https://github.com/microsoft/FeatureManagement-JavaScript/tree/main/examples
[source_code]: https://github.com/microsoft/FeatureManagement-JavaScript
