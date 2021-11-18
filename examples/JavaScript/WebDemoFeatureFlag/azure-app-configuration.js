const { AppConfigurationClient } = require("@azure/app-configuration");

if (!process.env.production) {
  require("dotenv").config();
  console.log("debug environment");
}

const connectionString = process.env.APPCONFIG_CONNECTION_STRING;
console.log(connectionString)
const client = new AppConfigurationClient(connectionString);

const getConfigurationSetting = async (key) => {

  try {

    return client.getConfigurationSetting({key}).then(result =>{
        console.log(result);
        return result.value;
      }).catch(ex =>{
        console.log(ex);
      });

  } catch (ex) {
    console.log(ex);
    return null; // no key found or error
  }
};

module.exports = {
  getConfigurationSetting
};
