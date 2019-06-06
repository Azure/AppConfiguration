## Azure App Configuration Extension release notes

The Azure App Configuration extension in Azure DevOps pipeline has been published to [Azure DevOps marketplace](https://marketplace.visualstudio.com/items?itemName=AzureAppConfiguration.azure-app-configuration-task&ssr=false#overview). The version of the Azure App Configuration task being used will print to the console when the task is executed.: ![sample](pictures/AzureDevOpsExtensionVersionSample.PNG)

### 1.6.3
* Support auto-populated `App Configuration name` dropdown from a textbox.
* Attempting to set environment variables with invalid keys now prints a masked value, `****`, instead of the actual value of the key-value.

### Initial version
* Add Azure App Configuration Extension to [Azure DevOps Marketplace](https://marketplace.visualstudio.com/).
* Integrate with pipeline framework control `Azure subscription` so that user could use existing `Connection Endpoint` to auth with Azure App Configuration instance.
* Support `Key filter` and `Label` to query matched key-values.


