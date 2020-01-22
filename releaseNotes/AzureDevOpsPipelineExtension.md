## Azure App Configuration Extension release notes

The Azure App Configuration extension in Azure DevOps pipeline has been published to [Azure DevOps marketplace](https://marketplace.visualstudio.com/items?itemName=AzureAppConfiguration.azure-app-configuration-task&ssr=false#overview). The version of the Azure App Configuration task being used will print to the console when the task is executed: ![sample](pictures/AzureDevOpsExtensionVersionSample.PNG)

### v2.0.1 - January, 21 2020
  * **Breaking change**: supported [Azure Active Directory authentication](https://github.com/Azure/AppConfiguration/blob/master/docs/REST/authorization/aad.md) of fetching key-values from App Configuration instance.
    * Change Service Connections'(Service Principals) role assignment to `Azure App Configuration Data Owner` or `Azure App Configuration Data Reader` role to grant pipeline Service Connection access to App Configuration access.

### v1.6.8 - December, 02 2019
  * Fixed the issue that the `App Configuration name` dropdown is not populated if the App Configuration pipeline task is added to the pipeline YAML file via the `assistant` UI of Tasks. [#202](https://github.com/Azure/AppConfiguration/issues/202)

### v1.6.7 - September, 04 2019
  * Updated to use the authorization header syntax that is compliant with the REST API spec.
  * Included API version for all requests sent to Azure App Configuration.

### v1.6.6 - June, 24 2019
* Improved [homepage documentation](https://marketplace.visualstudio.com/items?itemName=AzureAppConfiguration.azure-app-configuration-task):
  * Added a link to this release notes.
  * Added an example of how to consume fetched key-values.

### v1.6.3 - June, 03 2019
* Supported auto-populated `App Configuration name` dropdown from a textbox.
* Reset environment variables with invalid keys now prints a masked value, `****`, instead of the actual value of the key-value.

### v1.4.48 - May, 06 2019
* Initial version.
* Added Azure App Configuration Extension to [Azure DevOps Marketplace](https://marketplace.visualstudio.com/).
* Integrated with pipeline framework control `Azure subscription` so that user could use existing `Connection Endpoint` to auth with Azure App Configuration instance.
* Supported `Key filter` and `Label` to query matched key-values.


