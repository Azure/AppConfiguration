using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace FunctionApp
{
    public class ShowBetaFeature
    {
        private readonly IFeatureManagerSnapshot _featureManagerSnapshot;
        private readonly IConfigurationRefresher _configurationRefresher;

        public ShowBetaFeature(IFeatureManagerSnapshot featureManagerSnapshot, IConfigurationRefresher configurationRefresher)
        {
            _featureManagerSnapshot = featureManagerSnapshot;
            _configurationRefresher = configurationRefresher;
        }

        [FunctionName("ShowBetaFeature")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // Signal to refresh the feature flags from Azure App Configuration.
            // This will be a no-op if the cache expiration time window is not reached.
            // Remove the 'await' operator if it's preferred to refresh without blocking.
            await _configurationRefresher.Refresh();

            string featureName = "Beta";
            bool featureEnalbed = await _featureManagerSnapshot.IsEnabledAsync(featureName);

            return featureEnalbed
                ? (ActionResult)new OkObjectResult($"{featureName} feature is On")
                : new BadRequestObjectResult($"{featureName} feature is Off (or the feature flag '{featureName}' is not present in Azure App Configuration).");

        }
    }
}
