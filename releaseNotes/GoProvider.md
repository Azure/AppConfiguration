# Azure App Configuration Go Provider

[Source code][source_code] | [Package][package] | [Samples][samples]

## 1.0.0-beta.2 - May 29, 2025

### New Features

* Added support for dynamic configuration so the selected key-values will be automatically updated when data is changed in Azure App Configuration.
* Added support for periodically resolving Key Vault references to fetch latest version secret.

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