using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.UI.Models;
using Web.UI.Repositories.Models;
using Web.UI.Repositories;

namespace Web.UI.Controllers
{
    public class AutocompleteController : Controller
    {
        // GET: Autocomplete
        [HttpGet]
        public JsonResult GetData(string partnerPCN, string customerPCN, string agreementNumber)
        {
            
            var autocomplete =new  AutoComplete();
            autocomplete.Data = new List<AgreementRef>();
            ProductRepository results = new ProductRepository();
            autocomplete.Data = results.SearchAgreement(partnerPCN, customerPCN, "Hana Bank", agreementNumber, "date");
           return Json(autocomplete, JsonRequestBehavior.AllowGet);
            
        }
    }
}