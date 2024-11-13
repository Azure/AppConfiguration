# Microsoft JavaScript Feature Management for Application Insights in Node.js

[Source code][source_code] | [Package (npm)][package] | [Samples][samples]

## 2.0.0-preview.3 - Nov 8, 2024

### Enhancements

* Added support for publishing telemetry to Application Insights.

* Introduced a `TrackEvent` API, allowing users to replace existing Application Insights `TrackEvent` calls to include targeting information in custom events sent to Application Insights. [#64](https://github.com/microsoft/FeatureManagement-JavaScript/pull/64)

[package]: https://www.npmjs.com/package/@microsoft/feature-management-applicationinsights-node
[samples]: https://github.com/microsoft/FeatureManagement-JavaScript/tree/main/examples
[source_code]: https://github.com/microsoft/FeatureManagement-JavaScript