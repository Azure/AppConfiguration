# Azure App Configuration Special Types

Azure App Configuration supports several predefined configuration types that are optimized for specific scenarios and use cases. These special types provide structured data formats with validation and enhanced functionality beyond simple key-value pairs.

## Available Special Types

### Key Vault Reference

A Key Vault Reference allows you to securely store sensitive configuration values in Azure Key Vault while referencing them from Azure App Configuration. This provides an additional layer of security for secrets, connection strings, and other sensitive data.

**Use Cases:**
- Database connection strings
- API keys and secrets
- Certificates and passwords
- Any sensitive configuration data

**Schema:** [KeyVaultReference.v1.0.0.schema.json](KeyVaultReference.v1.0.0.schema.json)

### AI Chat Configuration

AI Chat Configuration provides a structured format for configuring AI chat applications, particularly those using Azure OpenAI or other chat completion services. This configuration type enables dynamic adjustment of AI parameters without requiring application restarts.

**Use Cases:**
- Chat completion parameters (temperature, max tokens, etc.)
- System prompts and conversation context
- Model selection and fine-tuning parameters
- Response formatting and behavior controls

**Schema:** [AIChatConfiguration.v1.0.0.schema.json](AIChatConfiguration.v1.0.0.schema.json)

## Benefits of Special Types

- **Validation**: Built-in schema validation ensures configuration correctness
- **Type Safety**: Structured data with well-defined properties and constraints
- **Documentation**: Self-documenting configuration with clear property descriptions
- **Tooling Support**: Enhanced IDE support and validation during development
- **Consistency**: Standardized formats across applications and environments
