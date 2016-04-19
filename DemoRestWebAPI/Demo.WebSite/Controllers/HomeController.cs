
namespace Demo.WebSite.Controllers
{
    using Contracts;
    using Impl;
    using Interface;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;    
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        #region Fields

        private readonly string purchaseorderBaseUrl;

        private readonly IHttpService httpService;

        #endregion
        
        #region Constructors and Destructors

        public HomeController()
        {
            httpService = new HttpService();
            purchaseorderBaseUrl =
               // "https://localhost:44303/api/purchaseorder";
            "https://localhost/DemoAPI/api/purchaseorder";
        }

        #endregion

        #region Public Methods and Operators

        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetPurchaseOrderAsync(long poId)
        {
            var requestUri = new Uri(string.Format("{0}/{1}", this.purchaseorderBaseUrl, poId));
            var ret = await httpService.HttpsGetAsync(requestUri);

            return Json(JsonConvert.DeserializeObject<List<PurchaseOrder>>(ret), JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetPurchaseOrderLineItemAsync(long poId, long poLiId)
        {
            var requestUri = new Uri(string.Format("{0}/{1}/lineitem/{2}", this.purchaseorderBaseUrl, poId, poLiId));
            var ret = await httpService.HttpsGetAsync(requestUri);

            return Json(JsonConvert.DeserializeObject<List<PurchaseOrderLineItem>>(ret), JsonRequestBehavior.AllowGet);
        }

        #endregion
        
    }
}