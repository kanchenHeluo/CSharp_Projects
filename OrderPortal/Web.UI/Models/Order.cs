using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Common.Extensions;
using Web.UI.Repositories.Models;
using Web.UI.ServiceGateway.DraftOrderServiceProxy;

namespace Web.UI.Models
{
    public class Order
    {
        public long? Id { get; set; }
        public long? DraftOrderId { get; set; }
        public string OrderName { get; set; }
        public int? AgreementId { get; set; } 
        public string AgreementNumber { get; set; }
        public string EndCustomerNumber { get; set; }
        public string DirectCustomerNumber { get; set; }
        public string EndCustomerName { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string SeconderyPurchaseOrderNumber { get; set; }
        public string PurchaseOrderTypeCode { get; set; }
        public string PricingCurrencyCode { get; set; }
        public string PricingCountryCode { get; set; }
        public decimal TotalExtendedAmount { get; set; }
        public string SalesLocationCode { get; set; }
        public string ProgramCode { get; set; }
        public string SourceCode { get; set; }
        public string SourceSystem { get; set; }
        public string CreatedUser { get; set; } 
        public string ModifiedUser { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool ValidateFlag { get; set; }
        public DateTime? UsageDate { get; set; }
        public string AssignedTo { get; set; }
        public string LockedBy { get; set; }
        public bool LockedFlag { get; set; }
        public string IndirectCustomerNumber { get; set; }
        public string UserComment { get; set; }
        public string UserNotes { get; set; }
        public List<DraftOrderComment> Comments { get; set; }
        public string CarrierAccountNumber { get; set; } 
        public string CarrierCode { get; set; } 
        public string Reference { get; set; } 
        public long? PurchaseOrderShipToId { get; set; } 
        public string PurchaseOrderStatusCode { get; set; }
        public Guid? CorrelationId { get; set; }

#if used
        public bool BillOnAnniversary { get; set; }
        public bool CanPartialBill { get; set; }
        public string CentralSalesTaxValue { get; set; }
        public bool CreatedFromInvalidPurchaseOrder { get; set; }
        public string CustomerTypeCode { get; set; }
        public string DeliveryMechanismCode { get; set; }
        public string EdiTradingPartnerCode { get; set; }
        public string EndCustomerPublicCustomerNumber { get; set; }
        public Guid HeaderGuid { get; set; }
        public string InCultureCode { get; set; }
        public string IndirectPurchaseOrderNumber { get; set; }
        public bool InvIsLock { get; set; }
        public string InvLockedBy { get; set; }
        public bool InvalidEmailFlag { get; set; }
        public string InvalidPOReason { get; set; }
        public bool IsAnniversaryOrder { get; set; }
        public bool IsAnnualOrder { get; set; }
        public bool IsCocPurchaseOrder { get; set; }
        public bool IsLocked { get; set; }
        public bool? IsRelatedPurchaseOrder { get; set; }
        public int IssueId { get; set; }
        public string Name { get; set; }
        public string OperationsCenterCode { get; set; }
        public string OrgGuid { get; set; }
        public bool PriceByNewLevel { get; set; }
        public string ProposalId { get; set; }
        
        public DateTime? PurchaseOrderStatusDate { get; set; }
        public int PurchaseOrderStatusRet { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? RequestDate { get; set; }
        public long? RequestedOrderId { get; set; }
        public int? SoftwareAdvisorId { get; set; }

        public string TransactionSetPurposeCode { get; set; }
#endif
    }
}