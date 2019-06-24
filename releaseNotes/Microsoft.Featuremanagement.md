# Microsoft.FeatureManagement and Microsoft.FeatureManagement.AspNetCore

## 1.0.0-preview-009000001-1251 - June 20, 2019

* Renamed 'Microsoft.FeatureManagment.FeatureAttribute' to 'Microsoft.FeatureManagment.Mvc.FeatureGateAttribute'.
* Enhanced FeatureGateAttribute to allow specifying whether 'any' or 'all' features need to be enabled.
* Enhanced feature tag helper to allow for multiple features, any/all requirement, and negated logic.
* Added AddSessionManager to IFeatureManagementBuilder.
