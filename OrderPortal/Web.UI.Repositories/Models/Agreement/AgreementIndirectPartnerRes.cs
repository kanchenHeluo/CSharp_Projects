using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Repositories.Models
{
    //TODO: remove this class
    public class AgreementIndirectPartnerRes
    {
        public DateTime IndirectPartnerActualEndEffectiveDate { get; set; }
        public string IndirectPartnerBusinessName { get; set; }
        public int? IndirectPartnerCustomerId { get; set; }
        public string IndirectPartnerCustomerTypeCode { get; set; }    
        public DateTime IndirectPartnerExpectedEndEffectiveDate { get; set; }      
        public string IndirectPartnerPublicCustomerNumber { get; set; } 
        public DateTime IndirectPartnerStartEffectiveDate { get; set; }
    }
}