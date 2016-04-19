using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.UI.Common;
using Web.UI.Repositories.Interfaces;
using Web.UI.UnitOfWork;

namespace Web.UI.Areas.Dashboard.Controllers
{
    public class DashboardHomeController : WebBaseController
    {
        [Dependency]
        public IPortalUnitOfWork PortalUnitOfWork { get; set; }


        public DashboardHomeController()           
        {

        } 
        // GET: Dashboard/Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdatePartner(string pcnNo)
        {
            UserContext.Current.SetPcn(pcnNo);
            return new EmptyResult();
        }
    }
}