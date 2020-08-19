# Feature Management Sample

This sample shows how to use Azure App Configuration Feature Flags with just Java.

## Connect to App Configuration Store

1. In the App Configuration portal for your config store, select `Access keys` from the sidebar. Select the Read-only keys tab. Copy the value of the primary connection string.
1. Add the primary connection string as an environment variable using the variable name `APP_CONFIGURATION_CONNECTION_STRING`.

**Note**: Make sure to restart your terminal/IDE to load the new Environment Variable.

## Creating a Feature Flag

1. In the App Configuration portal for your config store, select `Feature Manger` from the sidebar.
1. Select `+Add`
1. In the Key field enter `Beta`. Leave Label and Description blank. And select Apply.

## Usage

The default usage of this sample is an On/Off feature flag that switches between Hello Word/Bye World.

This example can be updated to use feature filters by adding one to your Beta feature. Set the Key value of the filter to be percentageFilter and add a parameter called percentage-filter-setting with a value of `"50"`. This will result in the feature being randomly either on or off 50 percent of the time.

Another built in filter is `TimeWindowFilter`, which will return true only if either; between the start and end date, after the start date, or before the end date. This can be done by adding the `TimeWIndowFilter` to the featureFilters `HashMap` in `HelloWorld.java`. Then adding the start and/or end dates to your feature filter, example; Start `"Wed, 01 May 2019 13:59:59 GMT"` and End `"Mon, 01 July 2019 00:00:00 GMT"`. The `TimeWindowFilter` parameters can be found in `FilterParameters.java`.

```Java
featureFilters.put("TimeWindowFilter", new TimeWindowFilter());
```

Additionally you can create your own filters by implementing the class `FeatureFilter.java` and following the same process used to add the `TimeWindowFilter`.

To run:

```terminal
mvn clean install
java -jar target/feature-management/feature-management.jar
```