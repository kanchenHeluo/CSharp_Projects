using System.Configuration;
using ExcelParser;
using Microsoft.Ajax.Utilities;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Web.Mvc;
using Web.UI.Common;
using Web.UI.Common.Extensions;
using Web.UI.Models;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Models;
using Web.UI.UnitOfWork;
using Agreement = Web.UI.Models.Agreement;
using System.Text;
using Web.Localization;

namespace Web.UI.Areas.Order.Controllers
{
    public class ManageVLOrderController : WebBaseController
    {
        [Dependency]
        public IPortalUnitOfWork PortalUnitOfWork { get; set; }

        [HttpPost]
        public async Task<JsonResult> SearchAgreement(AgreementRequest searchCriteria)
        {
            if (searchCriteria == null || searchCriteria.LookUpDate == default(DateTime) ||
                (searchCriteria.AgreementNumber.IsNullOrWhiteSpace() && searchCriteria.EndCustomerName.IsNullOrWhiteSpace() && searchCriteria.EndCustomerNumber.IsNullOrWhiteSpace()))
            {
                return InvalidRequest();
            }
            searchCriteria.PartnerNumber = UserContext.Current.PartnerPCN;
            var agreement = await PortalUnitOfWork.SearchAgreement(searchCriteria);
            return Json(agreement);
        }


        [HttpPost]
        public async Task<JsonResult> GetCustomers(AgreementRequest searchCriteria)
        {
            if ( searchCriteria.PartnerNumber.IsNullOrWhiteSpace())
            {
                return InvalidRequest();
            }
            var agreement = await PortalUnitOfWork.GetCustomers(searchCriteria);
            return Json(agreement);
        }

        [HttpPost]
        public async Task<JsonResult> SearchSku(ProductRequest productRequest)
        {
            if (productRequest == null || (productRequest.PartNumber.IsNullOrWhiteSpace()
                && productRequest.ProductFamilyCode.IsNullOrWhiteSpace() && productRequest.ItemName.IsNullOrWhiteSpace()))
            {
                return InvalidRequest();
            }
            productRequest.SearchPattern = productRequest.PartNumber.IsNullOrWhiteSpace() ? SearchPattern.Like : SearchPattern.Exact;
            var lineItems = await PortalUnitOfWork.SearchSku(productRequest);
            if (!lineItems.Results.Any())
            {
                Error = "SearchSkuMessage".Localize();//"Cannot find a result against the part number";
            }

            return Json(lineItems);
        }

        [HttpPost]
        public async Task<JsonResult> SaveDraftOrder(Agreement agreement, Models.Order order, List<LineItem> lineItems)
        {
            if (agreement == null || order == null || order.AgreementNumber.IsNullOrWhiteSpace() || order.AgreementId <=0 || order.ProgramCode.IsNullOrWhiteSpace() || order.DirectCustomerNumber.IsNullOrWhiteSpace())
            {
                return InvalidRequest();
            }
            var ret = await PortalUnitOfWork.SaveDraftOrder(agreement, order, lineItems, MyPrincipal.Current.UserName, Guid.NewGuid());

            Message = "SaveDraftOrderMessage".Localize();//"Save Draft Order Successfully.";
            return Json(ret);
        }

        [HttpPost]
        public async Task<JsonResult> ValidateDraftOrder(Agreement agreement, Models.Order order, List<LineItem> lineItems)
        {
            if (agreement == null || order == null || order.AgreementNumber.IsNullOrWhiteSpace() || order.AgreementId <= 0 || order.ProgramCode.IsNullOrWhiteSpace() || order.DirectCustomerNumber.IsNullOrWhiteSpace())
            {
                return InvalidRequest();
            }
            var ret = await PortalUnitOfWork.ValidateDraftOrder(agreement, order, lineItems, MyPrincipal.Current.UserName);

            return Json(ret);
        }

        [HttpPost]
        public async Task<JsonResult> GetDraftOrder(long? id, string pcnFilter)
        {
            if (!UserContext.Current.SetPcn(pcnFilter) || (id.HasValue && id <= 0) || pcnFilter.IsNullOrWhiteSpace())
            {
                return InvalidRequest();
            }

            var ret = await PortalUnitOfWork.GetDraftOrder(id, pcnFilter, MyPrincipal.Current.UserName);

            return id == null 
                ? Json(new {Pcn = pcnFilter, SavedOrder = ret.Select(item => item.Key).OrderByDescending(item => item.ModifiedDate)})
                : Json(new {Pcn = pcnFilter, Header = ret.FirstOrDefault().Key, LineItems = ret.FirstOrDefault().Value });
        }

