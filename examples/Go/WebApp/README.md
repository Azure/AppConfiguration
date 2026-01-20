# Gin Feature Flags Web App

This is a Gin web application using a feature flag in Azure App Configuration to dynamically control the availability of a new web page without restarting or redeploying it.

## Prerequisites

- An Azure account with an active subscription
- An Azure App Configuration store
- A feature flag named "Beta" in your App Configuration store
- Go 1.23 or later

## Running the Example

1. **Create a feature flag in Azure App Configuration:**

   Add a feature flag called *Beta* to the App Configuration store and leave **Label** and **Description** with their default values. For more information about how to add feature flags to a store using the Azure portal or the CLI, go to [Create a feature flag](https://learn.microsoft.com/azure/azure-app-configuration/manage-feature-flags?tabs=azure-portal#create-a-feature-flag).
   
2. **Set environment variable:**

   **Windows PowerShell:**
   ```powershell
   $env:AZURE_APPCONFIG_CONNECTION_STRING = "your-connection-string"
   ```

   **Windows Command Prompt:**
   ```cmd
   setx AZURE_APPCONFIG_CONNECTION_STRING "your-connection-string"
   ```

   **Linux/macOS:**
   ```bash
   export AZURE_APPCONFIG_CONNECTION_STRING="your-connection-string"
   ```

## Running the Application

```bash
go run main.go
```

Open http://localhost:8080 in your browser.

## Testing the Feature Flag

1. **Start the application** - Beta menu item should be hidden (feature disabled)
2. **Go to Azure portal** → App Configuration → Feature manager
3. **Enable the "Beta" feature flag**
4. **Wait up to 30 seconds** and refresh the page
5. **Observe the Beta menu item** appears in navigation
6. **Click the Beta menu** to access the Beta page
7. **Disable the flag again** and test that `/beta` returns 404