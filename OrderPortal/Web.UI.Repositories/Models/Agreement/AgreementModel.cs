using System;using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Web.UI.Repositories.Models
{
    public class AgreementModel
    {
        public AgreementModel()
        {
            ContractType = new DomainItem();
            Program = new DomainItem();
            AgreementType = new DomainItem();
            AgreementStatus = new DomainItem();
            PricingCurrency = new DomainItem();
            Country = new DomainItem();
            SalesLocation = new DomainItem();

            ContractTypes = new Collection<DomainItem>();
            Programs = new Collection<DomainItem>();
        }
        public DateTime ActualEndEffectiveDate { get; set; }
        public int? AffiliateAgreementID { get; set; }
        public int AgreementId { get; set; }
        public int AgreementYear { get; set; }
        public string AgreementNumber { get; set; }
        public DomainItem AgreementType { get; set; }
        public DomainItem AgreementStatus { get; set; }
        public string AgreementTradeStatusName { get; set; }
        public string BillingOptionsSupportedFlag { get; set; }
        public string BillToPCN { get; set; }
        // Confirm the following two identical to CoverageStart and Coverage End, which assembled ComplianceWindow
        public string CanHaveOrdersFlag { get; set; }
        public DateTime? ComplianceEnd { get; set; }
        public DateTime? ComplianceStart { get; set; }
        public DomainItem Country { get; set; }
        public string CurrencyCode { get; set; }
        public DomainItem ContractType { get; set; }
        public DateTime? CurrentAnniversaryStart { get; set; }
        public int EndCustomerID { get; set; }
        public string EndCustomerName { get; set; } // same with customer name?
        public string EndCustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public int DeviceCount { get; set; }
        public DateTime? EndEffectiveDate { get; set; }
        public Dictionary<string, string> ExtendedGetAgreement { get; set; }
        public DateTime FirstActivatedDate { get; set; }
        public DateTime FulfillmentExpectedEndDate { get; set; }
        public string HasCOCPAmendment { get; set; }
        public bool? IsAmendmentSigned { get; set; } // Duplicated with SignedAmendment, need to find a way to resolve it
        public string SignedAmendment { get; set; }
        public bool IsBECOrderAvailable { get; set; }
        public string IsCOCPAllowed { get; set; }
        public bool IsComplexDeal { get; set; }
        public bool IsCompliant { get; set; }
        public bool IsCurrentYearCompliant { get; set; }
        public bool? IsEntWideCALFlag { get; set; } // 
        public bool IsExtendedCompliance { get; set; }
        public bool IsFirstAnnualOrder { get; set; }
        public bool IsLotus { get; set; }
        public bool? IsMSFinancedFlag { get; set; } //
        public bool IsPriorYearCompliant { get; set; }
        public string LicenseAgreementTypeCode { get; set; }
        public DateTime? NextAnniversaryDate { get; set; }
        public int? PricingNotLockedCount { get; set; }
        public string OperationsCenterCode { get; set; }
        public string PricingCountryCode { get; set; }
        public DomainItem PricingCurrency { get; set; }
        public string PrimaryPCN { get; set; }
        public DomainItem Program { get; set; }
        public DateTime? QuickStartComplianceEndDate { get; set; }
        public DateTime? QuickStartCoverageStartDate { get; set; }
        public int? QuickStartLineItemCount { get; set; }
        public DateTime? ReducedAnniversaryDate { get; set; }
        public DateTime RenewalEndEffectiveDate { get; set; }
        public DateTime ResolvedEndDate { get; set; }
        public DomainItem SalesLocation { get; set; }
        public DateTime StartEffectiveDate { get; set; }
        public string TotalPricingRules { get; set; }
        public int UserCount { get; set; } 
        public List<AgreementOrderTypeRes> PurchaseOrderTypes { get; set; }
        public List<AgreementDirectPartnerRes> DirectChannelPartners { get; set; }
        public List<AgreementIndirectPartnerRes> IndirectChannelPartners { get; set; }
        public List<AgreementCentralSalesTaxTypeRes> CentralSalesTaxTypes { get; set; }

        // POET 
        public DateTime CoverageEnd { get; set; }
        public DateTime CoverageStart { get; set; }
        public bool Expired { get; set; }
        public int HasMultipleTransition { get; set; }
        
        public Collection<DomainItem> ContractTypes { get; set; }
        public Collection<DomainItem> Programs { get; set; } 
    }
}