        [HttpPost]
        public async Task<JsonResult> SearchOpportunity(string opportunityType, OpportunityRequest request)
        {
            DefaultJsonReturn = Enumerable.Empty<LineItem>();
            if (request == null || request.AgreementId <= 0 || request.AgreementNumber.IsNullOrWhiteSpace() || request.POAgreementNumber.IsNullOrWhiteSpace() || request.PublicCustomerNumber.IsNullOrWhiteSpace() || request.LookupDate == default(DateTime))
            {
                return InvalidRequest();
            }
            //POAgreementId 
            //LocaleCode = "1033",
            //OrgGuid { get; set; }
            request.EndCustomerNumber = UserContext.Current.PartnerPCN;
            IEnumerable<LineItem> lineItems = null;
            switch (opportunityType)
            {
                case "Renewal":
                    lineItems = await PortalUnitOfWork.SearchRenewalOpportunity(request);
                    break;
                case "StepUp":
                    lineItems = await PortalUnitOfWork.SearchStepUpOpportunity(request);
                    break;
                case "TrueUp":
                    lineItems = await PortalUnitOfWork.SearchTrueUpOpportunity(request);
                    break;
            }
            return Json(lineItems);
        }

        [HttpPost]
        public async Task<JsonResult> SearchOrderHistory(int agreementId)
        {
            DefaultJsonReturn = Enumerable.Empty<LineItem>();
            if (agreementId <=0 )
            {
                return InvalidRequest();
            }
            var ret = await PortalUnitOfWork.SearchHistoryLineItem(agreementId, UserContext.Current.PartnerPCN);
            return Json(ret);
        }

        [HttpPost]
        public async Task<JsonResult> SaveShipmentAddress(Shipment shipment )
        {
            if (shipment == null)
            {
                return InvalidRequest();
            }
            var ret = await PortalUnitOfWork.SaveShipmentAddress(shipment, MyPrincipal.Current.UserName);
            Message = "SaveShipmentAddressMessage".Localize();// "Ship To Address Saved Successfully.";
            return Json(ret);
        }

        public async Task<JsonResult> GetAllShipmentList(int agreementId)
        {
            if (agreementId <= 0)
            {
                return InvalidRequest();
            }
            var ret = await PortalUnitOfWork.GetAllShipmentList(agreementId);
            return Json(ret);
        }

        public async Task<JsonResult> DeleteShipmentAddress(long id)
        {
            if (id <= 0)
            {
                return InvalidRequest();
            }
            var ret = await PortalUnitOfWork.DeleteShipmentAddress(id);
            Message = "DeleteShipmentAddressMessage".Localize();//Ship To Delete Successfully.";
            return Json(ret);
        }
        
        [HttpPost]
        public JsonResult GetCoverageDate(string programType, DateTime poliUsageDate, DateTime? currentAnvDt,
            DateTime? endEffectiveDate, DateTime coverageEndDate, string subscriptionMonth, string billinOptionCode)
        {
            var ret = PortalUnitOfWork.GetCoverageDate(programType, poliUsageDate, currentAnvDt, endEffectiveDate,
                    coverageEndDate, subscriptionMonth, billinOptionCode);
            return Json(new {CoverageStartDate = ret.Key, CoverageEndDate = ret.Value});
        }

