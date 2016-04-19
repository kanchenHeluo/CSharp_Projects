using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.Models
{
    public class SpecialPricing
    {
        public string ColId { get; set; }
        public string AgreementOfferingID { get; set; }
        public string AgreementID { get; set; }
        public string SpecialPriceTypeCode { get; set; }
        public string PartNumber { get; set; }
        public string ProductFamilyCode { get; set; }
        public string DiscountOrCreditPercent { get; set; }
        public string FixedPrice { get; set; }
        public string ProtectionTypeCode { get; set; }
        public string StartEffectiveDate { get; set; }
        public string EndEffectiveDate { get; set; }
    }
}
