# C# Code Style

To improve code quality and maintainability, we have introduced new linting rules into our C# codebases.

## About the rule

The general rule we follow is "use Visual Studio defaults". Here are some specific guidelines:

- Always insert a final newline at the end of files.
- Use spaces for indentation with a size of 4.
- Trim trailing whitespace from lines.
- Avoid more than one empty line at any time.
- Avoid spurious free spaces.

We enforce consistent code style, including the usage of new lines, indentation and spacing by applying `IDE0055`, `IDE2000`, `IDE2002`, `IDE2003`, `IDE2004`, `IDE2005` and `IDE2006`. We have adopted all the default settings of [`IDE0055`](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0055), except for `dotnet_sort_system_directives_first = false`, to align with Visual Studio's default behavior where all `using` directives should be sorted in alphabetical order.

An [EditorConfig](https://editorconfig.org/) file (.editorconfig) is provided at the root of all repositories, defining all code format rules.

## How to enforce the linting rule

To ensure that linting rules are strictly enforced across all projects, we have configured each project's .csproj file with the following settings:

``` xml
<PropertyGroup>
  <EnforceCodeStyleInBuild>True</EnforceCodeStyleInBuild>
</PropertyGroup>
```

This configuration enables code style enforcement during the build process. Additionally, the severity of all code format rules has been set to error. As a result, any violations of the linting rules will cause the build to fail.

We recommend using Visual Studio for development. Visual Studio automatically highlights all code format issues and provides suggestions to fix them. We also recommend using the [dotnet-format tool](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-format) which will automatically fixes the coding format issue if possible.
