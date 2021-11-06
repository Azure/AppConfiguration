using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace FunctionAppIsolatedMode
{
    public class ShowBetaFeature
    {
        private readonly IFeatureManagerSnapshot _featureManagerSnapshot;
        private readonly ILogger _logger;

        public ShowBetaFeature(IFeatureManagerSnapshot featureManagerSnapshot, ILoggerFactory loggerFactory)
        {
            _featureManagerSnapshot = featureManagerSnapshot;
            _logger = loggerFactory.CreateLogger<ShowBetaFeature>();
        }

        [Function("ShowBetaFeature")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            // Read feature flag
            string featureName = "Beta";
            bool featureEnalbed = await _featureManagerSnapshot.IsEnabledAsync(featureName);

            response.WriteString(featureEnalbed 
                                 ? $"{featureName} feature is On"
                                 : $"{featureName} feature is Off (or the feature flag '{featureName}' is not present in Azure App Configuration).");

            return response;
        }
    }
}
