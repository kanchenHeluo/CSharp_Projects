using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web.UI.Common;
using Web.UI.Repositories.DomainModels;
using Web.UI.UnitOfWork;

namespace Web.UI.Areas.Order.Controllers
{
    public class UserMgmtController : Controller
    {
        [Dependency]
        public IPortalUnitOfWork PortalUnitOfWork { get; set; }

        private string UserPreferenceSessionName = "UserPreference";

        [HttpGet]
        public JsonResult GetUserPreference()
        {
            var up = Session[UserPreferenceSessionName] as UserPreference;
            return Json(up, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SetUserPreference(UserPreference up)
        {
            var userCtx = UserContext.Current;
            
            up.ApplicationId = Utility.GetAppliationId();
            up.RequestId = Guid.NewGuid().ToString();
            up.AccessorGuid = userCtx.AccessorGuid;
            up.UserCredential = MyPrincipal.Current.UserName;
            up.ApplicationGuid = Utility.GetApplicationGuid();
            up.Module = Utility.GetModule();
            var ret = await PortalUnitOfWork.SetUserPreference(up);

            if (ret)
            {
                Session[UserPreferenceSessionName] = up;
            }

            return Json(ret);
        }
    }
}