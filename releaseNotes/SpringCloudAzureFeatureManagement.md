# spring-cloud-azure-feature-management

[Source code][source_code] | [Package (Maven)][package] | [Samples][samples] | [Product documentation][docs]

# spring-cloud-azure-feature-management-web

[Source code web ][source_code_web] | [Package (Maven) web][package_web] | [Samples web][samples_web] | [Product documentation][docs]

## 1.1.1/1.2.1 - January 13, 2020

* The `isEnabled` method is now non-blocking Async and has been renamed to `isEnabledAsync` using Reactor.

## 1.1.0.M6 - December 09, 2019

* Fixed a bug where feature flag names were unable to contain dots (.) when they are read from a configuration file.

## 1.1.0.M5 - October 28, 2019

* Updated error message when filter is not found.
* Fixed bug where null values could be passed in as a feature.

## 1.1.0.M4 - August 27, 2019

* Initial Release
* Adds Feature Flags and Feature Filters
* Supports property file based feature flags
* Adds Support for Request based Feature Flags
* Adds FeatureGates, Disabled Action Handling, and Routing away from disabled features

<!-- LINKS -->
[docs]: https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-feature-management
[package]: https://mvnrepository.com/artifact/com.microsoft.azure/spring-cloud-azure-feature-management
[samples]: https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-samples/feature-management-sample
[source_code]: https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-feature-management

[package_web]: https://mvnrepository.com/artifact/com.microsoft.azure/spring-cloud-azure-feature-management-web
[samples_web]: https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-samples/feature-management-web-sample
[source_code_web]: https://github.com/microsoft/spring-cloud-azure/tree/master/spring-cloud-azure-feature-management-web
