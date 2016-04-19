using System.Web;
using System.Web.Mvc;
using Web.UI.Common;
using Web.UI.Filters;

namespace Web.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorHandlerFilter());
            filters.Add(new AuthenticationFilter());
        }
    }
}
