using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.UI.Repositories.Models;
using Web.UI.ServiceGateway.DraftOrderServiceProxy;

namespace Web.UI.Models
{
    public class LineItem
    {
        
         //Guid? LineItemGuid;
        public int Id { get; set; } 
        public string LineItemType { get; set; }
        public int ProductId { get; set; }
        public string PartNumber { get; set; }
        public int OriginalItemId { get; set; }
        public string OriginalPartNumber { get; set; }
        public string ItemName { get; set; }
        public string OriginalItemName { get; set; }
        public DateTime CoverageStartDate { get; set; }
        public DateTime CoverageEndDate { get; set; }
        public decimal UnitPrice { get; set; }
        public string UsageCountryCode { get; set; }

        public string CountryName { get; set; }

        public int QuantityOrdered { get; set; }
        public decimal ExtendedAmount { get; set; }
        public int QuantityAvailable { get; set; }

        public string BillingOptionCode { get; set; }

        public string PricingCurrencyCode { get; set; }
        public string PricingCountryCode { get; set; }
        
        public string ProgramOfferingCode { get; set; }
        public string ProductFamilyCode { get; set; }
        public string ProductFamilyName { get; set; }
        public string ProductTypeCode { get; set; }
        public string PurchaseUnitTypeCode { get; set; }
        public string PurchaseUnitQuantity { get; set; }
        public DateTime? POLIUsageDate { get; set; }
        public string SpecialDealNumber { get; set; }
        public string PoolName { get; set; }
        public string PoolCode { get; set; }
        public string CreatedBy { get; set; }
        public string FlagReason { get; set; }
        public string UserComment { get; set; }
        public bool IsStepUp { get; set; }
        public bool IsOlsItem { get; set; }
        public bool HasBillingOption { get; set; }
        public bool HasProgramOffering { get; set; }
        public List<DraftOrderComment> Comments { get; set; }

#if used    
        public int AgreementId { get; set; }
        public int PurchaseAgreementId { get; set; }
        public string PurchaseAgreementNumber { get; set; }
        public string EndCustomerNumber { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string PurchaseOrderTypeCode { get; set; }
        public string ProductTypeCode { get; set; }
        public string PoolCode { get; set; }
        public bool HasMultipleOfferings { get; set; }
        public DateTime ProductUsageDate { get; set; }
        public int POLineItemID { get; set; }
        public int ReferenceId { get; set; }
        public string StatusCode { get; set; }
        public bool IsPriced { get; set; }
        public bool IsEnterpriseWideCal { get; set; }
        public bool IsInvalidLineItem { get; set; }
        public string PurchaseUnitCode { get; set; }
        public int PricePathID { get; set; }
        public bool MultipleTransitionAssociatedFlag { get; set; }
        public bool FromReservation { get; set; }
        public string PurchasePeriodCode { get; set; }
        public bool IsCoterminous { get; set; }
#endif
    }
}