# Microsoft.FeatureManagement and Microsoft.FeatureManagement.AspNetCore

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
