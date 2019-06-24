# Microsoft.FeatureManagement and Microsoft.FeatureManagement.AspNetCore

## 1.0.0-preview-009000001-1251 - June 20, 2019

* Renamed 'Microsoft.FeatureManagment.FeatureAttribute' to 'Microsoft.FeatureManagment.Mvc.FeatureGateAttribute'.
* Enhanced FeatureGateAttribute to allow specifying whether 'any' or 'all' features need to be enabled.
* Enhanced feature tag helper to allow for multiple features, any/all requirement, and negated logic.
* Added `IFeatureManagementBuilder.AddSessionManager` to enhance discoverability for providing a custom feature session manager.
  * Previous approach was `IServiceCollection.AddSingleton<ISessionManager>(MySessionManager)`
