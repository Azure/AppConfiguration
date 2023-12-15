# Azure App Configuration JavaScript Provider

[Source code][source_code] | [Package (npm)][package] | [Samples][samples]

## 1.0.0-preview.2 - December 15, 2023
## Breaking Changes
- Changed the behavior when multiple labels are filtered in one selector. In the JavaScript provider configuration settings are loaded into a `Map` containing `key` and `value`, without preserving the `label`. If multiple labels are matched in one selector, the provider cannot determine which one to use. Now it will throw an error if label filter contains `*` or `,`. [#22](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/22)

## Enhancements
- Added more information to the doc comments for better IntelliSense. [#32](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/32)

## Bug Fixes
- Fixed wrong precedence of selectors after deduplication. [#31](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/31)
- Fixed type mismatch for `ConfigurationSettingId.label` by updating @azure/app-configuration to 1.5.0. [Azure/azure-sdk-for-js#27607](https://github.com/Azure/azure-sdk-for-js/issues/27607)
- Fixed retry logic when host of configuration store cannot be resolved, by updating @azure/core-rest-pipeline to 1.12.2. [Azure/azure-sdk-for-js#27037](https://github.com/Azure/azure-sdk-for-js/issues/27037)


## 1.0.0-preview.1 - October 24, 2023
### Bug fixes
- Updated version of @azure/identity to 3.3.2, fixing security issue [CVE-2023-36415](https://msrc.microsoft.com/update-guide/en-US/vulnerability/CVE-2023-36415)

## 1.0.0-preview - October 11, 2023
### Enhancements
- Added basic features for loading settings from configuration stores. [#1](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/1)
  - Support authentication with connection string or AAD.
  - Select settings with key/label filters.
  - Trim key with prefixes.
- Added support to resolve Azure Key Vault secret references automatically. [#2](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/2)
- Added support to parse JSON objects for applicable content types automatically. [#8](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/8)

[package]: https://www.npmjs.com/package/@azure/app-configuration-provider
[samples]: https://github.com/Azure/AppConfiguration-JavaScriptProvider/tree/main/examples
[source_code]: https://github.com/Azure/AppConfiguration-JavaScriptProvider
