// Watch this issue for this quickstart: https://github.com/Azure/azure-sdk-for-js/issues/18669
const {AppConfigurationClient, parseSecretReference} = require("@azure/app-configuration");

// Add settings `.env` file if not production environment
// Copy the `.env-sample` and rename it to `.env`, then
// add your own values
if (!process.env.production) {
  require("dotenv").config();
  console.log("debug environment");
}

// Authentication for App Configuration
const connection_string = process.env.AZURE_APP_CONFIG_CONNECTION_STRING;
const client = new AppConfigurationClient(connection_string);

// Authentication for Key Vault
if (
  !process.env["AZURE_TENANT_ID"] ||
  !process.env["AZURE_CLIENT_ID"] ||
  !process.env["AZURE_CLIENT_SECRET"] ||
  !process.env["KEYVAULT_URI"]
) {
  console.log(`At least one of the AZURE_TENANT_ID, AZURE_CLIENT_ID, AZURE_CLIENT_SECRET, and KEYVAULT_URI variables is not present, 
    please add the missing ones in your environment and rerun the sample.`);
  return;
}
// DefaultAzureCredential expects the following three environment variables:
// - AZURE_TENANT_ID: The tenant ID in Azure Active Directory
// - AZURE_CLIENT_ID: The application (client) ID registered in the AAD tenant
// - AZURE_CLIENT_SECRET: The client secret for the registered application
const { SecretClient, parseKeyVaultSecretIdentifier } = require("@azure/keyvault-secrets");
const { DefaultAzureCredential } = require("@azure/identity");
const credential = new DefaultAzureCredential();
const url = process.env["KEYVAULT_URI"];
const secretClient = new SecretClient(url, credential);

// Secret id in App Configuration
const appConfigurationKeyForSecret = "TestApp:Settings:KeyVaultMessage";

const run = async (key) => {
  try {
    // Get App config reference
    let retrievedSetting = await client.getConfigurationSetting({
      key,
    });

    // Parse App config reference to get reference URI
    const parsedSecretReference = parseSecretReference(retrievedSetting);

    // Parse reference URI to get Secret Name
    const secretName = parseKeyVaultSecretIdentifier(parsedSecretReference.value.secretId).name;

    // Read the secret
    const secret = await secretClient.getSecret(
      secretName
    );
    
    console.log(
      `Get the secret from keyvault key: {'${secret.name}', value: '${secret.value}'}`
    );
    return secret.value;
  } catch (err) {
    console.log(err);
  }
}

// Express server
const express = require("express");
const port = process.env.PORT || 3000;

// server
const app = express();

// root route
app.get("/", async (req, res) => {
  
  // get secret from key vault
  const secret = await run(appConfigurationKeyForSecret);

  // return JSON with secret name and secret value
  res.json({ [appConfigurationKeyForSecret]: secret });
});
app.listen(port, () => {
  console.log(`Server has started on port ${port}!`);
});
