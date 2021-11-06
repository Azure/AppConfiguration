using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;

namespace FunctionAppIsolatedMode
{
    internal static class AzureAppConfigurationRefreshExtensions
    {
        public static IFunctionsWorkerApplicationBuilder UseAzureAppConfiguration(this IFunctionsWorkerApplicationBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.UseMiddleware<AzureAppConfigurationRefreshMiddleware>();
        }
    }
}
