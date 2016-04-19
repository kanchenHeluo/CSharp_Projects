using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.DomainModels
{
    public class Agreement
    {
        /// <summary>
        /// AgreementId - Represents Affiliate or Enrollment contract's unique numeric system generated value
        /// </summary>
        public int AgreementId { get; set; }
        /// <summary>
        /// AgreementNumber - Represents Affiliate or Enrollment contract's unique alpha-numeric value 
        /// </summary>
        public string AgreementNumber { get; set; }
        /// <summary>
        /// PurchaseAgreementId - Represents Master in Select Plus or Enrollment contract's unique numeric system generated value
        /// </summary>
        public int PurchaseAgreementId { get; set; }
        /// <summary>
        /// PurchaseAgreementNumber - Represents Master in Select Plus or Enrollment contract's unique alpha-numeric value 
        /// </summary>
        public string PurchaseAgreementNumber { get; set; }

        /// <summary>
        /// Gets or sets program code
        /// </summary>

        public string ProgramCode { get; set; }

        /// <summary>
        /// Gets or sets program name
        /// </summary>

        public string ProgramName { get; set; }

        /// <summary>
        /// Gets or sets primary PCN 
        /// </summary>

        public string EndCustomerNumber { get; set; }

        /// <summary>
        /// Gets or sets the BillToPCN
        /// </summary>

        public string PartnerNumber { get; set; }

        /// <summary>
        /// Gets or sets customer name  
        /// </summary>

        public string EndCustomerName { get; set; }

        /// <summary>
        /// Gets or sets Contract Type Code
        /// </summary>
        public string ContractTypeCode { get; set; }

        /// <summary>
        /// Gets or sets Pricing Country Code
        /// </summary>

        public string PricingCountryCode { get; set; }

        /// <summary>
        /// Gets or sets next anniversary date 
        /// </summary>

        public DateTime? NextAnniversaryDate { get; set; }

        /// <summary>
        /// Gets or sets currency code  
        /// </summary>

        public string CurrencyCode { get; set; }

        /// <summary>
        /// Gets or sets country name  
        /// </summary>

        public string CountryName { get; set; }

        /// <summary>
        /// Gets or sets next compliance start date 
        /// </summary>

        public DateTime? ComplianceStart { get; set; }

        /// <summary>
        /// Gets or sets next compliance end date 
        /// </summary>

        public DateTime? ComplianceEnd { get; set; }

        /// <summary>
        /// Gets or sets IsMSFinancedFlag
        /// </summary>

        public bool? IsMSFinancedFlag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the flag of Enterprise Wide CAL is true
        /// </summary>

        public bool? IsEntWideCalFlag { get; set; }

        /// <summary>
        /// Gets or sets agreement year 
        /// </summary>

        public int AgreementYear { get; set; }

        /// <summary>
        /// Gets or sets device count  
        /// </summary>

        public int DeviceCount { get; set; }

        /// <summary>
        /// Gets or sets user count  
        /// </summary>

        public int UserCount { get; set; }

        /// <summary>
        /// Gets or sets the reduced anniversary date.
        /// </summary>

        public DateTime? ReducedAnniversaryDate { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether is current year compliant.
        /// </summary>

        public bool IsCurrentYearCompliant { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is current year compliant.
        /// </summary>

        public bool IsPriorYearCompliant { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is extended compliance.
        /// </summary>

        public bool IsExtendedCompliance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is compliant.
        /// </summary>

        public bool IsCompliant { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether is complex deal.
        /// </summary>

        public bool IsComplexDeal { get; set; }



        public bool IsAffiliate { get; set; }

        /// <summary>
        /// Gets or sets the Current Year anniversary date.
        /// </summary>

        public DateTime? CurrentAnniversaryStart
        { get; set; }

        /// <summary>
        /// Gets or sets the Agreement End Effective date.
        /// </summary>

        public DateTime? EndEffectiveDate
        { get; set; }

        public DateTime? StartEffectiveDate
        { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is first annual order.
        /// </summary>

        public bool IsFirstAnnualOrder { get; set; }

        /// <summary>
        /// Gets or sets the quick start compliance end date.
        /// </summary>

        public DateTime? QuickStartComplianceEndDate { get; set; }

        /// <summary>
        /// Gets or sets the is amendment signed.
        /// </summary>

        public bool? IsAmendmentSigned { get; set; }

        /// <summary>
        /// Gets or sets the quick start line item count.
        /// </summary>

        public int? QuickStartLineItemCount { get; set; }

        /// <summary>
        /// Gets or sets the pricing not locked count.
        /// </summary>

        public int? PricingNotLockedCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is bec order available.
        /// </summary>

        public bool IsBECOrderAvailable { get; set; }

        /// <summary>
        /// Gets or sets the min quick start coverage start date.
        /// </summary>

        public DateTime? QuickStartCoverageStartDate { get; set; }

        /// <summary>
        /// Gets or sets all the extended attributes of agreement details.
        /// </summary>

        public Dictionary<string, string> ExtendedProperty { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is IsLotus available.
        /// </summary>

        public bool IsLotus { get; set; }//Fareast\v-nanewa: VSTF#584017 MQR14 LOTUS Requirement

        /// <summary>
        /// Gets or sets the IsEUDraftAnniversaryOrder
        /// </summary>

        public bool IsEUDraftAnniversaryOrder { get; set; }

        /// <summary>
        /// Gets or sets CustomerLocationCode
        /// </summary>

        public string CustomerLocationCode { get; set; }

        /// <summary>
        /// Gets or sets CustomerLocationName
        /// </summary>

        public string CustomerLocationName { get; set; }

        /// <summary>
        /// Gets or sets ComplianceWindow
        /// </summary>

        public bool? ComplianceWindow { get; set; }

        /// <summary>
        /// Gets or sets the extended compliance window.
        /// </summary>

        public bool? ExtendedComplianceWindow { get; set; }

        /// <summary>
        /// Gets or sets the extended compliance window.
        /// </summary>
        public string PriorYearComplianceStatus { get; set; }

    }
}
