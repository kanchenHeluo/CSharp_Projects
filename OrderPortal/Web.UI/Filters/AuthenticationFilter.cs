using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using Web.UI.Common;
using System.Configuration;

namespace Web.UI.Filters
{
    public class AuthenticationFilter : AuthorizeAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

            if (skipAuthorization)
            {
                return;
            }

            var user = filterContext.HttpContext.User;
            if (user == null || !user.Identity.IsAuthenticated)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
            if (System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                //Below code builds the principle object from STS token                                                                                                                     
                ClaimManager.BuildClaim(ConfigurationManager.AppSettings["MSLGuid"]);
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            // modify filterContext.Result to go somewhere special...if you do
            // nothing here they will just go to the site's default login
        }
    }
}