# WebJob Hello World Sample

This sample demonstrates how to build a .NET 8 WebJobs application that uses Azure App Configuration to store and retrieve configurations.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Azure subscription with an App Configuration instance
- Azure Storage account

## How it works

This sample WebJob connects to Azure App Configuration to retrieve settings such as the Azure Storage connection string and queue name. It then listens to the specified queue for messages and logs them.

## Getting Started

1. Set the environment variable `ConnectionString` to your Azure App Configuration connection string:

   ```bash
   # Windows
   set ConnectionString=<your_connection_string>
   
   # Linux/macOS
   export ConnectionString=<your_connection_string>
   ```

2. Make sure you have the following configuration values set in your Azure App Configuration instance:
   - `WebJob:AzureWebJobsStorage` - Your Azure Storage connection string
   - `WebJob:QueueName` - The name of the queue to monitor

3. Run the application:

   ```bash
   dotnet run
   ```

## Dependencies

- Microsoft.Azure.WebJobs
- Microsoft.Azure.WebJobs.Extensions
- Microsoft.Azure.WebJobs.Extensions.Storage
- Microsoft.Extensions.Configuration.AzureAppConfiguration