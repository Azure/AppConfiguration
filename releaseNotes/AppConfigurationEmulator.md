# Azure App Configuration Emulator

[Image][image] | [Source code][source_code] | [Samples][samples]

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