## Azure App Configuration Push Extension release notes
Azure App Configuration Push extension for Azure DevOps pipeline can be installed from the [Azure DevOps marketplace](https://marketplace.visualstudio.com/items?itemName=AzureAppConfiguration.azure-app-configuration-task-push). The version information can be found in the console when the task is executed:
![sample](pictures/AzureDevOpsPushExtensionVersionSample.PNG)

### v1.3.4 - March, 12 2021
* Added the capability to use managed identity based authentication.

### v1.2.4 - February, 16 2021
* Fixed incorrect tooltip help messages for task parameters in task editor UI [#447](https://github.com/Azure/AppConfiguration/issues/447) 
* Updated error message when certificate based authentication is used to indicate that it is not supported.
* Fixed a bug when parsing yaml that caused objects to be flattened incorrectly.

### v1.1.4 - December, 10 2020
* Updated the readme 

### v1.1.3 - December, 10 2020
* Improved error message when required parameters are not provided. 

### v1.0.0 Preview - July, 19 2020
* Initial version.
* Added Azure App Configuration Push extension to [Azure DevOps Marketplace](https://marketplace.visualstudio.com/items?itemName=AzureAppConfiguration.azure-app-configuration-task-push).
