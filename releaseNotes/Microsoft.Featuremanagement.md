# Microsoft.FeatureManagement
[Source code][source_code] | [Package (NuGet)][package] | [Samples][samples] | [Product documentation][docs]

# Microsoft.FeatureManagement.AspNetCore
[Source code ][source_code_web] | [Package (NuGet)][package_web] | [Samples][samples_web] | [Product documentation][docs]

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

[package_web]: https://www.nuget.org/packages/Microsoft.FeatureManagement.AspNetCore
[samples_web]: https://github.com/microsoft/FeatureManagement-Dotnet/tree/master/examples/FeatureFlagDemo
[source_code_web]: https://github.com/microsoft/FeatureManagement-Dotnet/tree/master/src/Microsoft.FeatureManagement.AspNetCore