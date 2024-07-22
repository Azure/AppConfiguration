const { load } = require("@azure/app-configuration-provider");
const connectionString = process.env.AZURE_APPCONFIG_CONNECTION_STRING;

async function run() {
    console.log("Sample 1: Load key-values with default selector");

    // Connect to Azure App Configuration using a connection string and load all key-values with null label.
    const settings = await load(connectionString);

    console.log("---Consume configuration as a Map---");
    // Find the key "message" and print its value.
    console.log('settings.get("message"):', settings.get("message"));           // settings.get("message"): Message from Azure App Configuration
    // Find the key "app.greeting" and print its value.
    console.log('settings.get("app.greeting"):', settings.get("app.greeting")); // settings.get("app.greeting"): Hello World
    // Find the key "app.json" whose value is an object.
    console.log('settings.get("app.json"):', settings.get("app.json"));         // settings.get("app.json"): { myKey: 'myValue' }

    console.log("---Consume configuration as an object---");
    // Construct configuration object from loaded key-values, by default "." is used to separate hierarchical keys.
    const config = settings.constructConfigurationObject();
    // Use dot-notation to access configuration
    console.log("config.message:", config.message);             // config.message: Message from Azure App Configuration
    console.log("config.app.greeting:", config.app.greeting);   // config.app.greeting: Hello World
    console.log("config.app.json:", config.app.json);           // config.app.json: { myKey: 'myValue' }
}

run().catch(console.error);