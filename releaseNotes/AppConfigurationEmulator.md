# Azure App Configuration Emulator

[Image][image] | [Source code][source_code] | [Samples][samples]

## 1.0.0 - October 29. 2025

Initial stable release of the Azure App Configuration emulator.

### Enhancements

This release builds upon 1.0.0-preview and includes the following enhancements:

* Added sync-token header in responses [#46](https://github.com/Azure/AppConfiguration-Emulator/pull/46)
* Supported the following API versions:
  * `1.0`
  * `2023-10-01`
  * `2023-11-01`
  * `2024-09-01`
* Added support for volume mount of the storage folder [#53](https://github.com/Azure/AppConfiguration-Emulator/pull/53)


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