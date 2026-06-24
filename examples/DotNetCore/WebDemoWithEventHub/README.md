# Integrate Azure App Configuration with Azure Event Hub
You can use Azure App Configuration's Event Grid Subscription with Azure Event Hub to have your application notified (that is, receive push notifications) on any changes to the Config Store.

---

**Requirements**

To run the demo application, you'll need the following Azure Resources in order to receive notifications for Config Store changes:

- Event Hub
  - Azure Event Hub Namespace
  - Azure Event Hub
- Azure Storage
  - Azure Storage Account
  - Azure Storage Container
- Azure Key Vault
- Azure App Configuration Store

---

## Create and wire-up Azure Event Hub

### Create EventHub Namespace and EventHub

Before we can create an Event Hub, we would need an Azure Event Hub Namespace. Create one as shown in the screenshot below:

![CreateEventHubNamespace](./images/1%20-%20CreateEventHubNamespace.png)

After the namespace has been created, create a new Azure EventHub as shown:
![CreateEventHub1](./images/2%20-%20CreateEventHub1.png)

While creating the EventHub, make sure to turn on *Capture* (to capture events to be delivered to the application), and select either a new Storage Container or an existing one, as shown below:
![CreateEventHub2](./images/3%20-%20CreateEventHub2%20-%20with%20capture%20events.png)

Now, create an Access Policy for the EventHub with *Send* and *Listen* policies at a minimum:
![CreateAccessPolicy](./images/4%20-%20CreateAccessPolicyEventHub.png)

After the Access Policy for the EventHub has been created, make sure to note down the Connection String, name of the EventHub and the EventHub Namespace. These will be needed later (also, note down the Name and Connection String for the Azure Storage Container).
![ViewConnectionString](./images/5%20-%20ViewAndStoreEventHubConnectionStringInKeyVault.png)

### Create EventGridSubscription

Once you have created the EventHub, you can wire it up with App Configuration's EventGridSubscriptions to capture, store and notify your application on any Config Store changes.

Create a new Azure resource of Type Event Subscription as shown (make sure to select *App Configuration* as the Topic Type, and then select the Config Store you wish to listen to):
![CreateSubscription1](./images/6.1%20-%20CreateEventHubSubscription.png)

Next, click on *Select an endpoint* to select the Event Hub instance, as shown:
![CreateSubscription2](./images/6.2%20-%20SelectEventHubCreated.png)

## Store Secrets in Azure KeyVault

Store the Connection Strings for the Event Hub and the Storage Container in an Azure KeyVault as shown:
![StoreSecretsInKeyVault](./images/7%20-%20CreateKeyVaultSecrets.png)

## Create Config Store values

In the Azure App Configuration Store, store the necessary settings for the Event Hub instance, the storage instance and the settings needed for the application.

In the demo app, for connecting to Event Hub, the following configurations are needed:

- WebDemo:EventHubConnection:EventHubConnectionString
- WebDemo:EventHubConnection:EventHubName
- WebDemo:EventHubConnection:EventHubNamespace
- WebDemo:EventHubConnection:StorageConnectionString
- WebDemo:EventHubConnection:StorageContainerName

The *WebDemo:EventHubConnection:EventHubConnectionString* and *WebDemo:EventHubConnection:StorageConnectionString* are KeyVault References created as shown:
![CreateStorageConnectionString](./images/8%20-%20CreateStorageConnectionStringKeyVaultRef.png)
![CreateEventHubConnectionString](./images/9%20-%20CreateEventHubConnectionStringKeyVaultRef.png)

Apart from the configurations for EventHub, the demo app also needs the following settings:

- WebDemo:Sentinel
- WebDemo:Settings:AppName
- WebDemo:Settings:BackgroundColor
- WebDemo:Settings:FontSize
- WebDemo:Settings:Messages
- WebDemo:Settings:RefreshRate
- WebDemo:Settings:Version
- WebDemo:Settings:Language

By the end, the Config Store should resemble something like this:
![AppConfigStoreAfter](./images/10%20-%20AppConfigStoreAfter.png)

## Register Refresher and Event Handler in the Application

In your application, register the *WebDemo:Sentinel* key to refresh the configuration on:
![RegisterChangeKey](./images/11%20-%20RegisterRefreshConfigKey.png)

In the event handler in EventHubService.cs, add the ``SetDirty()`` method to refresh the config store when the registered key changes:
![InEventHandler](./images/12%20-%20Refresher.SetDirtyInProcessEventHandler.png)

## Run the Application

### First run

On the first run, the app should run with the current state of the Config Store as shown:
![RunBeforeChange](./images/13%20-%20RunBeforeChange.png)

### Make changes

Change the value of *Messages*:
![ChangeMessage](./images/14.1%20-%20Change.png)

Change the value of *BackgroundColor*:
![ChangeColor](./images/14.2%20-%20Change.png)

And change the value of the Sentinel key:
![ChangeSentinel](./images/14.3%20-%20SentinelChange.png)

### Refresh the app window

Now, when you refresh the browser tab/window, you should see the new config values:
![RefreshPage](./images/15%20-%20RefreshPage.png)

*Please note that Event Hub notifications might not be instantaneous, and it might take a few seconds for the change to reflect*.
