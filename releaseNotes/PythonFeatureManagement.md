# featuremanagement

[Source code][source_code] | [Samples][samples]

## 2.0.0b1 - September 10, 2024

### Enhancement

* Added support for variant feature flags defined using [Microsoft Feature Management schema](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureManagement.v2.0.0.schema.json). Variants and telemetry can be declared using [Microsoft Feature Flag schema v2](https://github.com/microsoft/FeatureManagement/blob/main/Schema/FeatureFlag.v2.0.0.schema.json). The Microsoft Feature Management schema is designed to be language agnostic, which enables you to apply a consistent feature management configuration across Microsoft feature management libraries of different programming languages.

### Variants

Variants are a tool that can be used to surface different variations of a feature to different segments of an audience. Previously, this library only worked with flags. The flags were limited to boolean values, as they are either enabled or disabled. Variants have dynamic values. They can be string, int, a complex object, or a reference to a ConfigurationSection.

```python
# Modify view based off multiple possible variants
variant = feature_manager.is_enabled("BackgroundUrl", TargetingContext(user_id="Adam"))

context["model"] = variant.configuration
```

Variants are defined within a Feature, under a new section named "Variants". Variants are assigned by allocation, defined in a new section named "Allocation".

```javascript
"BackgroundUrl": {
    "Variants": [
        {
            "Name": "GetStarted",
            "ConfigurationValue": "https://learn.microsoft.com/en-us/media/illustrations/biztalk-get-started-get-started.svg"
        },
        {
            "Name": "InstallConfigure",
            "ConfigurationValue": "https://learn.microsoft.com/en-us/media/illustrations/biztalk-host-integration-install-configure.svg"
        }
    ],
    "Allocation": { 
        // Defines Users, Groups, or Percentiles for variant assignment
    }
    // Filters and other Feature fields
}
```

### Telemetry

The feature management library now offers the ability to emit events containing information about a feature evaluation. This can be used to ensure a flag is running as expected, or to see which users were given which features and why they were given the feature. To enable this functionality, two things need to be done:

The flag needs to explicitly enable telemetry in its definition.

```json
"MyFlag": {
    "Telemetry": {
        "Enabled": true
    }
}
```

And a telemetry publisher needs to be registered. Custom publishers can be defined, but for Application Insights one is already available in the `featuremanagement.azuremonitor` moudle.

```python
from featuremanagement.azuremonitor import publish_telemetry

feature_manager = FeatureManager(azure_app_config, on_feature_evaluated=custom_publish)
```

An example is available to demonstrate how to use the new Telemetry in Python See [the example](https://github.com/microsoft/FeatureManagement-Python/blob/main/samples/feature_variant_sample_with_telemetry.py) in the examples folder.

## 1.0.0 - July 01, 2024

GA Release of the feature management support for Python.

## 1.0.0b1 - May 24, 2024

### Enhancement

Initial release of the feature management support for Python. Note that, version 1.2.0 or later of `azure-appconfiguration-provider` is required for loading feature flags from Azure App Configuration.

* Loading of feature flags from a dictionary.
* Support for basic feature flags with boolean states.
* Support for feature filters including built-in filters `TimeWindowFilter` and `TargetingFilter`.

[samples]: https://github.com/microsoft/FeatureManagement-Python/tree/main/samples
[source_code]: https://github.com/microsoft/FeatureManagement-Python
