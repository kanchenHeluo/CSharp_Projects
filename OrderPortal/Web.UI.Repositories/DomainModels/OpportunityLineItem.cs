using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.DomainModels
{
    public class OpportunityLineItem
    {
        public int OpportunityId { get; set; }
        public string OpportunityTypeCode { get; set; }
        public int LineItemId { get; set; }
        public string LineItemTypeCode { get; set; }
        public int AgreementId { get; set; }
        public int PurchaseAgreementId { get; set; }
        public string PurchaseAgreementNumber { get; set; }
        public string EndCustomerNumber { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string PurchaseOrderTypeCode { get; set; }
        public int ItemId { get; set; }
        public string PartNumber { get; set; }
        public string ItemName { get; set; }
        public bool IsOlsItem { get; set; }
        public string ProductTypeCode { get; set; }
        public string ProductFamilyCode { get; set; }
        public string PoolCode { get; set; }
        public string ProgramOfferingCode { get; set; }
        public bool HasMultipleOfferings { get; set; }
        public string BillingOptionCode { get; set; }
        public DateTime ProductUsageDate { get; set; }
        public int POLineItemID { get; set; }
        public int ReferenceId { get; set; }
        public int QuantityOrdered { get; set; }
        public int QuantityAvailable { get; set; }
        public string UsageCountryCode { get; set; }
        public DateTime CoverageStartDate { get; set; }
        public DateTime CoverageEndDate { get; set; }
        public string StatusCode { get; set; }
        public bool IsPriced { get; set; }
        public bool IsEnterpriseWideCal { get; set; }
        public bool IsInvalidLineItem { get; set; }
        public DateTime PricingPeriodDate { get; set; }
        public string PurchaseUnitCode { get; set; }
        public int PricePathID { get; set; }
        public string PurchaseUnitTypeCode { get; set; }
        public string PurchaseUnitQuantity { get; set; }
        public bool MultipleTransitionAssociatedFlag { get; set; }
        public bool FromReservation { get; set; }
        public string PurchasePeriodCode { get; set; }
        public DateTime PurchaseOrderUsageDate { get; set; }
        public bool IsCoterminous { get; set; }
        public decimal UnitPrice { get; set; }

        public int ParentItemId { get; set; }
        public string ParentPartNumber { get; set; }
        public string ParentItemName { get; set; }

        public Web.UI.ServiceGateway.OrderServiceProxy.VLItemInfo ItemDetails { get; set; }
    }
}
