using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.UI.Areas.Order.Controllers
{
    [AllowAnonymous] 
    public class ResourceController : Controller
    {
        // GET: Order/Resource
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AgreementSearch()
        {
            return PartialView("Partials/AgreementSearch"); 
        }

        public ActionResult AgreementSearchResult()
        {
            return PartialView("Partials/AgreementSearchResult");
        }

        public ActionResult OrderHeader()
        {
            return PartialView("Partials/OrderHeader");
        }

        public ActionResult SuperCatalog()
        {
            return PartialView("Partials/SuperCatalog");
        }

        public ActionResult OrderLineItem()
        {
            return PartialView("Partials/OrderLineItem");
        }

        public ActionResult OrderLineItemRow()
        {
            return PartialView("Partials/OrderLineItemRow");
        }

        public ActionResult OrderShipTo()
        {
            return PartialView("Partials/OrderShipTo");
        }

        public ActionResult OrderEditor()
        {
            return View("OrderEditor");
        }

        public ActionResult Dashboard()
        {
            return PartialView("Dashboard");
        }

        public ActionResult MockUpUI()
        {
            return View();
        }

        public ActionResult BatchUpload()
        {
            return PartialView("Partials/BatchUpload");
        }

        public ActionResult OrderSummary()
        {
            return View();
        }

        [HttpGet]
        public ActionResult OrderRenewal()
        {
            return View();
        }

        public ActionResult UserPreference()
        {
            return PartialView("Partials/UserPreference");
        }
    }
}