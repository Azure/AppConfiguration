const appConfig = require("@azure/app-configuration");

if (!process.env.production) {
    require("dotenv").config();
    console.log("debug environment");
}

const connection_string = process.env.AZURE_APP_CONFIG_CONNECTION_STRING;
const client = new appConfig.AppConfigurationClient(connection_string);

async function run() {
  
    let retrievedSetting = await client.getConfigurationSetting({
      key: "TestApp:Settings:Message"
    });
  
    console.log("Retrieved value:", retrievedSetting.value);
}
  
run().catch((err) => console.log("ERROR:", err));