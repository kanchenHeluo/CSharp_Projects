using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.UI.Models;
using Web.UI.Repositories.Models;
using Web.UI.Repositories;
using Web.UI.Common;
using Web.UI.Providers;
using Microsoft.Practices.Unity;
using System.Threading.Tasks;
using Web.UI.UnitOfWork;
using Web.UI.Repositories.Interfaces;
using Web.UI.Repositories.DomainModels;
using Web.Common.Extensions;
namespace Web.UI.Controllers
{
    public class DomainDataController : WebBaseController
    {
        private const int Locale = 1043;

        [Dependency]
        public IDomainDataProvider DomainData { get; set; }


         [Dependency]
        public IPortalUnitOfWork PortalUnitOfWork { get; set; }


         [Dependency]
         public IOrderRepository OrderRepository { get; set; }

        [HttpGet]
        public async Task<JsonResult> UsageCountries()
        {
             return Json(await DomainData.GetDomainCountryAsync(1033));
        }

        [HttpGet]
        public async Task<JsonResult> SalesLocations()
        {
            return Json(await OrderRepository.GetSalesLocations());
        }

        [HttpGet]
        public async Task<JsonResult> OrderHeaderAttributes(int agreementId, DateTime ?lookUpDate)
        {
            //sensitive to lookupdate, which is going to be the usage date selected 
            if (agreementId > 0)
            {
                return Json(await PortalUnitOfWork.GetOrderHeaderAttributes(agreementId, DateTime.Now, "1033"),
                    JsonRequestBehavior.AllowGet);
            }
            return Json(Task.FromResult<OrderHeaderAttributes>(null), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> LineItemAttributes(LineItemRequest lineItemRequest)
        {
            lineItemRequest.LocaleId = 1033;
            var itemAttributes=await PortalUnitOfWork.GeLineItemAttributes(lineItemRequest);
            if(lineItemRequest.ProductTypeCodes!=null && lineItemRequest.ProgramCodes!=null && lineItemRequest.PurchaseOrderTypes!=null)
            {
                itemAttributes.BillingOptions = await DomainData.GetBillingOptionsAsync(lineItemRequest.ProductTypeCodes.ToArray().Trim(), lineItemRequest.ProgramCodes.ToArray().Trim(), lineItemRequest.PurchaseOrderTypes.ToArray().Trim(), lineItemRequest.LocaleId);
            }


              return Json(itemAttributes, JsonRequestBehavior.AllowGet);

        }
        
        [HttpGet]
        public JsonResult FlagReasons()
        {
            return Json(new[]{
                new DomainItem(){Name="Pricing", Code="PRC"},
                new DomainItem(){Name="Billing", Code="BLG"}
            });
        }

        [HttpGet]
        public JsonResult GetLanguages()
        {
            return Json( DomainData.GetSupportedLanguages().ToList());
        }
    }
}