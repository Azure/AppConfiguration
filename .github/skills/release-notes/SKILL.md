---
name: release-notes
description: "Author and update release notes for Azure App Configuration libraries and providers. USE FOR: write release notes, add release note entry, update release notes, draft release notes, new version release notes, format release notes. DO NOT USE FOR: generating changelogs from git history, writing internal engineering notes."
---

# Release Notes Authoring Skill

This skill guides the authoring of customer-facing release notes for Azure App Configuration libraries and providers. Release notes live in `releaseNotes/` and are the primary way customers learn about new versions.

## Key Principle

Release notes are for **customers**, not engineers. Every entry should help a customer decide whether to upgrade and understand what changed from their perspective.

## File Location & Naming

- All release notes go in `releaseNotes/` in this repository, even if the library has its own repo.
- One Markdown file per product (e.g., `MicrosoftExtensionsConfigurationAzureAppConfiguration.md`).
- If adding release notes for a **new product**, create a new file in `releaseNotes/`.

## File Structure

Each release notes file follows this structure:

```markdown
# Product Name

[Source code][source_code] | [Package (NuGet/npm/PyPI)][package] | [Samples][samples]

## {version} - {Month} {Day}, {Year}

### Breaking Changes

### Enhancements

### Bug Fixes

<!-- Reference links at bottom of file -->
[source_code]: https://...
[package]: https://...
[samples]: https://...
```

## Version & Date Format

- Use the format: `## {version} - {Month} {Day}, {Year}`
- Use the full month name, never abbreviations.
- Examples:
  - `## 7.0.0 - November 21, 2023`
  - `## 2.4.0 - February 17, 2026`
  - `## v1.5.0 - February 14, 2026` (Go uses the `v` prefix)
  - `## 8.6.0-preview - February 26, 2026` (preview/beta versions)
- Sort entries in **reverse chronological order** (newest first).

## Categories

Organize changes under these headings. Omit a category if it has no entries for that release.

### Breaking Changes
Reserved for changes that require customers to modify their code or configuration.

