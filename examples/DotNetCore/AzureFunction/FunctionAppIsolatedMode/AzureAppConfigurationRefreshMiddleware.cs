using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace FunctionAppIsolatedMode
{
    internal class AzureAppConfigurationRefreshMiddleware : IFunctionsWorkerMiddleware
    {
        private IEnumerable<IConfigurationRefresher> _refreshers;

        public AzureAppConfigurationRefreshMiddleware(IConfigurationRefresherProvider refresherProvider)
        {
            _refreshers = refresherProvider.Refreshers;
        }
        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            foreach (var refresher in _refreshers)
            {
                _ = refresher.TryRefreshAsync();
            }

            await next(context);
        }
    }
}
