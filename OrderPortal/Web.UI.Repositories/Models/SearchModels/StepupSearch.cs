using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.Models
{
   public class StepupSearch
    {
        public string ColId { get; set; }
        public string OpportunityId { get; set; }
        public string OpportunityTypeCode { get; set; }
        public string LineItemId { get; set; }
        public string LineItemTypeCode { get; set; }
        public string AgreementId { get; set; }
        public string POAgreementId { get; set; }
        public string AgreementNumber { get; set; }
        public string POAgreementNumber { get; set; }
        public string EndCustomerNumber { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string PurchaseOrderTypeCode { get; set; }
        public string ItemId { get; set; }
        public string PartNumber { get; set; }
        public string ItemName { get; set; }
        public string IsOlsItem { get; set; }
        public string ProductTypeCode { get; set; }
        public string ProductFamilyCode { get; set; }
        public string PoolCode { get; set; }
        public string ProgramOfferingCode { get; set; }
        public string HasMultipleOffering { get; set; }
        public string BillingOptionCode { get; set; }
        public string POLIUsageDate { get; set; }
        public string POLineItemID { get; set; }
        public string ReferencePOLIId { get; set; }
        public string QuantityOrdered { get; set; }
        public string QuantityAvailable { get; set; }
        public string UsageCountryCode { get; set; }
        public string CoverageStartDate { get; set; }
        public string CoverageEndDate { get; set; }
        public string StatusCode { get; set; }
        public string IsPriced { get; set; }
        public string IsEnterpriseWideCal { get; set; }
        public string IsInvalidLineItem { get; set; }
        public string PricingPeriodDate { get; set; }
        public string PurchaseUnitCode { get; set; }
        public int PricePathID { get; set; }
        public string PurchaseUnitTypeCode { get; set; }
        public string PurchaseUnitQuantity { get; set; }
        public string MultipleTransitionAssociatedFlag { get; set; }
        public string FromReservation { get; set; }
        public string ColListCheckSum { get; set; }
        public string PurchasePeriodCode { get; set; }
        public string POUsageDate { get; set; }
        //public string POLIUsageDateTime { get; set; }
        //public string IsCoterminous { get; set; }
    }
}
