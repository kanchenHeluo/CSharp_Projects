using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Web.UI.Common.Extensions;
using Web.UI.Repositories.Models;
using Web.UI.Repositories.DomainModels;
using Web.UI.ServiceGateway.DraftOrderServiceProxy;
using Web.UI.UnitOfWork;
using UIModels = Web.UI.Models;
using Web.UI.Common;
using System.Threading.Tasks;

namespace Web.UI.Areas.Order.Controllers
{
    public class OrderDashboardController : WebBaseController
    {
        [Dependency]
        public IPortalUnitOfWork PortalUnitOfWork { get; set; }

        #region OrderLineItemModel instantiation
        private UIModels.OrderSearchResult olim = new UIModels.OrderSearchResult()
        {
            PurchaseOrderNumber = "PO-5656656",
            StatusName = "Review",
            AgreementNumber = "V232445",
            ProgramName = "Select Plus",
            DirectCustomerName = "Ingram Micro",
            InDirectCustomerName = "RMM Solutions",
            SalesLocationName = "United States",
            UsageDate = DateTime.Now,
            PricingCountryCode = "US",
            PricingCurrencyCode = "USD",
            OrderName = "Order Name111",
            EndCustomerName = "Dell Inc",
            SourceCode = "Batch",
            CreatedUser = "kuralel",
            Comments = new List<DraftOrderComment>()
        };

        #endregion
        // GET: Order/OrderDashboard
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<JsonResult> GetOrdersWithStatus(string pcnFilter, string customerNumber, long? id, int pageNumber, int pageSize, string[] purchaseOrderStatusCodeTable)
        {
            var ret =
                await PortalUnitOfWork.GetOrdersWithStatus(pcnFilter, customerNumber, id, MyPrincipal.Current.UserName, pageNumber, pageSize, purchaseOrderStatusCodeTable);

            if (id == null)
            {
                return Json(new 
                        {
                                Order = ret.Results.Select(item => item.Key).ToList(),
                                TotalCount = ret.TotalCount
                        });
            }
            Models.Order order = ret.Results.FirstOrDefault().Key;
            List<Models.LineItem> lineItems = ret.Results.FirstOrDefault().Value.EmptyIfNull().ToList();
            if (lineItems.Count > 0)
            {
                order.UsageDate = lineItems.Min(p => p.POLIUsageDate);
            }
            return Json(new {Order = order, LineItems = lineItems});
        }
    }
}