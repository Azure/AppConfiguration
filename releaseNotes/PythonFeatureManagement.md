# featuremanagement

[Source code][source_code] | [Samples][samples]

## 2.0.0b2 - October 14, 2024

### Enhancement

Added support for including `VariantAssignmentPercentage` and `DefaultWhenEnabled` properties to telemetry when telemetry is enabled and published to Azure Monitor.

## 2.0.0b1 - September 10, 2024

### Enhancement

* Added support for variant feature flags. A variant feature flag is an enhanced feature flag that supports multiple states or variations. While it can still be toggled on or off, it also allows for different configurations, ranging from simple primitives to complex JSON objects. Variant feature flags are particularly useful for feature rollouts, configuration rollouts, and feature experimentation (also known as A/B testing).

For more information, see the [feature reference document](https://learn.microsoft.com/azure/azure-app-configuration/feature-management-python-reference#variants).

* Added support for telemetry in feature flags. Telemetry is a powerful feature that allows you to track how your feature flags are being used. It provides insights into the effectiveness of your feature flags and helps you make data-driven decisions. Telemetry is particularly useful for feature experimentation (also known as A/B testing) and feature rollouts.

An example is available to demonstrate how to use the new Telemetry in Python See [the example](https://github.com/microsoft/FeatureManagement-Python/blob/main/samples/feature_variant_sample_with_telemetry.py) in the examples folder.

## 1.0.0 - July 01, 2024

GA Release of the feature management support for Python. Note that, version 1.2.0 or later of `azure-appconfiguration-provider` is required for loading feature flags from Azure App Configuration.

* Loading of feature flags from a dictionary.
* Support for basic feature flags with boolean states.
* Support for feature filters including built-in filters `TimeWindowFilter` and `TargetingFilter`.

## 1.0.0b1 - May 24, 2024

### Enhancement

Initial release of the feature management support for Python. Note that, version 1.2.0 or later of `azure-appconfiguration-provider` is required for loading feature flags from Azure App Configuration.

* Loading of feature flags from a dictionary.
* Support for basic feature flags with boolean states.
* Support for feature filters including built-in filters `TimeWindowFilter` and `TargetingFilter`.

[samples]: https://github.com/microsoft/FeatureManagement-Python/tree/main/samples
[source_code]: https://github.com/microsoft/FeatureManagement-Python
