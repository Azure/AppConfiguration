using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;

namespace FunctionAppIsolatedMode
{
    internal static class AzureAppConfigurationRefreshExtensions
    {
        public static IFunctionsWorkerApplicationBuilder UseAzureAppConfiguration(this IFunctionsWorkerApplicationBuilder builder)
        {
            return builder.UseMiddleware<AzureAppConfigurationRefreshMiddleware>();
        }
    }
}
