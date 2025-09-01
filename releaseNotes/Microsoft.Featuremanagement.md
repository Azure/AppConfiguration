# Microsoft.FeatureManagement

[Source code][source_code] | [Package (NuGet)][package] | [Samples][samples] | [Product documentation][docs]

## 4.3.0 - August 27, 2025

### Enhancements

* Introduced `ConfigurationFeatureDefinitionProviderOptions` which allows to enable the new configuration merging behavior for the built-in `ConfiguationFeatureDefinitionProvider`. When the same feature flag is defined in multiple sources, definitions are merged according to configuration provider registration order, with the last feature flag definition taking precedence in case of conflicts. [#552](https://github.com/microsoft/FeatureManagement-Dotnet/pull/552)

## 4.2.1 - July 9, 2025 (Delisted)

This release has been delisted due to the enhancement below resulting in an unintended breaking change. For more information, please go to [#550](https://github.com/microsoft/FeatureManagement-Dotnet/issues/550).

### Enhancements

* Added support for aggregating feature flags from multiple configuration providers. When the same feature flag is defined in multiple sources, definitions are merged according to configuration provider registration order, with the last feature flag definition taking precedence in case of conflicts. [#536](https://github.com/microsoft/FeatureManagement-Dotnet/pull/536)

## 4.1.0 - May 22, 2025

### Enhancements

* Added `DefaultWhenEnabled` and `VariantAssignmentPercentage` fields to the `FeatureEvaluation` event. [#495](https://github.com/microsoft/FeatureManagement-Dotnet/pull/495)

## 4.0.0 - November 1, 2024

### Enhancements

#### Variant Feature Flags

A variant feature flag is an enhanced feature flag that supports multiple states or variations. While it can still be toggled on or off, it also allows for different configurations, ranging from simple primitives to complex JSON objects. Variant feature flags are particularly useful for feature rollouts, configuration rollouts, and feature experimentation (also known as A/B testing).

The new `IVariantFeatureManager` has been introduced as the successor to the existing `IFeatureManager`. It retains all the functionalities of `IFeatureManager` while adding new `GetVariantAsync` methods and supporting `CancellationToken` for all methods.

``` C#
IVariantFeatureManager featureManager;
...
Variant variant = await featureManager.GetVariantAsync(MyFeatureFlags.HelpText, CancellationToken.None);
model.Text = variant.Configuration.Value;
```

*Note: If reading variant flags from App Configuration, version `8.0.0` or above for the `Microsoft.Extensions.Configuration.AzureAppConfiguration` or `Microsoft.Azure.AppConfiguration.AspNetCore` package is required.*

For more details on Variants, see [here](https://learn.microsoft.com/en-us/azure/azure-app-configuration/feature-management-dotnet-reference#variants).

#### Variant Service Provider

Variant feature flags can be used in conjunction with dependency injection to surface different implementations of a service for different users. This is accomplished by using the Variant Service Provider.

For more details on Variant Service Provider, see [here](https://learn.microsoft.com/en-us/azure/azure-app-configuration/feature-management-dotnet-reference#variants-in-dependency-injection)

#### Telemetry

Telemetry provides observability into flag evaluations, offering insights into which users received specific flag results. This enables more powerful metric analysis, such as experimentation.

For more details on Telemetry, see [here](https://learn.microsoft.com/en-us/azure/azure-app-configuration/feature-management-dotnet-reference#telemetry).

#### Microsoft Feature Management Schema

Added support for variant feature flags defined using [Microsoft Feature Management schema](https://github.com/microsoft/FeatureManagement/blob/c5fab16dbf1450dce0bbfe7c4207da735ff31916/Schema/FeatureManagement.v2.0.0.schema.json). Variants and telemetry can be declared using [Microsoft Feature Flag schema v2](https://github.com/microsoft/FeatureManagement/blob/c5fab16dbf1450dce0bbfe7c4207da735ff31916/Schema/FeatureFlag.v2.0.0.schema.json). Here is a [Sample](https://github.com/microsoft/FeatureManagement-Dotnet/blob/f47e188babea0a91488d2e6a0b2ab4c9405d0794/examples/VariantAndTelemetryDemo/appsettings.json#L12).

#### Performance Optimizations

The performance of the feature flag state evaluation has been improved by up to 20%, with a memory reduction of up to 30% for .NET 8 applications compared to the version 3.5.0 release.

### Other Changes

* `DefaultWhenEnabled` and `VariantAssignmentPercentage` were removed from the feature evaluation event. These were introduced in 4.0.0-preview5 version, but have been removed in this stable version.

## 4.0.0-preview5 - Oct 24, 2024

### Enhancements

* Added support for injecting additional telemetry fields on feature evaluation events if telemetry is enabled.
  * `DefaultWhenEnabled` reflects what the DefaultWhenEnabled variant on the flag is.
  * `VariantAssignmentPercentage` shows what percentage of users will be allocated the given Variant for the given Reason.

## 4.0.0-preview4 - Jul 19, 2024

### Enhancements

* The feature flag telemetry pipeline is now integrated with .NET `Acitivity` instrumentation. Feature manager now has an `AcitvitySource` called "Microsoft.FeatureManagement". If telemetry is enabled for a feature flag, whenever the feature flag is evaluated, feature manager will start an `Activity` and add an `ActivityEvent` with tags containing feature evaluation information.  [#455](https://github.com/microsoft/FeatureManagement-Dotnet/pull/455)

### Breaking Changes

* If you were using earlier preview versions of this package and configuration files to define variant feature flags, they are no longer supported in the [.NET Feature Management schema](https://github.com/microsoft/FeatureManagement-Dotnet/blob/main/schemas/FeatureManagement.Dotnet.v1.0.0.schema.json). Instead, please use the [Microsoft Feature Management schema](https://github.com/Azure/AppConfiguration/blob/main/docs/FeatureManagement/FeatureManagement.v2.0.0.schema.json) to define variant feature flags. [#421](https://github.com/microsoft/FeatureManagement-Dotnet/pull/421).

* `AddTelemetryPublisher` API and `ITelemetryPublisher` interface were removed. The feature flag telemetry pipeline is now integrated with .NET `Acitivity` instrumentation. [#455](https://github.com/microsoft/FeatureManagement-Dotnet/pull/455)

## 3.5.0 - Jul 19, 2024

* No changes in this release.

## 3.4.0 - Jun 21, 2024

### Enhancements

* All public classes no longer use init-only setters, ensuring compatibility with application using C# 7 or earlier. [#450](https://github.com/microsoft/FeatureManagement-Dotnet/pull/450)

## 3.3.1 - May 23, 2024

### Bug Fixes

* Fixed a bug that caused the time window filter to be unusable if `AddFeatureFilter<TimeWindowFilter>` was called.`. [#447](https://github.com/microsoft/FeatureManagement-Dotnet/issues/447)

## 3.3.0 - May 8, 2024

### Enhancements

* Added a `Recurrence` option to the `TimeWindow` filter to support recurring time window. This enables scenarios where a feature flag is activated based on a recurrence pattern, such as every day after 5 PM or every Friday. See more details [here](https://github.com/microsoft/FeatureManagement-Dotnet?tab=readme-ov-file#recurrence-pattern). ([#266](https://github.com/microsoft/FeatureManagement-Dotnet/pull/266))
* A `LoggerFactory` is no longer required when constructing built-in filters. ([#386](https://github.com/microsoft/FeatureManagement-Dotnet/pull/386))

### Bug Fixes

* Fixed a possible null-reference exception when enumerating `GetFeatureNamesAsync`. ([#438](https://github.com/microsoft/FeatureManagement-Dotnet/pull/438))

## 4.0.0-preview3 - April 10, 2024

### Enhancements

* Added support for variant feature flags defined using [Microsoft Feature Management schema](https://github.com/Azure/AppConfiguration/blob/main/docs/FeatureManagement/FeatureManagement.v1.0.0.schema.json). Variants and telemetry can be declared using [Microsoft Feature Flag schema v2](https://github.com/Azure/AppConfiguration/blob/main/docs/FeatureManagement/FeatureFlag.v2.0.0.schema.json). The Microsoft Feature Management schema is designed to be language agnostic, which enables you to apply a consistent feature management configuration across Microsoft feature management libraries of different programming languages.

## 4.0.0-preview2 - March 7, 2024

### Enhancements

* Added support for variant feature flag-based service provider in dependency injection. It allows different service implementations to be injected automatically for different targeted audiences based on their variant assignment. ([#39](https://github.com/microsoft/FeatureManagement-Dotnet/issues/39)). See more details [here](https://github.com/microsoft/FeatureManagement-Dotnet/tree/preview?tab=readme-ov-file#variants-in-dependency-injection).
* Added a `TargetingContext` property to the `EvaluationEvent`. This allows feature evaluation events to accurately represent what the targeting context was at the time of feature evaluation. ([#409](https://github.com/microsoft/FeatureManagement-Dotnet/issues/409))

## 3.2.0 - February 29, 2024

### Enhancements

* Added support for feature flags defined using [Microsoft Feature Management schema](https://github.com/Azure/AppConfiguration/blob/main/docs/FeatureManagement/FeatureManagement.v1.0.0.schema.json)

## 4.0.0-preview - January 4, 2024

### Variants

Variants are a tool that can be used to surface different variations of a feature to different segments of an audience. Previously, this library only worked with flags. The flags were limited to boolean values, as they are either enabled or disabled. Variants have dynamic values. They can be string, int, a complex object, or a reference to a ConfigurationSection.

```csharp
//
// Modify view based off multiple possible variants
Variant variant = await featureManager.GetVariantAsync(MyFeatureFlags.BackgroundUrl);

model.BackgroundUrl = variant.Configuration.Value;

return View(model);
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

For more details on Variants, see [here](https://github.com/microsoft/FeatureManagement-Dotnet/tree/release/v4?tab=readme-ov-file#variants).

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

And a telemetry publisher needs to be registered. Custom publishers can be defined, but for Application Insights one is already available in the `Microsoft.FeatureManagement.Telemetry.ApplicationInsights` package. Publishers can be added with a single line.

```csharp
builder.services
    .AddFeatureManagement()
    .AddTelemetryPublisher<ApplicationInsightsTelemetryPublisher>();
```

An example is available to demonstrate how to use the new Telemetry in an ASP.NET application. See [the example](https://github.com/microsoft/FeatureManagement-Dotnet/tree/release/v4/examples/EvaluationDataToApplicationInsights) in the examples folder.

For more details on Telemetry, see [here](https://github.com/microsoft/FeatureManagement-Dotnet/tree/release/v4?tab=readme-ov-file#telemetry).

### Additional Changes

#### IVariantFeatureManager

`IVariantFeatureManager` has been added as the successor of the existing `IFeatureManager`. It continues to offer the functions of `IFeatureManager`, but offers the new `GetVariantAsync` methods as well.

#### Cancellation Tokens

`IVariantFeatureManager` incorporates cancellation tokens into the methods of `IFeatureManager`. For existing apps to take advantage of cancellation tokens, use the `IVariantFeatureManager` interface instead and adjust calls to `IsEnabledAsync` or `GetFeatureNamesAsync` to include a `CancellationToken`. 

#### Status field

Status is a new optional field on a Feature that controls how a flag's enabled state is evaluated. Flags can set this field to `Disabled`. This will cause the flag to always act disabled, while the rest of the defined schema remains intact. See [here](https://github.com/microsoft/FeatureManagement-Dotnet/tree/release/v4?tab=readme-ov-file#status).

### Breaking Changes

There are no breaking changes in this release.

## 3.1.1 - December 13, 2023

### Bug Fixes
* Fixed a bug where feature manager will fail to add cache entry if the shared memory cache sets `SizeLimit`. ([#325](https://github.com/microsoft/FeatureManagement-Dotnet/issues/325)) 

## 3.1.0 - November 23, 2023

### Enhancements

* `FeatureManager` and `ConfigurationFeatureDefinitionProvider` are now public. ([#126](https://github.com/microsoft/FeatureManagement-Dotnet/issues/126))
   * Enables usage of external dependency injection containers.
   * Allows usage of `FeatureManager` without requiring dependency injection.

* Added support for server-side Blazor apps, where the following API can be used in place of the existing `AddFeatureManagement()` API. The new API registers the feature manager and feature filters as scoped services, while the existing API registers them as singletons. ([#258](https://github.com/microsoft/FeatureManagement-Dotnet/issues/258))
  ``` C#
  public static IFeatureManagementBuilder AddScopedFeatureManagement(this IServiceCollection services)
  ```

### Bug Fixes
* Fixed a bug introduced in the previous release where feature flags cannot be loaded from a custom section of configuration. ([#308](https://github.com/microsoft/FeatureManagement-Dotnet/issues/308))
* Fixed a bug introduced in the previous release where evaluation of a feature flag that references a contextual feature filter may throw an exception if there is no appropriate context provided during evaluation. ([#313](https://github.com/microsoft/FeatureManagement-Dotnet/issues/313))

## 3.0.0 - October 27, 2023

### Breaking Changes

* Dropped netcoreapp3.1 and net5.0 target frameworks since both have reached the end of their life cycle. https://github.com/microsoft/FeatureManagement-Dotnet/pull/267
* All feature flags must be defined in a `FeatureManagement` section within configuration. Previously flags were discovered at the top level of configuration if the `FeatureManagement` section was not defined, but this functionality has been removed. https://github.com/microsoft/FeatureManagement-Dotnet/pull/261

### Enhancements

* Built-in filters are registered by default. https://github.com/microsoft/FeatureManagement-Dotnet/pull/287
  This includes:
  * `TimeWindowFilter`
  * `ContextualTargetingFilter`
  * `PercentageFilter`
* `TargetingContextAccessor` can be added via the `.WithTargeting` extension method. This will automatically add the built-in `TargetingFilter`. https://github.com/microsoft/FeatureManagement-Dotnet/pull/287
* Contextual and non-contextual filters are now able to share the same name/alias. An example of two such filters are the built-in `TargetingFilter` and `ContextualTargetingFilter` that both use the alias `"Targeting"`. Given a scenario that a contextual and non-contextual filter are registered in the application, the filter that is used when evaluating a flag is dependent up on whether a context was passed in to `IFeatureManager.IsEnabled`. See 'contextual/non-contextual filter selection process' below for a more detailed explanation. https://github.com/microsoft/FeatureManagement-Dotnet/pull/262
* Added netstandard 2.1 as a target framework in the Microsoft.FeatureManagement package. https://github.com/microsoft/FeatureManagement-Dotnet/pull/267
* Added net7.0 as a target framework in the Microsoft.FeatureManagement.AspNetCore package. https://github.com/microsoft/FeatureManagement-Dotnet/pull/267

### Bug Fixes
* Prevents the usage of colon in Feature names.
* Adjusts log level for noisy warning when feature definitions are not found.
* Fixed an edge case in targeting if a user is allocated to exactly the 100th percentile (~1 in 2 billion chance)

### Migration

#### Adding built-in filters

It is no longer necessary to register the following filters manually:

  * `TimeWindowFilter`
  * `ContextualTargetingFilter`
  * `PercentageFilter`

The following code:

```
services.AddFeatureManagement()
    .AddFeatureFilter<TimeWindowFilter>();
```

should be simplified to:

```
services.AddFeatureManagement();
```

#### Adding Targeting Filter

Since the `TargetingFilter` (the non-contextual version) requires an implementation of ITargetingContextAccessor to function, it is not added by default. However, a discovery/helper method was added to streamline it's addition.

The following code:

```
services.AddSingleton<ITargetingContextAccessor, MyTargetingContextAccessor>();

services.AddFeatureManagement()
    .AddFeatureFilter<TargetingFilter>();
```

should be simplified to:

```
services.AddFeatureManagement()
    .WithTargeting<MyTargetingContextAccessor>();
```

### Additional

#### Contextual/non-contextual filter selection process
The following passage describes the process of selecting a filter when a contextual and non-contextual filter of the same name are registered in an application.

Let's say you have a non-contextual filter called FilterA and two contextual filters FilterB and FilterC which accept TypeB and TypeC contexts respectively. All of three filters share the same alias "SharedFilterName".
you also have a feature flag "MyFeature" which uses the feature filter "SharedFilterName" in its configuration.

If all of three filters are registered:
* When you call IsEnabledAsync("MyFeature"), the FilterA will be used to evaluate the feature flag.
* When you call IsEnabledAsync("MyFeature", context), if context's type is TypeB, FilterB will be used and if context's type is TypeC, FilterC will be used.
* When you call IsEnabledAsync("MyFeature", context), if context's type is TypeF, FilterA will be used.

## 2.6.1 - June 28, 2023

### Bug Fixes

* Fixed an edge case for EvaluateAsync call that doesn't use context from FeatureManager. ([#244](https://github.com/microsoft/FeatureManagement-Dotnet/issues/244))

## 2.6.0 - June 23, 2023

Promotes the changes in [2.6.0-preview](#260-preview2---june-7-2023) and [2.6.0-preview2](#260-preview2---june-7-2023) to stable. These changes include parameter caching, requirement type, and targeting exclusion.

## 2.6.0-preview2 - June 7, 2023

### Enhancement - Parameter Caching

Applications using built-in `ConfigurationFeatureDefinitionProvider` will now benefit from caching of feature filter parameters. This will improve performance of the application by reducing the number of times a filter's parameters are cast in short time frames, yielding observed performance increase of up to 100x. This change will not affect custom filters by default. For custom filters, the class must implement the `IFilterParametersBinder` interface. Below is an example.

```csharp
class MyFilter : IFeatureFilter, IFilterParametersBinder
{
    public object BindParameters(IConfiguration filterParameters)
    {
        return filterParameters.Get<FilterSettings>();
    }

    public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context, CancellationToken cancellationToken)
    {
        FilterSettings settings = (FilterSettings)context.Settings;
        ...
    }
}
```

For more details read [here](https://github.com/microsoft/FeatureManagement-Dotnet/pull/229)

## 2.6.0-preview - April 17, 2023

### Feature - RequirementType

Features can now declare a `RequirementType`. The default `RequirementType` is `Any`, which means if any of it's filters evaluate to true, then the feature will be enabled. Declaring a `RequirementType` of `All` means that every filter must evaluate to true in order for the feature to be enabled. Added in https://github.com/microsoft/FeatureManagement-Dotnet/pull/221.

```json
"FeatureW": {
    "RequirementType": "All",
    "EnabledFor": []
}
```

For more details read [here](https://github.com/microsoft/FeatureManagement-Dotnet#requirementtype)

### Targeting Exclusion

Targeting filters define an `Audience`. Now, `Audiences` can be fine tuned to exclude certain users and groups. By adding an `Exclusion` to an `Audience`, targeting filters will evaluate to false for users that are either directly defined, or a part of a group that is defined within the `Exclusion`. This takes priority over any other section of the Audience. Added in https://github.com/microsoft/FeatureManagement-Dotnet/pull/218.

```json
"Exclusion": {
    "Users": [
        "Mark"
    ],
    "Groups": [
        "Admins"
    ]
}
```

For more details read [here](https://github.com/microsoft/FeatureManagement-Dotnet#targeting-exclusion)

## 3.0.0-preview - June 17, 2022

### DEPRECATED

This release was deprecated. The dynamic feature functionality will be re-introduced in a later version with some design changes.

### Dynamic Features 

Dynamic features are a tool that can be used to surface different variants of a feature to different segments of an audience. Previously, this library only worked with feature flags. Feature flags are limited to boolean values, as they are either enabled or disabled. Dynamic features have dynamic values. They can be string, int, a complex object, or any other type. 

```cs
//
// Modify view based off multiple possible variants
model.BackgroundUrl = dynamicFeatureManager.GetVariantAsync<string>("HomeBackground", cancellationToken);

return View(model);
```

For more details read [here](https://github.com/microsoft/FeatureManagement-Dotnet/blob/release/v3/README.md#dynamic-features). 

### Cancellation token support 

Version 2 of Microsoft.FeatureManagement has an asynchronous pipeline, but cancellation token support was not added. Adding support for this in v2 would have required changing interfaces, thus a breaking change. V3 introduces this breaking change, and now proper cancellation is supported through the pipeline. 

### New Configuration Schema 

The original schema of the "FeatureManagement" configuration section treated all sub objects as feature flags. Now there are dynamic features alongside feature flags. Additionally, there are other switches that are expected to be added in the future to customize global feature management state. To make room for this the schema has been updated. 

```json
{
    "FeatureManagement": {
        "FeatureFlags": {
        },
        "DynamicFeatures": {
        }
    }
}
```

For more details read [here](https://github.com/microsoft/FeatureManagement-Dotnet/blob/release/v3/README.md#feature-flag-declaration).

### Breaking Changes

* `IFeatureFilter.EvaluateAsync` now accepts a cancellation token.
  * `IFeatureFilter.EvaluateAsync(FeatureFilterEvaluationContext)` -> `IFeatureFilter.EvaluateAsync(FeatureFilterEvaluationContext, CancellationToken)`
  * All built-in feature filters `EvaluateAsync` method now require a cancellation token. 
  * An equivalent change applies to `IContextualFeatureFilter`.
* `ITargetingContextAccessor.GetContextAsync` now accepts a cancellation token. 
  * `ITargetingContextAccessor.GetContextAsync()` -> `ITargetingContextAccessor.GetContextAsync(CancellationToken)`.
* All async `IFeatureManager` methods now accept a cancellation token.
* `IFeatureManager.GetFeatureNamesAsync` has been renamed to `IFeatureManager.GetFeatureFlagNamesAsync`.
* `IFeatureDefinitionProvider` has been renamed to `IFeatureFlagDefinitionProvider`.
  * All methods now accept cancellation token.
* `ISessionManager` now accepts cancellation token.
* `FeatureDefinition` renamed to `FeatureFlagDefinition`.
* `IFeatureManagementBuilder` now declares `AddFeatureVariantAssigner`.
* `FeatureFilterEvaluationContext.FeatureName` renamed to `FeatureFilterEvaluationContext.FeatureFlagName`

## 2.5.1 - April 6, 2022

### Bug Fixes

* Updated summary on `FeatureGateAttribute` to mention that it is usable on Razor pages. [#170](https://github.com/microsoft/FeatureManagement-Dotnet/pull/170)

## 2.5.0 - April 4, 2022

### Enhancements

* Updated `FeatureGateAttribute` to support Razor pages. This attribute can be placed on Razor page handlers to control access to the page based on whether a feature flag is on or off. [#166](https://github.com/microsoft/FeatureManagement-Dotnet/pull/166)

### Bug Fixes

* Fixed an issue in `PercentageFilter` where a feature may occasionally be considered as on even when the filter is set to 0 percent. [#156](https://github.com/microsoft/FeatureManagement-Dotnet/pull/156)

## 2.4.0 - September 24, 2021

### Enhancements

* Added option to throw when attempting to evaluate a missing feature. [#140](https://github.com/microsoft/FeatureManagement-Dotnet/pull/140)
* `IFeatureManagementSnapshot` is now thread-safe. [#141](https://github.com/microsoft/FeatureManagement-Dotnet/pull/141)

### Bug Fixes

* `FilterAliasAttribute` now uses the proper parameter name in an `ArgumentNullException` if `alias` is null.

## 2.3.0 - April 15, 2021

### net5.0 Targeting

The net5.0 framework has been added to the list of target frameworks. This change resolves dependency issues for ASP.NET Core 5.0 applications.

### Bug Fixes

* The license URL for these packages has been fixed.

## 2.2.0 - September 16, 2020

No changes have been made in this version. This is the first stable release with the targeting feature filter (introduced in 2.1.0-preview) and custom feature providers (introduced in 2.2.0-preview).

## 2.2.0-preview - July 10, 2020

### Custom Feature Providers

Support for custom feature providers has been added. [#79](https://github.com/microsoft/FeatureManagement-Dotnet/pull/79)

Implementing a custom feature provider enables developers to to read feature flags from sources such as a database or a feature management service. For more information on the concept of custom feature providers and how to use this new feature take a look at the project's [readme](https://github.com/microsoft/FeatureManagement-Dotnet#custom-feature-providers).

### Netcoreapp3.1 Targeting

The netcoreapp3.1 framework has been added to the list of target frameworks. This change resolves dependency issues for ASP.NET Core 3.1 applications. [#77](https://github.com/microsoft/FeatureManagement-Dotnet/pull/77)

## 2.1.0-preview - Apr 22, 2020

### Targeting

Support for rolling out features to a target audience has been added through built in feature filters. [#56](https://github.com/microsoft/FeatureManagement-Dotnet/pull/56)

Targeting enables developers to progressively roll out features to a target audience that can be increased gradually. For more information on the concept of targeting and how to use this new feature take a look at the project's [readme](https://github.com/microsoft/FeatureManagement-Dotnet#targeting).

## 2.0.0 - Feb 26, 2020

### Enumerating Feature Names

The `IFeatureManager` interface now exposes a way to enumerate all feature names that are registered in the system. This enables work flows where the states of all known features need to be evaluated.

```
IFeatureManager fm;

await foreach (string featureName in fm.GetFeatureNamesAsync())
{
  await IsEnabledAsync(featureName);
}
```

**Important:** Using the [`await foreach`](https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/generate-consume-asynchronous-stream#convert-to-async-streams) syntax requires using [version 8.0 or above of C#](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/configure-language-version).

### Missing Feature Filters

When the feature manager tries to evaluate the state of a feature that depends on a missing feature filter it will now throw a `FeatureManagementException` with the error `MissingFeatureFilter`.

The new fail-fast behavior can be disabled via feature management options if the old behavior to ignore missing feature filters is desired.

```
services.Configure<FeatureManagementOptions>(options =>
{
    options.IgnoreMissingFeatureFilters = true;
});
```

### Breaking Changes

* FeatureManager now throws a `FeatureManagementException` with error `AmbiguousFeatureFilter`, instead of `InvalidOperationException`, if a feature makes an ambiguous reference to two or more feature filters.
* `Task<bool> ISessionManager.TryGetAsync(string featureName, out bool enabled)` has been changed to `Task<bool?> ISessionManager.GetAsync(string featureName)` to enable async implementations.

## 2.0.0-preview-010610001-1263 - Nov 27, 2019

### Async Feature Filters

Support for async feature filters has been added. This results in the **entire** feature management **pipeline** being asynchronous. Async feature filters pave the way to performing async workloads in feature filters if desired.

**Before**

```
IFeatureManager fm;

if (fm.IsEnabled("MyFeature"))
{

}
```

**After**

```
IFeatureManager fm;

if (await fm.IsEnabledAsync("MyFeature"))
{

}
```

### Floating Context Support

The original design for the feature management library relied on applications to have an ambient context. An application's ambient context could be used in feature filters to obtain information such as user identity and other information relevant when toggling features. This led to a disconnect in console applications which do not have an ambient context in most cases. Now application's without an ambient context can float a context into the feature management system by using the new `IFeatureManager.IsEnabledAsync<TContext>(string feature, TContext context)` method. The `context` parameter is able to be consumed by feature filters that implement `IContextualFeatureFilter`. 

**Consumption**

The ability to pass a context when evaluating a feature has been added to `IFeatureManager`.

```
IFeatureManager fm = services.GetRequiredService<IFeatureManager>();

await fm.IsEnabledAsync("featureName", new MyApplicationContext
{
  UserId = "someUser"
});
```

**Contextual Feature Filters**

Contextual feature filters are feature filters that can utilize a context provided by the application when evaluating whether a feature is on or off. Contextual feature filters are a generic type. Their generic type parameter describes the interface that the passed in context must implement for the filter to be able to evaluate it.

As an example, `IContextualFeatureFilter<IAccountContext>` requires a context that implements `IAccountContext` to be passed in. If a feature is checked for enabled and a context is provided that does not implement `IAccountContext` then the previously mentioned filter would not run.

**IFeatureFilterMetadata**

With the introduction of `IContextualFeatureFilter` there are now two types of feature filters including `IFeatureFilter`. The two types of feature filters both inherit `IFeatureFilterMetadata`. `IFeatureFilterMetadata` is a marker interface and does not actually provide any feature filtering capabilities. It is used as the new parameter type for `IFeatureManagementBuilder.AddFeatureFilter`.

**Performance Improvement**

A cache for feature settings has been added which respects the reload token of the .NET Core configuration system. If a configuration provider is used that does not properly trigger the reload token of the .NET Core configuration system, `FeatureManager` will not be able to pickup changes.

### Breaking Changes
* `IFeatureManager.IsEnabled` is now asynchronous
  * `IsEnabled` was renamed to `IsEnabledAsync`.
  * `IFeatureManagerSnapshot.IsEnabled` is also affected.
* `IFeatureFilter.Evaluate` is now asynchronous
  * `Evaluate` was renamed to `EvaluateAsync`.
* `Mvc.Filters.FilterCollection.AddForFeature` now only accepts
`IAsyncActionFilter` rather than any type of filter.
* `AddRouteForFeature` has been removed.
* `ISessionManager` is now an async interface.

## 1.0.0-preview-009000001-1251 - June 20, 2019

* Renamed 'Microsoft.FeatureManagment.FeatureAttribute' to 'Microsoft.FeatureManagment.Mvc.FeatureGateAttribute'.
* Enhanced FeatureGateAttribute to allow specifying whether 'any' or 'all' features need to be enabled.
* Enhanced feature tag helper to allow for multiple features, any/all requirement, and negated logic.
* Added `IFeatureManagementBuilder.AddSessionManager` to enhance discoverability for providing a custom feature session manager.
  * Previous approach was `IServiceCollection.AddSingleton<ISessionManager>(MySessionManager)`

<!-- LINKS -->
[docs]: https://github.com/microsoft/FeatureManagement-Dotnet
[package]: https://www.nuget.org/packages/Microsoft.FeatureManagement
[samples]: https://github.com/microsoft/FeatureManagement-Dotnet/tree/master/examples/ConsoleApp
[source_code]: https://github.com/microsoft/FeatureManagement-Dotnet
