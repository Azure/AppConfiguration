# Azure App Configuration Examples

## .NET Samples

### [AI Chat App](./DotNetCore/ChatApp)

This example showcases a .NET console application that retrieves chat responses from Azure OpenAI. It demonstrates how to configure chat completion using AI Configuration from Azure App Configuration, enabling rapid prompt iteration and frequent tuning of model parameters—without requiring application restarts, rebuilds, or redeployments.

### [Azure Functions (Isolated worker model)](./DotNetCore/AzureFunctions/FunctionAppIsolated)

This example showcases a .NET isolated worker model Function App, which operates out-of-process in Azure Functions. It demonstrates how to enable dynamic configuration and utilize feature flags from App Configuration. Additionally, it illustrates how to leverage the App Configuration references feature in Azure Functions to manage trigger parameters within App Configuration.

### [Azure Functions (In-process model)](./DotNetCore/AzureFunctions/FunctionAppInProcess)

This example showcases a .NET class library Function App, which runs in-process with the Azure Functions runtime. It demonstrates how to enable dynamic configuration and utilize feature flags from App Configuration. Additionally, it illustrates how to leverage App Configuration for a queue-triggered function, with the trigger settings stored in App Configuration.

### [Console App](./DotNetCore/ConsoleApplication)

This example demonstrates how to enable dynamic configuration from App Configuration in a .NET console app using the minimal project template style.

### [Web App](./DotNetCore/WebDemo)

This example is an ASP.NET Core web app, which uses the minimal project template style. It demonstrates how to enable dynamic configuration and use feature flags from App Configuration.

### [WebDemoWithEventHub](./DotNetCore/WebDemoWithEventHub/WebDemoWithEventHub)

This ASP.NET Core app demonstrates how to enable dynamic configuration from App Configuration using the *push model*. It uses Event Hub to consume push notifications from Event Grid triggered by changes made in App Configuration.

### [WebJobHelloWorld](./DotNetCore/WebJobs/WebJobHelloWorld)

This example demonstrates how to enable dynamic configuration from App Configuration in a Web Job app written in .NET Core.

### [MVC Web App (.NET Framework)](./DotNetFramework/WebDemo)

This ASP.NET web application is a .NET Framework MVC 5 app. It leverages the [configuration builder](https://www.nuget.org/packages/Microsoft.Configuration.ConfigurationBuilders.AzureAppConfiguration/) for App Configuration to load configuration to App Settings and consumes from the `ConfigurationManager`. As is the design of the .NET Framework App Settings, the configuration will only be updated upon app restart.

### [WebForm App (.NET Framework)](./DotNetFramework/WebFormApp)

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

## Go Samples

### [AI Chat App](./Go/ChatApp/)

This example showcases a Go console application that retrieves chat responses from Azure OpenAI. It demonstrates how to configure chat completion using AI Configuration from Azure App Configuration, enabling rapid prompt iteration and frequent tuning of model parameters—without requiring application restarts, rebuilds, or redeployments.

### [Console app](./Go/ConsoleApp/)

This example demonstrates how to enable dynamic configuration from App Configuration in a Go console application.

### [Gin web app](./Go/WebApp/)

This example demonstrates how to enable dynamic configuration and use feature flags from App Configuration in a web application built with the Gin framework.

