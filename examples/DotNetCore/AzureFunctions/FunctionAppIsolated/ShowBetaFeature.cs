using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace FunctionAppIsolated
{
    public class ShowBetaFeature
    {
        private readonly IVariantFeatureManagerSnapshot _featureManager;
        private readonly ILogger<ShowBetaFeature> _logger;

        public ShowBetaFeature(IVariantFeatureManagerSnapshot featureManager, ILogger<ShowBetaFeature> logger)
        {
            _featureManager = featureManager;
            _logger = logger;
        }

        [Function("ShowBetaFeature")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Read feature flag
            string featureName = "Beta";
            bool featureEnabled = await _featureManager.IsEnabledAsync(featureName);

            return new OkObjectResult(featureEnabled
                ? $"{featureName} feature is On"
                : $"{featureName} feature is Off (or not found in Azure App Configuration).");
        }
    }
}