- **Always explain why** the breaking change was made.
- **Always explain how to migrate** from the old behavior to the new one.
- Example:
  > Removed `IConfigurationRefresher.SetDirty` API in favor of `IConfigurationRefresher.ProcessPushNotification` API for push-model based configuration refresh. Unlike the SetDirty method, the ProcessPushNotification method guarantees that all configuration changes up to the triggering event are loaded in the following configuration refresh. For more details on the ProcessPushNotification API, refer to [this tutorial](https://link).

### Enhancements
New features, capabilities, or improvements.

- Describe **what new scenarios are enabled** and how to take advantage of them.
- Don't just name a new API — explain what it lets customers do.
- Include brief code snippets when they help illustrate usage.
- Example:
  > Added health check integration for `Microsoft.Extensions.Diagnostic.HealthChecks`. You can call `AddAzureAppConfiguration` on `IHealthCheckBuilder` to register a health check for the Azure App Configuration provider. [#644](https://github.com/Azure/AppConfiguration-DotnetProvider/pull/644)
  >
  > ```cs
  > builder.Services
  >    .AddHealthChecks()
  >    .AddAzureAppConfiguration();
  > ```

### Bug Fixes
Fixes for issues customers may have encountered.

- **Describe the symptom**, not the implementation fix. Customers need to know if the bug affected them.
- Bad: "Added a call to `ConfigureAwait(false)` in `LoadKeyValuesRegisteredForRefresh`"
- Good: "Fixed a bug where ASP.NET Framework applications may fail to refresh their configuration."
- Example:
  > Fixed a bug where calls to `FeatureFlagOptions.Select` were ignored if they were followed by a call to either `AzureAppConfigurationOptions.Select` or `AzureAppConfigurationOptions.SelectSnapshot`. [#628](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/628)

## Linking to Issues and PRs

- **Always link** each change to a GitHub issue or PR.
- **Prefer linking to issues** over PRs — issues provide more context and already link to the PR.
- Use the format: `[#123](https://github.com/org/repo/issues/123)`
- Place the link at the end of the bullet point.
- Example:
  > Added the following new API for additional App Configuration geo-replication support. [#385](https://github.com/Azure/AppConfiguration-DotnetProvider/issues/385)

## Writing Style

- Use **past tense**: "Added support for…", "Fixed a bug where…", "Removed the API…"
- Write from the **customer's perspective**, not the developer's.
- Be concise but informative — one to two sentences per entry is typical.

## What to Exclude

Do not include changes that are not meaningful to customers:

- Internal code refactoring
- Telemetry additions or changes
- Test improvements
- CI/CD pipeline changes
- Dependency bumps that don't affect customer behavior (unless they fix a customer-facing issue)

## Special Cases

### Preview / Beta Releases
- Include preview and beta versions in the same file, interleaved chronologically.
- Use the full version string: `## 8.6.0-preview - February 26, 2026`
- Preview/beta releases are **cumulative** — each one includes all changes from previous previews in that series. List all changes in every preview entry; duplicates across preview entries are expected.
- The first stable release also re-lists all changes that were introduced across its preview/beta cycle. Do not omit items just because they appeared in a prior preview.


### Delisted Releases
- Mark delisted versions in the heading: `## 4.2.1 - July 9, 2025 (Delisted)`
- Explain why the release was delisted and link to the tracking issue.

### Stable Releases That Graduate Preview Features
- Note which features are graduating from preview: "This is the first stable release of the `SetClientFactory` API introduced in 8.2.0-preview."

## Product Reference

Use this table to determine the correct release notes file, GitHub repo (for issue/PR links), and package registry for each product. This information is also available in the [main README](../../../README.md).

### Configuration Providers

| Release Notes File | Module | GitHub Repo | Package |
|---|---|---|---|
| `MicrosoftExtensionsConfigurationAzureAppConfiguration.md` | Microsoft.Extensions.Configuration.AzureAppConfiguration | [Azure/AppConfiguration-DotnetProvider](https://github.com/Azure/AppConfiguration-DotnetProvider) | [NuGet](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.AzureAppConfiguration/) |
| `MicrosoftAzureAppConfigurationAspNetCore.md` | Microsoft.Azure.AppConfiguration.AspNetCore | [Azure/AppConfiguration-DotnetProvider](https://github.com/Azure/AppConfiguration-DotnetProvider) | [NuGet](https://www.nuget.org/packages/Microsoft.Azure.AppConfiguration.AspNetCore/) |
| `MicrosoftAzureAppConfigurationFunctionsWorker.md` | Microsoft.Azure.AppConfiguration.Functions.Worker | [Azure/AppConfiguration-DotnetProvider](https://github.com/Azure/AppConfiguration-DotnetProvider) | [NuGet](https://www.nuget.org/packages/Microsoft.Azure.AppConfiguration.Functions.Worker/) |
| `SpringCloudAzureAppConfigurationConfig.md` | spring-cloud-azure-appconfiguration-config | [Azure/azure-sdk-for-java](https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/spring/spring-cloud-azure-appconfiguration-config) | [Maven](https://search.maven.org/artifact/com.azure.spring/spring-cloud-azure-appconfiguration-config) |
| `AzureAppConfigurationProviderPython.md` | azure-appconfiguration-provider | [Azure/azure-sdk-for-python](https://github.com/Azure/azure-sdk-for-python/tree/main/sdk/appconfiguration/azure-appconfiguration-provider) | [PyPI](https://pypi.org/project/azure-appconfiguration-provider/) |
| `JavaScriptProvider.md` | @azure/app-configuration-provider | [Azure/AppConfiguration-JavaScriptProvider](https://github.com/Azure/AppConfiguration-JavaScriptProvider) | [npm](https://www.npmjs.com/package/@azure/app-configuration-provider) |
| `GoProvider.md` | azureappconfiguration | [Azure/AppConfiguration-GoProvider](https://github.com/Azure/AppConfiguration-GoProvider) | [pkg.go.dev](https://pkg.go.dev/github.com/Azure/AppConfiguration-GoProvider/azureappconfiguration) |

### Feature Management Libraries

| Release Notes File | Module | GitHub Repo | Package |
|---|---|---|---|
| `Microsoft.Featuremanagement.md` | Microsoft.FeatureManagement | [microsoft/FeatureManagement-Dotnet](https://github.com/microsoft/FeatureManagement-Dotnet) | [NuGet](https://www.nuget.org/packages/Microsoft.FeatureManagement) |
| `Microsoft.Featuremanagement.AspNetCore.md` | Microsoft.FeatureManagement.AspNetCore | [microsoft/FeatureManagement-Dotnet](https://github.com/microsoft/FeatureManagement-Dotnet) | [NuGet](https://www.nuget.org/packages/Microsoft.FeatureManagement.AspNetCore) |
| `Microsoft.Featuremanagement.Telemetry.ApplicationInsights.md` | Microsoft.FeatureManagement.Telemetry.ApplicationInsights | [microsoft/FeatureManagement-Dotnet](https://github.com/microsoft/FeatureManagement-Dotnet) | [NuGet](https://www.nuget.org/packages/Microsoft.FeatureManagement.Telemetry.ApplicationInsights) |
| `Microsoft.Featuremanagement.Telemetry.ApplicationInsights.AspNetCore.md` | Microsoft.FeatureManagement.Telemetry.ApplicationInsights.AspNetCore | [microsoft/FeatureManagement-Dotnet](https://github.com/microsoft/FeatureManagement-Dotnet) | [NuGet](https://www.nuget.org/packages/Microsoft.FeatureManagement.Telemetry.ApplicationInsights.AspNetCore) |
| `SpringCloudAzureFeatureManagement.md` | spring-cloud-azure-feature-management | [Azure/azure-sdk-for-java](https://github.com/Azure/azure-sdk-for-java/tree/main/sdk/spring/spring-cloud-azure-feature-management) | [Maven](https://search.maven.org/artifact/com.azure.spring/spring-cloud-azure-feature-management) |
| `PythonFeatureManagement.md` | featuremanagement | [microsoft/FeatureManagement-Python](https://github.com/microsoft/FeatureManagement-Python) | [PyPI](https://pypi.org/project/FeatureManagement/) |
| `JavaScriptFeatureManagement.md` | @microsoft/feature-management | [microsoft/FeatureManagement-JavaScript](https://github.com/microsoft/FeatureManagement-JavaScript) | [npm](https://www.npmjs.com/package/@microsoft/feature-management) |
| `JavaScriptFeatureManagementApplicationInsightsBrowser.md` | @microsoft/feature-management-applicationinsights-browser | [microsoft/FeatureManagement-JavaScript](https://github.com/microsoft/FeatureManagement-JavaScript) | [npm](https://www.npmjs.com/package/@microsoft/feature-management-applicationinsights-browser) |
| `JavaScriptFeatureManagementApplicationInsightsNode.md` | @microsoft/feature-management-applicationinsights-node | [microsoft/FeatureManagement-JavaScript](https://github.com/microsoft/FeatureManagement-JavaScript) | [npm](https://www.npmjs.com/package/@microsoft/feature-management-applicationinsights-node) |
| `GoFeatureManagement.md` | featuremanagement | [microsoft/FeatureManagement-Go](https://github.com/microsoft/FeatureManagement-Go) | [pkg.go.dev](https://pkg.go.dev/github.com/microsoft/Featuremanagement-Go/featuremanagement) |

### Platform & Tools

| Release Notes File | Module | GitHub Repo | Package |
|---|---|---|---|
| `KubernetesProvider.md` | Azure App Configuration Kubernetes Provider | [Azure/AppConfiguration-KubernetesProvider](https://github.com/Azure/AppConfiguration-KubernetesProvider) | [MCR](https://mcr.microsoft.com/en-us/product/azure-app-configuration/kubernetes-provider/about) |
| `AppConfigurationEmulator.md` | Azure App Configuration Emulator | [Azure/AppConfiguration-Emulator](https://github.com/Azure/AppConfiguration-Emulator) | — |
| `AzureDevOpsPipelineExtension.md` | Azure App Configuration (Azure Pipeline) | — | — |
| `AzureDevOpsPushPipelineExtension.md` | Azure App Configuration Push (Azure Pipeline) | — | — |

### Legacy (Deprecated)

| Release Notes File | Module | GitHub Repo |
|---|---|---|
| `AzureSpringCloudAppConfigurationConfig.md` | azure-spring-cloud-appconfiguration-config | [Azure/azure-sdk-for-java](https://github.com/Azure/azure-sdk-for-java) |
| `AzureSpringCloudFeatureManagement.md` | azure-spring-cloud-feature-management | [Azure/azure-sdk-for-java](https://github.com/Azure/azure-sdk-for-java) |
