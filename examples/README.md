# Azure App Configuration Examples

## DotNet Samples

### [Azure Functions (In-process)](./DotNetCore/AzureFunction/FunctionApp)

This example is a .NET class library Function App, which runs in-process with the runtime of Azure Functions. It demonstrates how to enable dynamic configuration and use feature flags from App Configuration. It also shows how to leverage App Configuration for a queue triggered function with the trigger settings stored in App Configuration.

### [Azure Functions (Out-of-process)](./DotNetCore/AzureFunction/FunctionAppIsolatedMode)

This example is a .NET isolated process Function App, which runs out-of-process in Azure Functions. It demonstrates how to enable dynamic configuration and use feature flags from App Configuration.

### [ConsoleApplication](./DotNetCore/ConsoleApplication)

This example demonstrates how to enable dynamic configuration from App Configuration in a console app written in .NET Core.

### [WebDemo (.NET Core)](./DotNetCore/WebDemo)

This ASP.NET Core app demonstrates how to enable dynamic configuration from App Configuration using the *poll model*. It uses Azure AD/Managed Identity to authenticate with App Configuration.

### [WebDemo (.NET 6)](./DotNetCore/WebDemoNet6)

This example is an ASP.NET Core app, which uses the modernized project template introduced in .NET 6. It demonstrates how to enable dynamic configuration and use feature flags from App Configuration.

### [WebDemoWithEventHub](./DotNetCore/WebDemoWithEventHub/WebDemoWithEventHub)

This ASP.NET Core app demonstrates how to enable dynamic configuration from App Configuration using the *push model*. It uses Event Hub to consume push notifications from Event Grid triggered by changes made in App Configuration.

### [WebJobHelloWorld](./DotNetCore/WebJobs/WebJobHelloWorld)

This example demonstrates how to enable dynamic configuration from App Configuration in a Web Job app written in .NET Core.

### [WebDemo (.NET Framework)](./DotNetFramework/WebDemo)

This ASP.NET web application is a .NET Framework MVC 5 app. It leverages the [configuration builder](https://www.nuget.org/packages/Microsoft.Configuration.ConfigurationBuilders.AzureAppConfiguration/) for App Configuration to load configuration to App Settings and consumes from the `ConfigurationManager`. As is the design of the .NET Framework App Settings, the configuration will only be updated upon app restart.

### [WebFormApp (.NET Framework)](./DotNetFramework/WebFormApp)

This ASP.NET web application is a .NET Framework Web Forms app. It demonstrates how to leverage the App Configuration [.NET Standard provider library](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.AzureAppConfiguration/) to achieve dynamic configuration and control feature launches with feature flags. The same technique applies to .NET Framework MVC apps.

## Spring Samples

Contains multiple Azure App Configuration Spring boot sample projects.

### [azure-spring-cloud-appconfiguration-config-sample](https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/appconfiguration/azure-spring-cloud-appconfiguration-config/azure-spring-cloud-appconfiguration-config-sample)

This example shows how to use the basic features of Azure App Configuration in a in Java Spring cloud console application.

### [azure-spring-cloud-starter-appconfiguration-config-sample](https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/appconfiguration/azure-spring-cloud-starter-appconfiguration-config/azure-spring-cloud-starter-appconfiguration-config-sample)

This example shows how to dynamically update configuration properties in a Spring Web application.

### [azure-spring-cloud-appconfiguration-config-convert-sample](https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/appconfiguration/azure-spring-cloud-appconfiguration-config/azure-spring-cloud-appconfiguration-config-convert-sample)

This example shows how to convert an existing Java Spring boot application to use Azure App Configuration.

### [azure-spring-cloud-feature-management-sample](https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/appconfiguration/azure-spring-cloud-feature-management/azure-spring-cloud-feature-management-sample)

This example shows how to use feature flags in a Java Spring boot application.

### [azure-spring-cloud-feature-management-web-sample](https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/appconfiguration/azure-spring-cloud-feature-management-web/azure-spring-cloud-feature-management-web-sample)

This example shows how to use feature flags in a Spring Web application.

## Python Samples

### [python-django-webapp-sample](./Python/python-django-webapp-sample/)

This example shows how to use the Azure App Configuration Python Provider in your python Django app.

### [python-flask-webapp-sample](./Python/python-flask-webapp-sample/)

This example shows how to use the Azure App Configuration Python Provider in your python Flask app.