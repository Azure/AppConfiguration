using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace FunctionAppIsolatedMode
{
    internal class AzureAppConfigurationRefreshMiddleware : IFunctionsWorkerMiddleware
    {
        public IEnumerable<IConfigurationRefresher> Refreshers { get; }

        public AzureAppConfigurationRefreshMiddleware(IConfigurationRefresherProvider refresherProvider)
        {
            Refreshers = refresherProvider.Refreshers;
        }
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            foreach (var refresher in Refreshers)
            {
                _ = refresher.TryRefreshAsync();
            }

            await next(context).ConfigureAwait(false);
        }
    }
}
