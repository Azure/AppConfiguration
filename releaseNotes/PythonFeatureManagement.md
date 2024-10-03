# featuremanagement

[Source code][source_code] | [Samples][samples]

## 2.0.0b1 - September 10, 2024

### Enhancement

* Added support for variant feature flags defined using [Microsoft Feature Management schema](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureManagement.v2.0.0.schema.json). Variants and telemetry can be declared using [Microsoft Feature Flag schema v2](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFlag.v2.0.0.schema.json). The Microsoft Feature Management schema is designed to be language agnostic, which enables you to apply a consistent feature management configuration across Microsoft feature management libraries of different programming languages.

See our [documentation](https://learn.microsoft.com/en-us/azure/azure-app-configuration/feature-management-python-reference?pivots=preview-version#variants) for more information.

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
