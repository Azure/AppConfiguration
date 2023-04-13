# spring-cloud-azure-feature-management

[Source code][source_code] | [Package (Maven)][package] | [Samples][samples] | [Product documentation][docs]

[CHANGELOG](https://github.com/Azure/azure-sdk-for-java/blob/main/sdk/spring/spring-cloud-azure-feature-management/CHANGELOG.md)

# spring-cloud-azure-feature-management-web

[Source code][source_code_web] | [Package (Maven)][package_web]

## 4.7.0 - April 06, 2023

### Breaking Changes

* Libraries and namespaces to `spring-cloud-azure-feature-management-web` and `com.azure.spring.cloud.feature.management`.

## 1.3.0 - April 21, 2021

* Updated to newer versions of dependencies. Spring Boot 2.4.3 and Spring Cloud 3.0.1.

## 1.2.9 - March 18, 2021

* Updated to newer versions of dependencies. Spring Boot 2.3.5.RELEASE and Spring Cloud 2.2.5.RELEASE.

## 1.2.8 - March 9, 2021

* Should not be used, released with incorrect version of Spring Boot.

## 1.2.7 - July 14, 2020

* Fixed the bug that feature flags that do not use feature filters will appear always off regardless of their actual states.

## 1.1.2/1.2.2 - February 25, 2020

* `FeatureManager` now has a method `getAllFeatureNames` which will return the names of all loaded Feature Flags.
* Simplified Feature Management config schema format, there is now a schema file under docs/FeatureManagement/Clients/SpringCloud. Example:

```yaml
feature-management:
  feature-t: false
  feature-u:
    enabled-for:
      -
        name: Random
  feature-v:
    enabled-for:
      -
        name: TimeWindow
        parameters:
          start: "Wed, 01 May 2019 13:59:59 GMT"
          end:   "Mon, 01 July 2019 00:00:00 GMT"
```

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
[docs]: https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/spring/spring-cloud-azure-feature-management
[package]: https://mvnrepository.com/artifact/com.azure.spring/spring-cloud-azure-feature-management
[samples]: https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/appconfiguration
[source_code]: https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/spring/spring-cloud-azure-feature-management

[package_web]: https://mvnrepository.com/artifact/com.azure.spring/spring-cloud-azure-feature-management-web
[source_code_web]: https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/spring/spring-cloud-azure-feature-management-web
