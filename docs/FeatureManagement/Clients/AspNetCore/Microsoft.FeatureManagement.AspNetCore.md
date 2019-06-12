# ASP.Net Core Feature Flags

Feature flags provide a way for ASP.Net Core applications to turn features on or off dynamically. Developers can use feature flags in simple use cases like conditional statements to more advanced scenarios like conditionally adding routes or MVC filters. Feature flags build on top of the .NET Core configuration system. Any .NET Core configuration provider is capable of acting as the back-bone for feature flags.

Here are some of the benefits of using this library:

* A common convention for feature management
* Low barrier-to-entry
  * Built on `IConfiguration`
  * Supports JSON file feature flag setup
* Feature Flag lifetime management
  * Configuration values can change in real-time, feature flags can be consistent across the entire request
* API extensions for ASP.Net Core and MVC framework
  * Routing
  * Filters
  * Action Attributes

**API Reference**: https://go.microsoft.com/fwlink/?linkid=2091700

### Feature Flags
Feature flags are composed of two parts, a name and a list of feature-filters that are used to turn the feature on.

### Feature Filters
Feature filters define a scenario for when a feature should be enabled. When a feature is evaluated for whether it is on or off, its list of feature-filters are traversed until one of the filters decides the feature should be enabled. At this point the feature is considered enabled and traversal through the feature filters stops. If no feature filter indicates that the feature should be enabled, then it will be considered disabled.

As an example, a Microsoft Edge browser feature filter could be designed. This feature filter would activate any features it is attached to as long as an HTTP request is coming from Microsoft Edge.

## Registration

The .NET Core configuration system is used to determine the state of feature flags. The foundation of this system is `IConfiguration`. Any provider for IConfiguration can be used as the feature state provider for the feature flag library. This enables scenarios ranging from appsettings.json to Azure App Configuration and more.

### Feature Flag Declaration

The feature management library supports appsettings.json as a feature flag source since it is a provider for .NET Core's IConfiguration system. Below we have an example of the format used to set up feature flags in a json file.

```
{ 
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },

  // Define feature flags in a json file
  "FeatureManagement": {
    "FeatureT": {
      "EnabledFor": [
        {
          "Name": "AlwaysOn"
        }
      ]
    }
    "FeatureU": {
      "EnabledFor": []
    }
    "FeatureV": {
      "EnabledFor": [
        {
          "Name": "TimeWindow",
          "Parameters": {
            "Start": "Wed, 01 May 2019 13:59:59 GMT",
            "End": "Mon, 01 July 2019 00:00:00 GMT"
          }
        }
      ]
    }
  }
}
```

The `FeatureManagement` section of the json document is used by convention to load feature flag settings. In the section above, we see that we have provided three different features. Features define their feature filters using the `EnabledFor` property. In the feature filters for `FeatureT` we see `AlwaysOn`. This feature filter is built-in and if specified will always enable the feature. The `AlwaysOn` feature filter does not require any configuration so it only has the _Name_ property. `FeatureU` has no filters in its `EnabledFor` property and thus will never be enabled. Any functionality that relies on this feature being enabled will not be accessible as long as the feature filters remain empty. However, as soon as a feature filter is added that enables the feature it can begin working. `FeatureV` specifies a feature filter named `TimeWindow`. This is an example of a configurable feature filter. We can see in the example that the filter has a parameter's property. This is used to configure the filter. In this case, the start and end times for the feature to be active are configured.

### On/Off Declaration
 
The following snippet demonstrates an alternative way to define a feature that can be used for on/off features. 
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },

  // Define feature flags in config file
  "FeatureManagement": {
    "FeatureT": true, // On feature
    "FeatureX": false // Off feature
  }
}
```
### Referencing

To make it easier to reference these feature flags in code, we recommend to define feature flag variables like below.

    // Define feature flags in an enum
    [FeatureFlag]
    public enum MyFeatureFlags
    {
        FeatureT,
        FeatureU,
        FeatureV
    }
    
### Service Registration

Feature flags rely on .NET Core dependency injection. We can register the feature management services using standard conventions.

```
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;

