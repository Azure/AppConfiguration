# Microsoft Feature Management for Go

[Source code][source_code] | [Package][package] | [Samples][samples]

## v1.1.1 - October 17, 2025

### Enhancement

* Upgraded Go version requirement to `1.24.0`. [#35](https://github.com/microsoft/FeatureManagement-Go/pull/35)

## v1.1.0 - August 20, 2025

### Enhancement

* Added support for variant feature flags. A variant feature flag is an enhanced feature flag that supports multiple states or variations. While it can still be toggled on or off, it also allows for different configurations, ranging from simple primitives to complex JSON objects. Variant feature flags are particularly useful for feature rollouts, configuration rollouts, and feature experimentation (also known as A/B testing). [#16](https://github.com/microsoft/FeatureManagement-Go/pull/16)

## v1.0.0 - August 01, 2025

This is the first stable release of Feature Management for Go, including support for the following capabilities. Note that, version `1.1.0-beta.1` or later of [`azureappconfiguration`](https://pkg.go.dev/github.com/Azure/AppConfiguration-GoProvider/azureappconfiguration) is required for loading feature flags from Azure App Configuration.


### Supported features
- Built-in functionality for consuming feature flags defined in Azure App Configuration, as well as an extensibility point via the `FeatureFlagProvider` interface to consume feature flags defined by other providers.
- Basic feature flags, as well as feature filters, including:
    - [`Microsoft.TimeWindow`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.TimeWindow.v1.0.0.schema.json)
    - [`Microsoft.Targeting`](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFilters/Microsoft.Targeting.v1.0.0.schema.json)
    - Custom filters

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