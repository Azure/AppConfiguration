## Azure App Configuration Push Extension release notes
Azure App Configuration Push extension for Azure DevOps pipeline can be installed from the [Azure DevOps marketplace](https://marketplace.visualstudio.com/items?itemName=AzureAppConfiguration.azure-app-configuration-task-push). The version information can be found in the console when the task is executed:
![sample](pictures/AzureDevOpsPushExtensionVersionSample.PNG)

### v4.4.0 - November, 15 2022
* Added ImportMode option support for KVSet profile.
* Added Strict support for KVSet profile. With the added support of Strict for KVSet profile, when Strict is set to true any key-values in the store that are not included in the configuration file will be deleted.

### v3.4.0 - October, 19 2022
* Added ImportMode option, ImportMode is only supported for Default profile.
* Added DryRun option.

### v3.3.0 - July, 13 2022
* Added KVSet file content profile support.
* Fixed a bug that caused an unexpected error "Cannot convert undefined or null to object" when pushing properties from .properties file.

### v3.2.0 - March, 02 2022
Fixed a bug that caused an invalid_client error when using certificate-based authentication [#608](https://github.com/Azure/AppConfiguration/issues/608).

### v3.1.0 - January, 07 2022
* **Breaking change**: 
With the added support of sovereign clouds such as Azure Government and Azure China, the task inputs were updated.

  **Before**: 
  Task input was *App Configuration Name*

  **After**:
  Task input was updated to *App Configuration endpoint*. The *App Configuration endpoint* can be gotten from the App Configuration store overview page.

### v2.0.0 - October, 12 2021
* Upgraded task to use Node 10. It previously used Node 6.
* Added support for importing feature flags from json/yaml files.
* Added capability to detect configuration file encoding format and display appropriate error message [#550](https://github.com/Azure/AppConfiguration/issues/550).

### v1.4.4 - July, 22 2021
* The Azure App Configuration Push pipeline task is now generally available.
* Added the capability to use certificate based authentication.
* Added logs that show more details with reference to parameters used to run the task.
* Fixed a bug that caused the warning "Can\'t find loc string for key:CouldNotFetchAccessTokenforAzureStatusCode causes app config azdo task to fail" [#520](https://github.com/Azure/AppConfiguration/issues/520).
* Added more information to the error message "Failed to parse" [#513](https://github.com/Azure/AppConfiguration/issues/513).

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
