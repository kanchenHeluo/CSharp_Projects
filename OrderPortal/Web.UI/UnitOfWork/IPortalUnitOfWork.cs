using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.UI.Models;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Interfaces;
using Web.UI.Repositories.Models;
using Agreement = Web.UI.Models.Agreement;
using ExcelParser;
using Web.UI.ServiceGateway.OrderServiceProxy;


namespace Web.UI.UnitOfWork
{
    public interface IPortalUnitOfWork
    {
        Task<IEnumerable<Agreement>> SearchAgreement(AgreementRequest searchCriteria);
        Task<Agreement> GetAgreementDetails(string agreementNumber);
        Task<IEnumerable<LineItem>> SearchTrueUpOpportunity(OpportunityRequest searchCriteria);
        Task<IEnumerable<LineItem>> SearchStepUpOpportunity(OpportunityRequest searchCriteria);
        Task<IEnumerable<LineItem>> SearchRenewalOpportunity(OpportunityRequest searchCriteria);
        Task<IEnumerable<LineItem>> SearchHistoryLineItem(int agreementId, string endCustomerNumber);
        Task<OrderHeaderAttributes> GetOrderHeaderAttributes(int agreementId, DateTime lookUpDate, string localeCode);
        Task<LineItemAttributes> GeLineItemAttributes(LineItemRequest lineItemRequest);
        Task<SearchResult<LineItem>> SearchSku(ProductRequest productRequest);
        Task<PriceAtNewLevel> GetPriceEstimate(LineItem lineItem, Agreement agreement, string poType);
        KeyValuePair<DateTime, DateTime> GetCoverageDate(string programType, DateTime poliUsageDate,DateTime? currentAnvDt,
            DateTime? endEffectiveDate, DateTime coverageEndDate, string subscriptionMonth, string billinOptionCode);
        Task<long> CreateOrder(Agreement agreement, Models.Order order, List<LineItem> lineItems, Shipment shipTo, string userName);
        Task<long> LockDraftOrder(long draftOrderId, string userName, int maxLockMinutes = 30);
        Task<long> UnlockDraftOrder(long draftOrderId, string userName, int maxLockMinutes = 30);
        Task<long> DeleteDraftOrder(long draftOrderId, string userName, int maxLockMinutes = 30);
        Task<List<string>> ValidateDraftOrder(Agreement agreement, Order order, List<LineItem> orderLineitems, string userName);
        Task<long> SaveDraftOrder(Agreement agreement, Order order, List<LineItem> orderLineitems, string userName, Guid correlationId);
        Task<SaveShipmentAddressResult> SaveShipmentAddress(Shipment shipment, string userName);
        Task<List<Shipment>> GetAllShipmentList(int agreementId);
        Task<bool> DeleteShipmentAddress(long id);
        Task<IEnumerable<KeyValuePair<Order, IEnumerable<LineItem>>>> GetDraftOrder(long? draftOrderId, string pcnFilter, string userName);
        Task<IEnumerable<Order>> BulkUploadSave(List<Agreement> agreements, List<PurchaseOrderWithLineItems> excel, bool unsubmitflag, string userName);
        Task<KeyValuePair<DateTime, List<string>>> ValidateShipToAddress(Shipment shipment);
        Task<UserPreference> GetUserPreference(UserPreference request);
        Task<bool> SetUserPreference(UserPreference request);

        Task<IEnumerable<Customer>> GetCustomers(AgreementRequest request);

        Task<SearchResult<KeyValuePair<Order, IEnumerable<LineItem>>>> GetOrdersWithStatus(string pcnFilter,
                                                                                          string customerNumber,
                                                                                          long? id,
                                                                                          string userName,
                                                                                          int pageNumber,
                                                                                          int pageSize,
                                                                                          string[] purchaseOrderStatusCodeTable);
    }
}