public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
      services.AddFeatureManagement()
              .AddFeatureFilter<PercentageFilter>()
              .AddFeatureFilter<TimeWindowFilter>();
  }
}
```

This tells the feature manager to use the "FeatureManagement" section from the configuration for feature flag settings. It also registers two built-in feature filters named `PercentageFilter` and `TimeWindowFilter`. When filters are referenced in feature flag settings (appsettings.json) the _Filter_ part of the type name can be omitted.


**Advanced:** If the root of the configuration is provided, the feature manager will first look for a configuration section with the name of the feature, and will then fall back to the "FeatureManagement" section automatically.

## Consumption
The simplest use case for feature flags is to do a conditional check for whether a feature is enabled to take different paths in code. The uses cases grow from there as the feature flag API begins to offer extensions into ASP.Net Core.

### Feature Check
The basic form of feature management is checking if a feature is enabled and then performing actions based on the result. This is done through the `IFeatureManager`'s `IsEnabled` method.

```
...
IFeatureManager featureManager;
...
if (featureManager.IsEnabled(nameof(MyFeatureFlags.FeatureU)))
{
    // Do something
}
```

### Dependency Injection

When using the feature management library with MVC, the `IFeatureManager` can be obtained through dependency injection.

```
public class HomeController : Controller
{
    private readonly IFeatureManager _featureManager;
    
    public HomeController(IFeatureManager featureManager)
    {
        _featureManager = featureManager;
    }
}
```

### Controllers and Actions
MVC controller and actions can require that a given feature, or one of any list of features, be enabled in order to execute. This can be done through the `FeatureAttribute`. 

```
[Feature(MyFeatureFlags.FeatureX)]
public class HomeController : Controller
{
    ...
}
```

The `HomeController` above requires "FeatureX" to be enabled before any action it contains can be executed. 

```
[Feature(MyFeatureFlags.FeatureY)]
public IActionResult Index()
{
    return View();
}
```

The `Index` MVC action above requires "FeatureY" to be enabled before it can execute. 

### Disabled Action Handling

When an MVC controller or action is blocked because none of the features it specifies are enabled, a registered `IDisabledFeaturesHandler` will be invoked. By default, a minimalistic handler is registered which returns HTTP 404. This can be overridden using the `IFeatureManagementBuilder` when registering feature flags.

```
public interface IDisabledFeaturesHandler
{
    Task HandleDisabledFeature(IEnumerable<string> features, ActionExecutingContext context);
}
```

### View

In MVC views `<feature>` tags can be used to conditionally render content based on whether a feature is enabled or not.

```
<feature name=@nameof(MyFeatureFlags.FeatureX)>
  <p>This can only be seen if 'FeatureX' is enabled.</p>
