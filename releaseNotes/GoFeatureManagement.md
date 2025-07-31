# Microsoft Feature Management for Go

[Source code][source_code] | [Package][package] | [Samples][samples]

## v1.0.0 - August 01, 2025

This is the first stable release of Feature Management for Go, including support for the following capabilities. Note that, version `1.1.0-beta.1` or later of Go provider is required for loading feature flags from Azure App Configuration.

- Support for feature flag providers.
- Built-in feature flag provider for Azure App Configuration by the module `featuremanagement/providers/azappconfig`.
- Support for basic feature flags with boolean states.
- Support for feature filters, including custom filters and built-in filters [`Microsoft.TimeWindow`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.TimeWindow.v1.0.0.schema.json) and [`Microsoft.Targeting`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.Targeting.v1.0.0.schema.json).

## 1.0.0-beta.2 - July 18, 2025

### Bug Fixes

- Fixed a bug where unmarshaling `TargetingFilterParameters` used incorrect JSON tag, causing `RolloutPercentage` to always be 0 and preventing proper percentage-based rollouts.

## 1.0.0-beta.1 - July 01, 2025

Initial release of Feature Management for Go, including support for the following capabilities. Note that, version `1.1.0-beta.1` or later of Go provider is required for loading feature flags from Azure App Configuration.

- Support for feature flag providers.
- Built-in feature flag provider for Azure App Configuration by the module `featuremanagement/providers/azappconfig`.
- Support for basic feature flags with boolean states.
- Support for feature filters, including custom filters and built-in filters [`Microsoft.TimeWindow`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.TimeWindow.v1.0.0.schema.json) and [`Microsoft.Targeting`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.Targeting.v1.0.0.schema.json).

[source_code]: https://github.com/microsoft/FeatureManagement-Go
[package]: https://pkg.go.dev/github.com/microsoft/Featuremanagement-Go/featuremanagement
[samples]: https://github.com/microsoft/FeatureManagement-Go/tree/main/example