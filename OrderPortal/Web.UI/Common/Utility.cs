using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using Web.Common.Extensions;

namespace Web.UI.Common
{
    public static class Utility
    {

        public static string GetUserName()
        {
            string userName = string.Empty;
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var currentPricipal = Thread.CurrentPrincipal as ClaimsPrincipal;
                if(currentPricipal!=null)
                {
                  var firstNameClaim=  currentPricipal.Claims.Single(c => c.Type == Constants.ClaimFirstName);
                    if(firstNameClaim!=null)
                    {
                        userName = firstNameClaim.Value;
                    }
                }

            }
            return userName;
        }

        public static UrlHelper GetUrlHelper()
        {
            return new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        public static int GetMaxLockMinutes()
        {
            return ConfigurationManager.AppSettings.GetValue<int>("MaxLockMinutes", 30);
        }

        public static string GetAppliationId()
        {
            return ConfigurationManager.AppSettings.GetValue<string>("ApplicationId", "OrderPortal");
        }

        public static string GetApplicationGuid()
        {
            return ConfigurationManager.AppSettings.GetValue<string>("ApplicationGuid", "B139BF32-9D90-4BCE-90C3-0F87BEB4BA7C");
        }

        public static string GetModule()
        {
            return ConfigurationManager.AppSettings.GetValue<string>("Module", "TXWEB");
        }
    }
}