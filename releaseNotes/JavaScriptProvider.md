# Azure App Configuration JavaScript Provider

[Source code][source_code] | [Package (npm)][package] | [Samples][samples]

## 1.0.0-preview.1 - October 24, 2023
### Fixed
- Updated version of @azure/identity to 3.3.2, for [CVE-2023-36415](https://msrc.microsoft.com/update-guide/en-US/vulnerability/CVE-2023-36415)

## 1.0.0-preview - October 11, 2023
### Added
- Added basic features for loading settings from configuration stores. [#1](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/1)
  - Support authentication with connection string or AAD.
  - Select settings with key/label filters.
  - Trim key with prefixes.
- Added support to resolve Azure Key Vault secret references automatically. [#2](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/2)
- Added support to parse JSON objects for applicable content types automatically. [#8](https://github.com/Azure/AppConfiguration-JavaScriptProvider/pull/8)

[package]: https://www.npmjs.com/package/@azure/app-configuration-provider
[samples]: https://github.com/Azure/AppConfiguration-JavaScriptProvider/tree/main/examples
[source_code]: https://github.com/Azure/AppConfiguration-JavaScriptProvider
