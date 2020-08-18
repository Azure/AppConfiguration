# Feature Management Sample

This sample shows how to use Azure App Configuration Feature Flags with just Java. To use this sample update `HelloWorld.java` with your Connection String. Then create a feature flag in your configuration store. For this sample a basic on/off feature flag called Beta is needed. Switching between On/Off in your config store will switch between Hello World/Bye World.

This example can be updated to use feature filters by adding one to your Beta feature. Set the Key value of the filter to be percentageFilter and add a parameter called percentage-filter-setting with a value of 50. This will result in the feature being randomly either on or off 50 percent of the time.

Another built in filter is `TimeWindowFilter`, which will return true only if either; between the start and end date, after the start date, or before the end date. This can be done by adding the `TimeWIndowFilter` to the featureFilters `HashMap` in `HelloWorld.java`. Then adding the start and/or end dates to your feature. The `TimeWindowFilter` parameters can be found in `FilterParameters.java`.

Additionally you can create your own filters by implementing the class `FeatureFilter.java` and following the same process used to add the `TimeWindowFilter`.

To run:

```terminal
mvn clean install
java -jar target/feature-management/feature-management.jar
```