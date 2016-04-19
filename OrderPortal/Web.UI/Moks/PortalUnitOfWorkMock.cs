using Microsoft.Ajax.Utilities;
using Microsoft.IT.Licensing.Entity.DomainData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.UI.Models;
using Web.UI.Repositories;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Interfaces;
using Web.UI.Repositories.Models;
using Agreement = Web.UI.Models.Agreement;
using DomainItem = Web.UI.Repositories.Models.DomainItem;
using ExcelParser;
using Web.UI.ServiceGateway.OrderServiceProxy;

namespace Web.UI.UnitOfWork
{
    public class PortalUnitOfWorkMock : IPortalUnitOfWork
    {
        PortalUnitOfWork real = new PortalUnitOfWork()
        {
            OrderRepository = new OrderRepository(),
            ProductRepository = new ProductRepository(),
            AgreementRepository = new AgreementRepository()
        };
        #region dummy

        readonly Agreement _agreement = new Agreement()
        {
            AgreementId = 1,
            AgreementNumber = "982342",
            ComplianceEnd = DateTime.Now,
            ComplianceStart = new DateTime(),
            CountryCode = "US",
            CountryName = "US",
            EndCustomerNumber = "PCN",
            ProgramCode = "NCP",
        };

        Web.UI.Models.Order _order = new Web.UI.Models.Order()
        {
            DirectCustomerNumber = "dddd",
            LockedBy = "User1",
            ProgramCode = "Pc",
            PurchaseOrderNumber = "222sd",
            PurchaseOrderTypeCode = "PO",
            UsageDate = new DateTime(),
        };

        LineItem _lineItem = new LineItem()
        {
            CoverageEndDate = DateTime.Now,
            CoverageStartDate = new DateTime(),
            BillingOptionCode = "Month",
            ProductId = 1,
            ItemName = "Product",
            Id = 1,
            LineItemType = "New Order",
            PartNumber = "bt1-00019",
            ProgramOfferingCode = "STU",
            UnitPrice = 20,
            PurchaseUnitTypeCode ="UT",
            PurchaseUnitQuantity ="12",
            POLIUsageDate = DateTime.Now,
        };

        #endregion

