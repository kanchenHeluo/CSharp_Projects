using System;
using System.Collections.Generic;
using Web.UI.Repositories.Models;
using Web.UI.ServiceGateway.OrderServiceProxy;
using System.Threading.Tasks;
using Web.UI.Repositories.DomainModels;

namespace Web.UI.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository
    {
        /// <summary>
        /// Get Renewal Opportunities from Order Service
        /// </summary>
        /// <param name="agreementNumber"></param>
        /// <param name="endCustomerNumber"></param>
        /// <param name="OrgGuid"></param>
        /// <returns></returns>
        Task<SearchResult<OpportunityItem>> GetRenewalOpportunties(OpportunityRequest orderParam);
        /// <summary>
        /// GetStepUpOpportunties From Order Service
        /// </summary>
        /// <param name="agreementNumber"></param>
        /// <param name="endCustomerNumber"></param>
        /// <param name="OrgGuid"></param>
        /// <returns></returns>

        Task<SearchResult<OpportunityItem>> GetNonResStepUpOpportunties(OpportunityRequest orderParam);

        Task<SearchResult<OpportunityItem>> GetTrueUpOpportunities(OpportunityRequest orderParam);


        Task<IEnumerable<DomainItem>> GetOrderHeaderAttributes(int agreementId);


        Task<IEnumerable<DomainItem>> GetLineItemAttributes(LineItemAttributeRequest localeid);

        Task<IEnumerable<CreateVLOrderResponse>> CreateOrder(OrderHeader orderHeader, List<OrderLineItem> lineItems, Shipment shipment,
            string userName);

        Task<PriceAtNewLevel> GetPriceEstimate(OrderLineItem lineItem, Agreement agreement, string poType);

        Task<long> LockDraftOrder(long draftOrderId, string userName, int maxLockMinutes);
        Task<long> UnlockDraftOrder(long draftOrderId, string userName, int maxLockMinutes);
        Task<long> DeleteDraftOrder(long draftOrderId, string userName, int maxLockMinutes);
        Task<List<string>> ValidateDraftOrder(OrderHeader order, List<OrderLineItem> orderLineitems, string userName);
        Task<long> SaveDraftOrder(OrderHeader order, List<OrderLineItem> orderLineitems, string userName, Guid correlationId);
        Task<SaveShipmentAddressResult> SaveShipmentAddress(Shipment shipment, string userName);
        Task<List<Shipment>> GetAllShipmentList(int agreementId);
        Task<bool> DeleteShipmentAddress(long id);
        Task<KeyValuePair<DateTime, List<string>>> ValidateShipToAddress(Shipment validateShipment);
        Task<IEnumerable<KeyValuePair<OrderHeader, IEnumerable<OrderLineItem>>>> GetDraftOrder(long? draftOrderId, string pcnFilter, string userName);
        /// <summary>
        /// Get Sales LocationsFrom Order Service
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<DomainItem>> GetSalesLocations();
        
        Task<SearchResult<KeyValuePair<OrderHeader, IEnumerable<OrderLineItem>>>> GetOrdersWithStatus(string pcnFilter,
            string customerNumber, long? id, string userName, int pageNumber, int pageSize, string[] purchaseOrderStatusCodeTable);
    }
}