using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace WebDemoWithEventHub
{
    public class ConfigRefresher : IConfigRefresher
    {
        public IConfigurationRefresher Refresher { get; set; }

        public void RefreshConfiguration()
        {
            Refresher.TryRefreshAsync();
        }
    }
}
