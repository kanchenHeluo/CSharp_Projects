using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Web.UI.Repositories.Models
{
   public  class AgreementRef
    {

        public string ColID { get; set; }
        public string Agreementid { get; set; }
        public string AgreementNumber { get; set; }
        public string AgreementTypeCode { get; set; }
        public string Affliate { get; set; }
        public string AffiliateSupportedFlag { get; set; }
        public string MainAgreementID { get; set; }
        public string MainAgreementNumber { get; set; }
        public string StartEffectiveDate { get; set; }
        public string AgreementEndDate { get; set; }
        public string RenewedFlag { get; set; }
        public string EndCustomerID { get; set; }
        public string EndCustomerNumber { get; set; }
        public string ProgramCode { get; set; }
        public string LicenseAgreementTypeCode { get; set; }
        public string PricingCountryCode { get; set; }
        public string PricingCurrencyCode { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifedDate { get; set; }
        public string ModifedBy { get; set; }
        public string SalesLocationCode { get; set; }
        public string DirectCustomerId { get; set; }
        public string DirectCustomerNumber { get; set; }
        public string CustomerTypeCode { get; set; }
        public string CustomerName { get; set; }
        public string OrganizationGUID { get; set; }
        public string SOAId { get; set; }
        public string SOANumber { get; set; } 
    }
}
