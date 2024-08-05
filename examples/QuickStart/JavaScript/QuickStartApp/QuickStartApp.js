const { load } = require("@azure/app-configuration-provider");
const connectionString = process.env.AZURE_APPCONFIG_CONNECTION_STRING;

async function run() {
    console.log("Sample 1: Load key-values with default selector");

    // Connect to Azure App Configuration using a connection string and load all key-values with null label.
    const settings = await load(connectionString);

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