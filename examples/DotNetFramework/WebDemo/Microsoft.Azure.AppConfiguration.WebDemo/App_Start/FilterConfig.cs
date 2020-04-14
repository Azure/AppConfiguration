using System.Web;
using System.Web.Mvc;

namespace Microsoft.Azure.AppConfiguration.WebDemo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
