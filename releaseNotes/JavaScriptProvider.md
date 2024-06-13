# Azure App Configuration JavaScript Provider

[Source code][source_code] | [Package (npm)][package] | [Samples][samples]

## 1.0.0 - June 6, 2024

This is the first stable release of the following features.
- Loading and composing configuration with key-value selectors.
- Authentication with connection string or Microsoft Entra Id.
- Loading key-values as either a `Map` or an object.
- Trimming prefixes from keys.
- Loading key-values with JSON content type as configuration objects.
- Key Vault reference resolution.
- Support for dynamic configuration refresh.

Get started with the [quickstart](https://learn.microsoft.com/en-us/azure/azure-app-configuration/quickstart-javascript-provider).

## 1.0.0-preview.4 - April 11, 2024

### Breaking Changes
- Excluded feature flags from loaded settings. For example, if you attempt to load all settings by specifying a selector with `keyFilter: "*"`, only configuration settings are included now, whereas previously feature flags were also loaded. [#55](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/55)

## 1.0.0-preview.3 - March 21, 2024
### Enhancements
- Added support for dynamic configuration refresh. See an example [here](https://github.com/Azure/AppConfiguration-JavaScriptProvider/blob/main/examples/refresh.mjs). [#21](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/21)
- Added support for consuming configuration as an object. A new API, `constructConfigurationObject`, has been added to construct a configuration object based on the key-values loaded from Azure App Configuration. It minimizes necessary code changes for applications that were using JSON configuration file to adopt Azure App Configuration. [#49](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/49)

## 1.0.0-preview.2 - December 15, 2023
### Breaking Changes
- The label filter in a selector is restricted to a single label. An error will be thrown if a label filter contains `*` or `,`. This change is to avoid the ambiguity when multiple values are loaded for the same key. Key-values with different labels can still be loaded using separate selectors for proper configuration composition. [#22](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/22)
- Fixed a bug where the last selector may not take precedence if multiple selectors with the same key and label filters are provided. After applying the fix, the resulting configuration of an application may change. To ensure the intended configuration composition, remove any duplicated selectors.[#23](https://github.com/Azure/AppConfiguration-JavaScriptProvider/issues/23)

### Bug Fixes
- Updated the reference of `@azure/core-rest-pipeline` to `1.12.2`, which added retry for DNS resolution of App Configuration endpoints. [Azure/azure-sdk-for-js#27037](https://github.com/Azure/azure-sdk-for-js/issues/27037)

## 1.0.0-preview.1 - October 24, 2023
### Bug fixes
- Updated the reference of `@azure/identity` to `3.3.2`, addressing [CVE-2023-36415](https://msrc.microsoft.com/update-guide/en-US/vulnerability/CVE-2023-36415)

## 1.0.0-preview - October 11, 2023
Added support for
- authentication with connection string or Microsoft Entra
- loading and composing configuration with key-value selectors
- key prefix trimming
- Key Vault reference resolution
- Key-value with JSON content-type

[package]: https://www.npmjs.com/package/@azure/app-configuration-provider
[samples]: https://github.com/Azure/AppConfiguration-JavaScriptProvider/tree/main/examples
[source_code]: https://github.com/Azure/AppConfiguration-JavaScriptProvider
