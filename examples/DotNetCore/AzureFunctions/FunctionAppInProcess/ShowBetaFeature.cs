using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace FunctionAppInProcess
{
    public class ShowBetaFeature(IVariantFeatureManagerSnapshot featureManager, IConfigurationRefresherProvider refresherProvider)
    {
        private readonly IVariantFeatureManagerSnapshot _featureManager = featureManager;
        private readonly IConfigurationRefresher _configurationRefresher = refresherProvider.Refreshers.First();

        [FunctionName("ShowBetaFeature")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Signal to refresh the feature flags from Azure App Configuration.
            // This will be a no-op if the refresh interval has not elapsed.
            // Remove the 'await' operator if it's preferred to refresh without blocking.
            await _configurationRefresher.TryRefreshAsync();

            // Read feature flag
            string featureName = "Beta";
            bool featureEnabled = await _featureManager.IsEnabledAsync(featureName);

            return new OkObjectResult(featureEnabled
                ? $"{featureName} feature is On"
                : $"{featureName} feature is Off (or not found in Azure App Configuration).");
        }
    }
}
