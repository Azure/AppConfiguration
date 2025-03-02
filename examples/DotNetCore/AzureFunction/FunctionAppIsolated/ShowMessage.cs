using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunctionAppIsolated
{
    public class ShowMessage
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ShowMessage> _logger;

        public ShowMessage(IConfiguration configuration, ILogger<ShowMessage> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [Function("ShowMessage")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            // Read configuration data
            string key = "TestApp:Settings:Message";
            string? message = _configuration[key];

            return new OkObjectResult(message ?? $"Please create a key-value with the key '{key}' in Azure App Configuration.");
        }
    }
}
