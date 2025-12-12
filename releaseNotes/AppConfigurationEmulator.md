# Azure App Configuration Emulator

[Image][image] | [Source code][source_code] | [Samples][samples]

## 1.0.2 - December 11, 2025

### Bug fixes

* Fixed a bug where the null label `%00` in URL queries was not recognized correctly. [#63](https://github.com/Azure/AppConfiguration-Emulator/pull/63)

## 1.0.1 - December 4, 2025

### Bug fixes

* Upgraded the React version for the emulator UI. [#64](https://github.com/Azure/AppConfiguration-Emulator/pull/64)

## 1.0.0 - October 29, 2025

Initial stable release of the Azure App Configuration emulator.

### Enhancements

This release builds upon 1.0.0-preview and includes the following enhancements:

* Added support for the following API versions:
  * `1.0`
  * `2023-10-01`
  * `2023-11-01`
  * `2024-09-01`
* Added support for volume mount of the storage folder, letting containers persist configuration with a lightweight host volume. [#53](https://github.com/Azure/AppConfiguration-Emulator/pull/53)

### Bug fixes

* Fixed a bug where missing sync-token headers caused client SDKs to crash with null pointer panic. [#46](https://github.com/Azure/AppConfiguration-Emulator/pull/46) ([#43](https://github.com/Azure/AppConfiguration-Emulator/issues/43))


## 1.0.0-preview - July 31, 2025

Initial preview release of the Azure App Configuration emulator.

The Azure App Configuration Emulator is a local development tool that provides a lightweight implementation of the Azure App Configuration service. This emulator allows developers to test and develop applications locally without requiring an active Azure subscription or connection to the cloud service.

### Supported Features

* Authentication Methods
  * Anonymous authentication
  * HMAC authentication
* Resources
  * `/keys`
  * `/kv`
  * `/labels`
  * `/locks`
  * `/revisions`
* Web UI

<!-- LINKS -->
[image]: https://mcr.microsoft.com/artifact/mar/azure-app-configuration/app-configuration-emulator/about
[source_code]: https://github.com/Azure/AppConfiguration-Emulator
[samples]: https://github.com/Azure/AppConfiguration-Emulator/tree/main/examples