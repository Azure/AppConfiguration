# Copilot Instructions — Azure App Configuration

This is the central hub repository for **Azure App Configuration**. It contains cross-language examples, documentation, configuration schemas, and release notes for the App Configuration ecosystem.

## Repository Structure

- `examples/` — Working sample applications across .NET, Java/Spring, Python, Go, and JavaScript
- `docs/` — Configuration type schemas (AI Chat, Key Vault Reference, KVSet) and integration guides
- `releaseNotes/` — Release notes for all App Configuration libraries and providers

This is **not** a library repo — it is a reference and documentation repository.

## Languages & Frameworks

| Language | Frameworks | Location |
|----------|-----------|----------|
| .NET | ASP.NET Core, Azure Functions, .NET Framework | `examples/DotNetCore/`, `examples/DotNetFramework/` |
| Java | Spring Boot, Spring Cloud Azure | `examples/Spring/` |
| Python | Flask, Django, console apps | `examples/Python/` |
| Go | Gin, console apps | `examples/Go/` |
| JavaScript | Node.js | `examples/QuickStart/JavaScript/` |

## Code Style

### Python
- **Black** formatter: line-length=120, target Python 3.8+
- **Pylint**: max-line-length=120
- **MyPy**: type checking is recommended; use where configured in individual examples
- Run formatting and linting directly via these tools (or any per-example tooling); no shared tox tasks are defined at the repo root

### All Languages
- Each example must be self-contained with its own README, dependencies, and setup instructions
- READMEs must include: Features, Prerequisites, Setup, and Running instructions
- Environment variables and connection strings must be documented but never hardcoded
- Secrets must use Key Vault references, never plain text in config

## Adding or Modifying Examples

- Follow the existing directory structure: `examples/{Language}/{ExampleName}/`
- Include a `README.md` with clear setup and run instructions
- List all prerequisites (SDKs, Azure resources, environment variables)
- Keep examples minimal and focused on demonstrating a single integration pattern
- Use the Azure App Configuration provider library for the target language

## Documentation & Schemas

- Configuration schemas live in `docs/ConfigurationTypes/` as versioned JSON Schema files
- Follow semantic versioning in schema filenames: `{Type}.v{Major}.{Minor}.{Patch}.schema.json`

## Release Notes

- Release notes are Markdown files in `releaseNotes/`, one per product
- Entries are in reverse chronological order (newest first)
- Each entry includes version number, date, and a bulleted list of changes

## Security

- Report vulnerabilities through MSRC (https://msrc.microsoft.com/create-report), never via public issues
- Never commit secrets, connection strings, or credentials
