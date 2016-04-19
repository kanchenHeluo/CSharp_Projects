using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Repositories.Models
{
    //TODO: remove this class
    public class AgreementDirectPartnerRes
    {
        public DateTime DirectPartnerActualEndEffectiveDate { get; set; }
        public string DirectPartnerBillToCustomerNumber { get; set; }
        public string DirectPartnerBusinessName { get; set; }
        public string DirectPartnerCustomerCurrencyCode { get; set; }
        public int? DirectPartnerCustomerID { get; set; }
        public string DirectPartnerCustomerNumber { get; set; }
        public string DirectPartnerCustomerSalesLocationCode { get; set; }
        public string DirectPartnerCustomerTypeCode { get; set; }
        public DateTime DirectPartnerExpectedEndEffectiveDate { get; set; }
        public string DirectPartnerPublicCustomerNumber { get; set; }
        public int? DirectPartnerRevenueSystemId { get; set; }
        public DateTime DirectPartnerStartEffectiveDate { get; set; }
    }
}