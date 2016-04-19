using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.UI.App_Start;
using Web.UI.Common;
using Web.UI.Repositories.DomainModels;
using Web.UI.UnitOfWork;

namespace Web.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
            var wrapper = new EventHandlerTaskAsyncHelper(ApplyUserPreferenceAsync);
            this.AddOnAcquireRequestStateAsync(wrapper.BeginEventHandler, wrapper.EndEventHandler);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityWebActivator.Start();

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new IsoDateTimeConverter());
            ModelBinders.Binders.DefaultBinder = new JsonNetModelBinder();

            GlobalConfiguration.Configuration.Formatters.Insert(0, new JsonNetFormatter(serializerSettings));
            AutoMapperConfig.RegisterMappings();
        }

        private string UserPreferenceSessionName = "UserPreference";
        private async Task ApplyUserPreferenceAsync(object sender, EventArgs e)
        {
            string preferredLanguage = "en-us";

            if (HttpContext.Current != null && HttpContext.Current.Session != null && !string.IsNullOrEmpty(MyPrincipal.Current.UserName))
            {
                UserPreference up = HttpContext.Current.Session[UserPreferenceSessionName] as UserPreference;
                if (up != null)
                {
                    preferredLanguage = up.Language;
                }
                else
                {
                    ClaimManager.BuildClaim(Utility.GetApplicationGuid());
                    var userCtx = UserContext.Current;
                    if (userCtx != null)
                    {
                        IPortalUnitOfWork puw = DependencyResolver.Current.GetService(typeof(IPortalUnitOfWork)) as IPortalUnitOfWork;
                        up = await puw.GetUserPreference(new UserPreference()
                        {
                            ApplicationId = Utility.GetAppliationId(),
                            RequestId = Guid.NewGuid().ToString(),
                            AccessorGuid = userCtx.AccessorGuid,
                            UserCredential = MyPrincipal.Current.UserName,
                            ApplicationGuid = Utility.GetApplicationGuid(),
                            Module = Utility.GetModule(),
                        });

                        up.Language = string.IsNullOrEmpty(up.Language) ? "en-us" : up.Language.ToLower();
                        up.AddressFormat = string.IsNullOrEmpty(up.AddressFormat) ? "en-us" : up.AddressFormat.ToLower();
                        if (string.IsNullOrEmpty(up.DateFormat))
                        {
                            up.DateFormat = "MM/DD/YYYY";
                        }

                        HttpContext.Current.Session[UserPreferenceSessionName] = up;
                        preferredLanguage = up.Language;
                    }
                }
            }

            CultureInfo ci = new CultureInfo(preferredLanguage);
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        protected void Application_EndRequest()
        {
            string requestedWithHeader = HttpContext.Current.Request.Headers["X-Requested-With"];
            if (Context.Response.StatusCode == 302 &&
                !string.IsNullOrEmpty(requestedWithHeader) &&
                requestedWithHeader.Equals("XMLHttpRequest", StringComparison.OrdinalIgnoreCase))
            {
                Context.Response.StatusCode = 200;
                Context.Response.AddHeader("REQUIRES_AUTH", "1");
            }
        }
    }
}
