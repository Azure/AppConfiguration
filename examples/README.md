# Azure App Configuration Examples

## [ConfigurationStoreBackup](./ConfigurationStoreBackup)
This Azure Functions App demonstrates how to backup configuration data from one App Configuration store to another store. It uses Event Grid for change notifications received from App Configuration and saves them in an Azure Storage queue. The app wakes up every 10 minutes to backup the data if there are any changes.

## [Azure Functions (In-process)](./DotNetCore/AzureFunction/FunctionApp)
This example is a .NET class library Function App, which runs in-process with the runtime of Azure Functions. It demonstrates how to enable dynamic configuration and use feature flags from App Configuration. It also shows how to leverage App Configuration for a queue triggered function with the trigger settings stored in App Configuration.

## [Azure Functions (Out-of-process)](./DotNetCore/AzureFunction/FunctionAppIsolatedMode)
This example is a .NET isolated process Function App, which runs out-of-process in Azure Functions. It demonstrates how to enable dynamic configuration and use feature flags from App Configuration.

## [ConsoleApplication](./DotNetCore/ConsoleApplication)
This example demonstrates how to enable dynamic configuration from App Configuration in a console app written in .NET Core.

## [WebDemo (.NET Core)](./DotNetCore/WebDemo)
This ASP.NET Core app demonstrates how to enable dynamic configuration from App Configuration using the *poll model*. It uses Azure AD/Managed Identity to authenticate with App Configuration.

## [WebDemoWithEventHub](./DotNetCore/WebDemoWithEventHub/WebDemoWithEventHub)
This ASP.NET Core app demonstrates how to enable dynamic configuration from App Configuration using the *push model*. It uses Event Hub to consume push notifications from Event Grid triggered by changes made in App Configuration.

## [WebJobHelloWorld](./DotNetCore/WebJobs/WebJobHelloWorld)
This example demonstrates how to enable dynamic configuration from App Configuration in a Web Job app written in .NET Core.

## [WebDemo (.NET Framework)](./DotNetFramework/WebDemo)
This ASP.NET web application is a .NET Framework MVC 5 app. It leverages the [configuration builder](https://www.nuget.org/packages/Microsoft.Configuration.ConfigurationBuilders.AzureAppConfiguration/) for App Configuration to load configuration to App Settings and consumes from the `ConfigurationManager`. As is the design of the .NET Framework App Settings, the configuration will only be updated upon app restart.

## [WebFormApp](./DotNetFramework/WebFormApp)
This ASP.NET web application is a .NET Framework Web Forms app. It demonstrates how to leverage the App Configuration [.NET Standard provider library](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.AzureAppConfiguration/) to achieve dynamic configuration and control feature launches with feature flags. The same technique applies to .NET Framework MVC apps.
