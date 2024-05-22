# featuremanagement

[Source code][source_code] | [Samples][samples]

## 1.0.0b1 - May 22, 2024

Initial Beta Release of the Feature Management for Python

### New Features

* Loading of feature flags from a file, see [feature management schema].
* Loading of feature flags from Azure App Configuration provider.
* Checking if a feature is enabled.
* Default feature filters: TimeWindowFilter, TargetingFilter.
* Custom feature filters.

[samples]: https://github.com/microsoft/FeatureManagement-Python/tree/main/samples
[source_code]: https://github.com/microsoft/FeatureManagement-Python
[feature management schema]:https://github.com/Azure/AppConfiguration/blob/main/docs/FeatureManagement/FeatureManagement.v2.0.0.schema.json