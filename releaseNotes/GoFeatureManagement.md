# Microsoft Feature Management for Go

[Source code][source_code] | [Package][package] | [Samples][samples]

## 1.0.0-beta.1 - July 01, 2025

Initial release of Feature Management for Go, including support for the following capabilities. Note that, version `1.1.0-beta.1` or later of Go provider is required for loading feature flags from Azure App Configuration.

- Introduced a separate module `featuremanagement/providers/azappconfig` to load feature flags from Azure App Configuration. External dependencies are decoupled from the core `featuremanagement` module and can be installed independently as needed.
- Support for basic feature flags with boolean states.
- Support feature filters, including custom filters and built-in filters [`Microsoft.TimeWindow`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.TimeWindow.v1.0.0.schema.json) and [`Microsoft.Targeting`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.Targeting.v1.0.0.schema.json).

[source_code]: https://github.com/microsoft/FeatureManagement-Go
[package]: https://pkg.go.dev/github.com/microsoft/Featuremanagement-Go/featuremanagement
[samples]: https://github.com/microsoft/FeatureManagement-Go/tree/main/example