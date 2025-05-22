# WebJob Hello World Sample

This sample demonstrates how to build a .NET 8 WebJobs application that uses Azure App Configuration to store and retrieve configurations.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Azure subscription with an App Configuration instance
- Azure Storage account
- Azure identity with permissions to access the App Configuration instance

## How it works

This sample WebJob connects to Azure App Configuration using DefaultAzureCredential to retrieve settings such as the Azure Storage connection string and queue name. It then listens to the specified queue for messages and logs them.

## Getting Started

1. Set the environment variable `AppConfigurationEndpoint` to your Azure App Configuration endpoint URI:

   ```bash
   # Windows
   set AppConfigurationEndpoint=https://<your-appconfig-name>.azconfig.io
   
   # Linux/macOS
   export AppConfigurationEndpoint=https://<your-appconfig-name>.azconfig.io
   ```

2. Make sure you have the following configuration values set in your Azure App Configuration instance:
   - `WebJob:AzureWebJobsStorage` - Your Azure Storage connection string
   - `WebJob:QueueName` - The name of the queue to monitor

3. Ensure you have proper authentication set up for DefaultAzureCredential. This could be:
   - Azure CLI login
   - Visual Studio/Visual Studio Code authentication
   - Environment variables for service principal credentials
   - Managed Identity (when deployed to Azure)

4. Run the application:

   ```bash
   dotnet run
   ```

## Dependencies

- Azure.Identity
- Microsoft.Azure.WebJobs
- Microsoft.Azure.WebJobs.Extensions
- Microsoft.Azure.WebJobs.Extensions.Storage.Queues
- Microsoft.Extensions.Configuration.AzureAppConfiguration