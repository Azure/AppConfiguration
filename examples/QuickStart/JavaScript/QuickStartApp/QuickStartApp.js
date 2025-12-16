const { load } = require("@azure/app-configuration-provider");
const { DefaultAzureCredential } = require("@azure/identity");

const endpoint = process.env.AZURE_APPCONFIGURATION_ENDPOINT;

async function run() {
    console.log("Sample 1: Load key-values with default selector");

    // Connect to Azure App Configuration using Microsoft Entra ID authentication and load all key-values with null label.
    const settings = await load(endpoint, new DefaultAzureCredential());

    console.log("---Consume configuration as a Map---");
    // Find the key "message" and print its value.
    console.log('settings.get("message"):', settings.get("message"));           // settings.get("message"): Message from Azure App Configuration

    console.log("---Consume configuration as an object---");
    // Construct configuration object from loaded key-values, by default "." is used to separate hierarchical keys.
    const config = settings.constructConfigurationObject();
    // Use dot-notation to access configuration
    console.log("config.message:", config.message);             // config.message: Message from Azure App Configuration
}

run().catch(console.error);