</feature>
```

The `<feature>` tag requires a tag helper to work. This can be done by adding the feature management tag helper to the _ViewImports.cshtml file.
```
@addTagHelper *, Microsoft.FeatureManagement.AspNetCore
```

### MVC Filters

MVC filters can be set up to conditionally execute based on the state of a feature. This is done by registering MVC filters in a feature aware manner.

```
services.AddMvc(o => 
{
    o.Filters.AddForFeature<SomeMvcFilter>(nameof(MyFeatureFlags.FeatureV));
});
```

The code above adds an MVC filter named `SomeMvcFilter`. This filter is only triggered within the MVC pipeline if the feature it specifies, "FeatureV", is enabled.

### Routing
Certain routes may expose application capabilites that are gated by features. These routes can be dynamically registered and exposed based on whether a feature is enabled.

```
app.UseMvc(routes =>
{
    routes.MapRouteForFeature(nameof(MyFeatureFlags.Beta), "betaDefault" /* route name */, "{controller=Beta}/{action=Index}/{id?}"
});
```

The route above exposes beta functionality of some application that is gated behind the "Beta" feature flag. The state of this feature is evaluated on every request, which means routes can be exposed to a subset of requests and or users. Furthermore, routes can dynamically be added or removed based on the toggling of feature states.

### Application building

The feature management library can be used to add application branches and middleware that execute conditionally based on feature state.

```
app.UseMiddlewareForFeature<ThirdPartyMiddleware>(nameof(MyFeatureFlags.FeatureU));
```

With the above call, the application adds a middleware component that only appears in the request pipeline if the feature "FeatureU" is enabled. If the feature is enabled/disabled during runtime, the middleware pipeline can be changed dynamically.

This builds off the more generic capability to branch the entire application based on a feature.

```
app.UseForFeature(featureName, appBuilder => 
{
    appBuilder.UseMiddleware<T>();
});
```

## Implementing a Feature Filter

Creating a feature filter provides a way to enable features based on criteria that you define. To implement a feature filter, the `IFeatureFilter` interface must be implemented. `IFeatureFilter` has a single method named `Evaluate`. When a feature specifies that it can be enabled for a feature filter, the `Evaluate` method is called. If `Evaluate` returns `true` it means the feature should be enabled.

Feature filters are registered by the `IFeatureManagementBuilder` when `AddFeatureManagement` is called. These feature filters have access to the services that exist within the service collection that was used to add feature flags. Dependency injection can be used to retrieve these services.

### Parameterized Feature Filters

Some feature filters require parameters to decide whether a feature should be turned on or not. For example a browser feature filter may turn on a feature for a certain set of browsers. It may be desired that Edge and Chrome browsers enable a feature, while FireFox does not. To do this a feature filter can be designed to expect parameters. These parameters would be specified in the feature configuration, and in code would be accessible via the `FeatureFilterEvaluationContext` parameter of `IFeatureFilter.Evaluate`.

```
  public class FeatureFilterEvaluationContext
  {
      /// <summary>
      /// The name of the feature being evaluated.
      /// </summary>
      public string FeatureName { get; set; }

      /// <summary>
      /// The settings provided for the feature filter to use when evaluating whether the feature should be enabled.
      /// </summary>
      public IConfiguration Parameters { get; set; }
  }
```

`FeatureFilterEvaluationContext` has a property named `Parameters`. These parameters represent a raw configuration that the feature filter can use to decide how to evaluate whether the feature should be enabled or not. To use the browser feature filter as an example once again, the filter could use `Parameters` to extract a set of allowed browsers that would have been specified for the feature and then check if the request is being sent from one of those browsers.

```
  [FilterAlias("Browser")]
  public class BrowserFilter : IFeatureFilter
  {
      ... Removed for example

      public bool Evaluate(FeatureFilterEvaluationContext context)
      {
          BrowserFilterSettings settings = context.Parameters.Get<BrowserFilterSettings>() ?? new BrowserFilterSettings();

          //
          // Here we would use the settings and see if the request was sent from any of BrowserFilterSettings.AllowedBrowsers
      }
  }
```

### Filter Alias Attribute

When a feature filter is registered to be used for a feature flag, the alias used in configuration is the name of the feature filter type with the _filter_ suffix, if any, removed. For example `MyCriteriaFilter` would be referred to as _MyCriteria_ in configuration.

  "MyFeature": {
    "EnabledFor": [
      {
        "Name": "MyCriteria"
      }
    ]
  }

This can be overridden through the use of the `FilterAliasAttribute`. A feature filter can be decorated with this attribute to declare the name that should be used in configuration to reference this feature filter within a feature flag.

### Using HttpContext

Feature filters can evaluate whether a feature should be enabled based off the properties of an HTTP Request. This is performed by inspecting the HTTP Context. A feature filter can get a reference to the HTTP Context by obtaining an `IHttpContextAccessor` through dependency injection.

```
public class BrowserFilter : IFeatureFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BrowserFilter(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }
}
```

The `IHttpContextAccessor` must be added to the dependency injection container on startup for it to be available. It can be registered in the `IServiceCollection` using the following method.

```
public void ConfigureServices(IServiceCollection services)
{
    …
    services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    …
}
```

### Built-In Feature Filters

There a few feature filters that come with the `Microsoft.FeatureManagement` package. These feature filters are not added automatically, but can be referenced and registered as soon as the package is registered.

Each of the built-in feature filters have their own parameters. Here is the list of feature filters along with examples.

#### Microsoft.Percentage

This filter provides the capability to enable a feature based on a set percentage.

```
    "EnhancedPipeline": {
      "EnabledFor": [
        {
          "Name": "Microsoft.Percentage",
          "Parameters": { 
            "Value": 50
          }
        }
      ]
    }
```

#### Microsoft.TimeWindow

This filter provides the capability to enable a feature based on a time window. If only `End` is specified, the feature will be considered on until that time. If only start is specified, the feature will be considered on at all points after that time.

```    "EnhancedPipeline": {
      "EnabledFor": [
        {
          "Name": "Microsoft.TimeWindow",
          "Parameters": {
            "Start": "Wed, 01 May 2019 13:59:59 GMT",
            "End": "Mon, 01 July 2019 00:00:00 GMT"
          }
        }
      ]
    }
```

### Feature Filter Alias Namespaces

All of the built-in feature filter alias' are in the 'Microsoft' feature filter namespace. This is to prevent conflicts with other feature filters that may share the same simple alias. The segments of a feature filter namespace are split by the '.' character. A feature filter can be referenced by its fully qualified alias such as 'Microsoft.Percentage' or by the last segment which in the case of 'Microsoft.Percentage' is 'Percentage'.

## Caching

Feature state is provided by the IConfiguration system. Any caching and dynamic updating is expected to be handled by configuration providers. The feature manager asks IConfiguration for the latest value of a feature's state whenever a feature is checked to be enabled.

### Snapshot
There are scenarios which require the state of a feature to remain consistent during the lifetime of a request. The values returned from the standard `IFeatureManager` may change if the `IConfiguration` source which it is pulling from is updated during the request. This can be prevented by using `IFeatureManagerSnapshot`. `IFeatureManagerSnapshot` can be retrieved in the same manner as `IFeatureManager`. `IFeatureManagerSnapshot` implements the interface of `IFeatureManager`, but it caches the first evaluated state of a feature during a request and will return the same state of a feature during its lifetime.
