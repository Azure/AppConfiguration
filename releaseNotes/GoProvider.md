# Azure App Configuration Go Provider

[Source code][source_code] | [Package][package] | [Samples][samples]

## 1.0.0-beta.2 - May 29, 2025

### Enhancement

* Added support for sentinel key-based dynamic configuration refresh from Azure App Configuration.
* Added support for monitoring all selected key-values for dynamic configuration refresh from Azure App Configuration.
* Added support for periodically resolving Key Vault references to fetch the latest version of secret.

## 1.0.0-beta.1 - April 10, 2025

Added support for
- authentication with connection string or Microsoft EntraID
- loading and composing configuration with key-value selectors
- key prefix trimming
- Key Vault reference resolution
- strongly-typed struct data binding 
- returning key-values as raw json bytes

[source_code]: https://github.com/Azure/AppConfiguration-GoProvider
[package]: https://pkg.go.dev/github.com/Azure/AppConfiguration-GoProvider/azureappconfiguration
[samples]: https://github.com/Azure/AppConfiguration-GoProvider/tree/main/example