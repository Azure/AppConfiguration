# azure-spring-cloud-feature-management

[Source code][source_code] | [Package (Maven)][package] | [Samples][samples] | [Product documentation][docs]

# azure-spring-cloud-feature-management-web

[Source code web ][source_code_web] | [Package (Maven) web][package_web] | [Samples web][samples_web] | [Product documentation][docs]

## 2.0.0 - June 22, 2021

* The Feature Management libraries have have renamed to match the structure and naming convention in the azure-sdk-for-java.
* Targeting, a new built in Feature Filter type.
  * Support for rolling out features to a target audience has been added through built in feature filters.
  * Targeting enables developers to progressively roll out features to a target audience that can be increased gradually.  For more information on the concept of targeting and how to use this new feature take a look at the project's [readme](https://github.com/Azure/azure-sdk-for-java/tree/master/sdk/appconfiguration/azure-spring-cloud-feature-management#targetingfilter).
* TimeWindowFilter now supports ISO 8601 Dates.

<!-- LINKS -->
[docs]: https://github.com/Azure/azure-sdk-for-java/tree/master/sdk/appconfiguration/azure-spring-cloud-feature-management
[package]: https://mvnrepository.com/artifact/com.azure.spring/azure-spring-cloud-feature-management
[samples]: https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/appconfiguration/feature-management-sample
[source_code]: https://github.com/Azure/azure-sdk-for-java/tree/master/sdk/appconfiguration/azure-spring-cloud-feature-management

[package_web]: https://mvnrepository.com/artifact/com.azure.spring/azure-spring-cloud-feature-management-web
[samples_web]: https://github.com/Azure-Samples/azure-spring-boot-samples/tree/main/appconfiguration/feature-management-web-sample
[source_code_web]: https://github.com/Azure/azure-sdk-for-java/tree/master/sdk/appconfiguration/azure-spring-cloud-feature-management-web