        [HttpPost]
        public async Task<JsonResult> GetLineItemEstimate(LineItem lineItem, Agreement agreement, string poType)
        {
            if (lineItem == null || agreement == null || poType.IsNullOrWhiteSpace() || agreement.AgreementNumber.IsNullOrWhiteSpace() || agreement.EndCustomerNumber.IsNullOrWhiteSpace()
                || !lineItem.POLIUsageDate.HasValue || lineItem.QuantityOrdered<=0 || lineItem.BillingOptionCode.IsNullOrWhiteSpace() || lineItem.ProgramOfferingCode.IsNullOrWhiteSpace())
            {
                return InvalidRequest();
            }
            return Json(await PortalUnitOfWork.GetPriceEstimate(lineItem, agreement, poType));
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<JsonResult> CreateOrder(Agreement agreement, Models.Order order, List<LineItem> lineItems,Shipment shipTo)
        {

            var result  = await PortalUnitOfWork.CreateOrder(agreement, order, lineItems, shipTo, MyPrincipal.Current.UserName );
            if (result != -1)
            {
                Message = "CreateOrderMessage".Localize();//"Create Order Succeed!";
            }
            else
            {
                Error = "CreateOrderError".Localize();// "Create Order Failed";
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> LockDraftOrder(long id)
        {
            return Json(await PortalUnitOfWork.LockDraftOrder(id, MyPrincipal.Current.UserName, Utility.GetMaxLockMinutes()));
        }

        [HttpPost]
        public async Task<JsonResult> UnlockDraftOrder(long id)
        {
            return Json(await PortalUnitOfWork.UnlockDraftOrder(id, MyPrincipal.Current.UserName, Utility.GetMaxLockMinutes()));
        }

        [HttpPost]
        public async Task<JsonResult> DeleteDraftOrder(long id)
        {
            return Json(await PortalUnitOfWork.DeleteDraftOrder(id, MyPrincipal.Current.UserName, Utility.GetMaxLockMinutes()));
        }

        [HttpPost]
        public async Task<JsonResult> Upload(FormCollection form)
        {
            // do we need to submit order after save it as the draft?
            var unsubmitflag = !String.IsNullOrEmpty(form["unsubmitflag"]) &&
                              (String.Compare(form["unsubmitflag"], "true", StringComparison.OrdinalIgnoreCase) == 0
                                  ? true
                                  : false);

            //check the supporting file format and content not empty
            var fileContent = form["fileContent"];
            var supportedFileFormatIdentifier =
                new Dictionary<string, string> {
                    {
                        "openxmlformats-officedocument.spreadsheetml.sheet","xlsx"
                    },
                    {
                        "application/vnd.ms-excel", "xls"
                    }
                };
            if (!(supportedFileFormatIdentifier.Keys.Any(s => fileContent.IndexOf(s, StringComparison.InvariantCulture) != -1) && !string.IsNullOrEmpty(fileContent)))
            {
                Error = "Incorrect file format.";
                return Json(false);
            }
            string fileFormatIdentifier =
                supportedFileFormatIdentifier.Keys.First(s => fileContent.IndexOf(s, StringComparison.InvariantCulture) != -1);

            //check wheather the uploaded file is base64 encoding.
            const string separatorIdentitier = "base64,";
            if (fileContent.IndexOf(separatorIdentitier, StringComparison.InvariantCulture) == -1)
            {
                Error = "Not a base64 encoding format.";
                return Json(false);
            }

            //remove encoding header.
            var headerPosition = fileContent.IndexOf(separatorIdentitier, StringComparison.InvariantCulture) + separatorIdentitier.Length;
            fileContent = fileContent.Remove(0, headerPosition);
            var bytes = Convert.FromBase64String(fileContent);

            //parse excel file.
            var excel = new ExcelParser.ExcelParser();
            List<PurchaseOrderWithLineItems> parsedResults = null;

            if (supportedFileFormatIdentifier[fileFormatIdentifier] == "xlsx")
            {
                parsedResults = excel.Parse(new MemoryStream(bytes));
            }
            else if (supportedFileFormatIdentifier[fileFormatIdentifier] == "xls")
            {
                var tempExcelStorePath = ConfigurationManager.AppSettings["ExcelTempStore"];
                var tempFileName = Guid.NewGuid().ToString() + "_" + DateTime.Now.ToString();

                var tempFile = Path.Combine(tempExcelStorePath, tempFileName.Replace('/', '_').Replace('\\', '_').Replace(':', '_').Replace(' ', '_'));

                System.IO.File.WriteAllBytes(tempFile, bytes);
                parsedResults = excel.ParseXls(tempFile);
            }

            //return if excel parser couldn't get anything back.
            if (parsedResults == null)
            {
                if (excel.IssueAndErrors.Any())
                {
                    return Json(new { source = "excel", errors = excel.IssueAndErrors });
                }
                else
                {
                    Error = "No PO has been parsed from the excel template, please use the latest template";
                    return Json(null);
                }
            }

            //return if excel parser meet any critical error.
            if (excel.IssueAndErrors.Any(n => n.Severity == Severity.Critical))
            {
                return Json(new { source = "excel", errors = excel.IssueAndErrors.Where(n => n.Severity == Severity.Critical).ToList() });
            }

            //in case order header usage date is empty in excel.
            var now = DateTime.Now;
            parsedResults.ForEach(n => { n.OrderHeader.UsageDate = n.OrderHeader.UsageDate ?? now; });

            //prepare distinct agreement request for agreement search api call.
            var agreementRequests =
                (from item in parsedResults.EmptyIfNull()
                 group item by new
                 {
                     AgreementNumber = item.OrderHeader.AgreementNumber,
                     UsageDate = item.OrderHeader.UsageDate
                 }
                into agreementNumberGroup
                select new AgreementRequest
                {
                    AgreementNumber = agreementNumberGroup.Key.AgreementNumber,
                    LookUpDate = agreementNumberGroup.Key.UsageDate ?? new DateTime()
                }).ToList();

            //call agreement search api.
            var agreements = new List<KeyValuePair<string, Agreement>>();
            foreach (var request in agreementRequests.EmptyIfNull())
            {
                var agreement = await PortalUnitOfWork.SearchAgreement(request);
                agreements.Add(new KeyValuePair<string, Agreement>(request.AgreementNumber, agreement.FirstOrDefault()));
            };

            //return if any agreement doesn't exist.
            if(agreements.Any(n => n.Value == null))
            {
                return Json(new { source = "agreement", errors = agreements.Where(n => n.Value == null).Select(n => n.Key).ToList()});
            };

            List<Agreement> agreementsForSearch = agreements.Where(n => n.Value != null).Select(n => n.Value).ToList();
            var ret = await PortalUnitOfWork.BulkUploadSave(agreementsForSearch , parsedResults, unsubmitflag, MyPrincipal.Current.UserName);

            const string composite = "{0}  draft order saved; {1} order submitted.";
            var saved = ret.EmptyIfNull().Count(n => n.DraftOrderId != null);
            var submitted = ret.EmptyIfNull().Count(n => n.Id != null);
            Message = string.Format(composite, saved, submitted);

            return Json(new { source = "batch", orders = ret });
        }

        [HttpPost]
        public async Task<JsonResult> ValidateShipToAddress(Shipment shipment)
        {
            var res = await PortalUnitOfWork.ValidateShipToAddress(shipment);
            return Json(res);
        }
    }
}