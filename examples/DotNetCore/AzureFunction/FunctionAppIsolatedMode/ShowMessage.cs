using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunctionAppIsolatedMode
{
    public class ShowMessage
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public ShowMessage(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _logger = loggerFactory.CreateLogger<ShowMessage>();
        }

        [Function("ShowMessage")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            // Read configuration data
            string key = "TestApp:Settings:Message";
            string message = _configuration[key];

            response.WriteString(message ?? $"Please create a key-value with the key '{key}' in Azure App Configuration.");

            return response;
        }
    }
}
