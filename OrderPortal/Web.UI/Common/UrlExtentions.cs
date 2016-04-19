using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.UI.Common
{
    public static class UrlExtentions
    {

        public static string Home(this UrlHelper helper)
        {
            return helper.Content("~/");
        }
        public static string Dashboard(this UrlHelper helper)
        {
            return helper.Content("~/Dashboard/DashboardHome/Index/");
        }

        public static string OrderDashboard(this UrlHelper helper)
        {
            return helper.Content("~/Order/OrderDashboard/");
        }

        public static string PartnerLogin(this UrlHelper helper)
        {
            return helper.Content("~/home/LoginPartner");
        }
        public static string LoginWindowsLive(this UrlHelper helper)
        {
            return helper.Content("~/home/LoginWindowsLive");
        }

        public static string UnAuthorized(this UrlHelper helper)
        {
            return helper.Content("~/Home/UnAuthorized/");
        }

        public static string SignOut(this UrlHelper helper)
        {
            return helper.Content("~/Home/SignOut/");
        }
        //public static string DashBoard(this UrlHelper helper)
        //{
        //    var request = helper.RequestContext.HttpContext.Request;
        //    //TODO:Move the hardcoded value to config
        //    var uriBuilder = new UriBuilder(request.Url.Scheme, request.Url.Host, request.Url.Port, "OrderCenter");
        //    return uriBuilder.ToString();
        //}
    }
}