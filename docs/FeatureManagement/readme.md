# Azure App Configuration Feature Management

Azure App Configuration provides feature management support through the creation of **Feature Flags**. Feature flags consist of a unique identifier and an enabled state that can be evaluated dynamically. These flags are stored in Azure App Configuration as key-values with a common set of conventions so that they can be identified among other non feature flag key-values.

## Conventions

All feature flag key-values are prefixed with ".appconfig.featureflag/" to distinguish among other key-values. These feature flags also use a custom content type to describe the structure of the key-value's value. These two conventions allow clients to identify feature flags, and safely parse them into meaningful feature flag parameters when querying key-values from Azure App Configuration.

The following are the currently used content types in Azure App Configuration feature management:

[application/vnd.microsoft.appconfig.ff+json;charset=utf-8](./FeatureFlag.v1.0.0.schema.json)


