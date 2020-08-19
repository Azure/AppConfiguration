# Feature Management Sample

This sample shows you how to create feature flags and integrate feature filters in a Java application using Azure App Configuration.

## Connect to App Configuration Store

1. In the Azure portal, navigate to your App Configuration store and select **Settings** > **Access keys** from the sidebar. Select the Read-only keys tab and copy the value of the primary connection string.
1. Add the primary connection string as an environment variable using the variable name `APP_CONFIGURATION_CONNECTION_STRING`.

    ```cmd
    setx ConnectionString "connection-string-of-your-app-configuration-store"
    ```

**Note**: Make sure to restart your terminal/IDE to load the new Environment Variable.

## Creating a Feature Flag

In the Azure portal navigate to your configuration store. Select **Feature manager** > **+Add** from the sidebar, and create a feature flag called `Beta`. Leave the `Label` and `Description` fields undefined. Select **Apply** to save the new feature flag.

## Usage

This sample is an On/Off feature flag that switches between returning the messages "Hello Word" and "Bye World".

To add feature filters to this sample, update the `Beta` feature flag. Navigate to **Feature manager** and on your `Beta` flag click **...** then **Edit**.
Click **Add Filter** and add a filter with Key value **percentageFilter**. Click on the **...** and select **Edit parameters**. Add a parameter called **percentage-filter-setting** with a value of **"50"**. This will result in the feature being randomly either on or off, with a 50 percent probability for each.

Another built in filter is `TimeWindowFilter`. This filter returns true only if either; your current time is between the start and end date, after the start date, or before the end date. Integrate the `TimeWIndowFilter` by adding it to the `featureFilters HashMap` in *HelloWorld.java*. 

```Java
featureFilters.put("TimeWindowFilter", new TimeWindowFilter());
```

In *FilterParameters.java*, update the `TimeWindowFilter` parameters. Aad the start and/or end dates to your feature filter. For example, Start `"Wed, 01 May 2019 13:59:59 GMT"` and End `"Mon, 01 July 2019 00:00:00 GMT"`.
Additionally you can create your own feature flag filters by implementing the class `FeatureFilter.java` and following the same process used to add the `TimeWindowFilter`.

### Execute

```terminal
mvn clean install
java -jar target/feature-management/feature-management.jar
```