        public IAgreementRepository AgreementRepository { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IOrderRepository OrderRepository { get; set; }
        public IUserRepository UserRepository { get; set; }

        public Task<IEnumerable<Agreement>> SearchAgreement(AgreementRequest searchCriteria)
        {
            return Task.FromResult(Enumerable.Repeat(_agreement, 1));
        }

        public Task<Agreement> GetAgreementDetails(string agreementNumber)
        {
            return Task.FromResult(_agreement);
        }

        public Task<IEnumerable<LineItem>> SearchTrueUpOpportunity(OpportunityRequest searchCriteria)
        {
            return Task.FromResult((new List<LineItem> { _lineItem, _lineItem }).Select(t => t));
        }

        public  Task<IEnumerable<LineItem>> SearchStepUpOpportunity(OpportunityRequest searchCriteria)
        {
            return Task.FromResult((new List<LineItem> { _lineItem, _lineItem }).Select(t => t));
            //return await real.SearchStepUpOpportunity(searchCriteria);
        }

        public Task<IEnumerable<LineItem>> SearchRenewalOpportunity(OpportunityRequest searchCriteria)
        {
            return Task.FromResult((new List<LineItem> { _lineItem, _lineItem }).Select(t=>t));
            //return await real.SearchRenewalOpportunity(searchCriteria);
        }

        public Task<IEnumerable<LineItem>> SearchHistoryLineItem(int agreementId, string endCustomerNumber)
        {
            throw new NotImplementedException();
        }

        public LineItem SearchSku(string partNumber, int agreementId, string programCode, string poType, DateTime? usageDate)
        {
            return _lineItem;
        }

        public Task<SearchResult<LineItem>> SearchSku(ProductRequest productRequest)
        {
            return null;
        }

        public Task<OrderHeaderAttributes> GetOrderHeaderAttributes(int agreementId, DateTime lookUpDate, string localeCode)
        {
            var orderHeaderAttributes = new OrderHeaderAttributes
            {
                IndirectPartners = new List<DomainItem> {new DomainItem() {Code = "In 1", Name ="Indirect Partner 1"}},
                PurchaseOrderTypes = new List<DomainItem>
                    {
                        new DomainItem() {Code = "NE", Name = "New Order"},
                        new DomainItem() {Code = "TUP", Name = "TrueUp Order"}
                    },
                DirectPartners = new List<DomainItem>()
                {
                    new DomainItem(){Name = "Partner 1", Code="P1"},
                    new DomainItem(){Name = "Partner 2", Code="P2"}
                }

            };
            return Task.FromResult(orderHeaderAttributes);
         
        }

        public KeyValuePair<DateTime, DateTime> GetCoverageDate(string programType, DateTime poliUsageDate,
            DateTime? currentAnvDt,
            DateTime? endEffectiveDate, DateTime coverageEndDate, string subscriptionMonth, string billinOptionCode)
        {
            return new KeyValuePair<DateTime, DateTime>(DateTime.MinValue, DateTime.MaxValue);    
        }

        public Task<LineItemAttributes> GeLineItemAttributes(LineItemRequest lineItemRequest)
        {
            var orderItemAttributes = new LineItemAttributes
            {
                PurchaseUnitQuantities = new List<DomainItem>() {new DomainItem() {Code = "12", Name = "MT"}},
                PurchaseUnitTypes = new List<DomainItem>()
                    {
                        new DomainItem() {Code = "MON", Name = "Months"},
                        new DomainItem() {Code = "12", Name = "12 Months"}
                    },
                ProgramOfferings = new List<DomainItem>()
                    {
                          new DomainItem() {Code = "CUS", Name = "CUS"},
                        new DomainItem() {Code = "STU", Name = "STU"} 
                      
                    },
                BillingOptions = new List<BillingOption>() 
                    { 
                        new BillingOption(){Name="Annual Billing", Code="AE"},
                        new BillingOption(){Name="Monthly Billing", Code="ME"}
                    }
            };

            return Task.FromResult(orderItemAttributes);
        }

        public Task<long> SaveDraftOrder(Agreement agreement, Order order, List<LineItem> orderLineitems, string userName, Guid correlationId)
        {
            return real.SaveDraftOrder(agreement, order, orderLineitems, userName, Guid.NewGuid());
        }

        public Task<IEnumerable<KeyValuePair<Order, IEnumerable<LineItem>>>> GetDraftOrder(long? draftOrderId, string pcnFilter, string userName)
        {
            return real.GetDraftOrder(draftOrderId, pcnFilter, userName);
        }

        public Task<IEnumerable<Order>> BulkUploadSave(List<Agreement> agreements, List<PurchaseOrderWithLineItems> excel, bool unsubmitflag, string userName)
        {
            return real.BulkUploadSave(agreements, excel, unsubmitflag, userName);
        }

        public Task<PriceAtNewLevel> GetPriceEstimate(LineItem lineItem, Agreement agreement, string poType)
        {
            return Task.Factory.StartNew<PriceAtNewLevel>(() => new PriceAtNewLevel() { SystemPrice = 99.99M, PurchaseUnitQuantity = -1 });
        }

        public Task<long> CreateOrder(Agreement agreement, Order order, List<LineItem> lineItems, Shipment shipTo, string userName)
        {
            throw new NotImplementedException();
        }

        public Task<long> LockDraftOrder(long draftOrderId, string userName, int maxLockMinutes)
        {
            return real.LockDraftOrder(draftOrderId, userName, maxLockMinutes);
        }

        public Task<long> UnlockDraftOrder(long draftOrderId, string userName, int maxLockMinutes)
        {
            return real.UnlockDraftOrder(draftOrderId, userName, maxLockMinutes);
        }

        public Task<long> DeleteDraftOrder(long draftOrderId, string userName, int maxLockMinutes)
        {
            return real.DeleteDraftOrder(draftOrderId, userName, maxLockMinutes);
        }

        public Task<List<string>> ValidateDraftOrder(Agreement agreement, Order order, List<LineItem> orderLineitems, string userName)
        {
            return real.ValidateDraftOrder(agreement, order, orderLineitems, userName);
        }

        public async Task<SaveShipmentAddressResult> SaveShipmentAddress(Shipment shipment, string userName)
        {
            return null;
        }

        public async Task<List<Shipment>> GetAllShipmentList(int agreementId)
        {
            return null;
        }

        public async Task<bool> DeleteShipmentAddress(long id)
        {
            return true;
        }

        public async Task<KeyValuePair<DateTime, List<string>>> ValidateShipToAddress(Shipment shipment)
        {
            return new KeyValuePair<DateTime, List<string>>(DateTime.Now, null);
        }

        public async Task<UserPreference> GetUserPreference(UserPreference request)
        {
            
            return null;
        }
        public async Task<bool> SetUserPreference(UserPreference request)
        {
            
            return true;
        }

        public async Task<IEnumerable<Customer>> GetCustomers(AgreementRequest request)
        {

            return null;
        }

        public async Task<SearchResult<KeyValuePair<Order, IEnumerable<LineItem>>>> GetOrdersWithStatus(string pcnFilter,
            string customerNumber, long? id, string userName, int pageNumber, int pageSize, string[] purchaseOrderStatusCodeTable)
        {
            return null;
        }
    }
}