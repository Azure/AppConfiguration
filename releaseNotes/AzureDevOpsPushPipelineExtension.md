## Azure App Configuration Push Extension release notes

> **DEPRECATION NOTICE**  
> This pipeline task is now available as a built-in task for Azure Pipelines under the new name, **Azure App Configuration Import**. This means you no longer need to install it from Marketplace. While you can continue using the Marketplace version, please note that it will no longer receive updates. Transitioning to the new built-in task requires minimal effort, as it supports all the features and functionalities of the Marketplace version. In most cases, it is as simple as updating the task name and version in your pipeline.

>
> Upgrade to [Azure App Configuration Import](https://learn.microsoft.com/azure/azure-app-configuration/azure-pipeline-import-task) today to access the latest features and improvements.

Azure App Configuration Push extension for Azure DevOps pipeline can be installed from the [Azure DevOps marketplace](https://marketplace.visualstudio.com/items?itemName=AzureAppConfiguration.azure-app-configuration-task-push). The version information can be found in the console when the task is executed:
![sample](pictures/AzureDevOpsPushExtensionVersionSample.PNG)

### v7.0.0 - July, 3, 2024
- Added support for Node.js 20. This pipeline task now supports both Node.js 16 and Node.js 20.

### v6.4.0 - November, 20 2023
* Added support for a feature that allows users to add a prefix to the key name of feature flags. This will provide more flexibility and better organization of feature flags. [#810](https://github.com/Azure/AppConfiguration/issues/810).

### v6.3.0 - October, 13 2023
* Added capability to use workload identity federation for authentication.

### v5.0.0 - February, 02 2023
**Breaking Changes**
  - Updated the task to require Node.js 16. It previously required 10.
  - Updated the minimum supported azure pipeline agent version to [2.206.1](https://github.com/microsoft/azure-pipelines-agent/releases/tag/v2.206.1) or later. Previously it was [2.144.0](https://github.com/microsoft/azure-pipelines-agent/releases/tag/v2.144.0).
  - The behavior when importing configurations with JSON content types such as `application/json` or `application/vnd.mycustomresource+json` has changed. The properties in the provided configuration are serialized as JSON to respect the provided content type. Settings that are expected to end up as JSON serialized strings in App Configuration should now be specified as JSON objects. Additionally, the depth property should also be adjusted to match the expected depth the JSON structure will be flattened to.

    **Before**
    ```
    {  
      "app": "{\"uri\":\"https://keyvault.vault.azure.net/secrets/secret\"}"
    }

    task: AzureAppConfigurationPush@4
    inputs:
      azureSubscription: '<subscription>'
      AppConfigurationEndpoint: '<store endpoint>'
      ....
      ContentType: 'application/vnd.microsoft.appconfig.keyvaultref+json;charset=utf-8'
    ```

    **After**
    ```
    {  
      "app": {
          "uri": "https://keyvault.vault.azure.net/secrets/secret " 
        }
    }

    task: AzureAppConfigurationPush@5
    inputs:
      azureSubscription: '<subscription>'
      AppConfigurationEndpoint: '<store endpoint>'
      Depth: 1
      ....
      ContentType: 'application/vnd.microsoft.appconfig.keyvaultref+json;charset=utf-8'

    ```

  - Feature flag import now requires adherence to the [feature management schema](https://github.com/microsoft/FeatureManagement-Dotnet/blob/release/v3/docs/schemas/FeatureManagement.v1.0.0.json). Settings with feature flag prefix and content type will fail to import

### v4.4.0 - November, 15 2022
* Added ImportMode option support for KVSet profile.
* Added Strict support for KVSet profile. When using the KVSet profile and setting Strict to true any key-values in the store that are not included in the configuration file will be deleted.

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
