using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.Models
{
   public  class Offering
    {

        public string ColId { get; set; }
        public string AgreementOfferingID { get; set; }
        public string AgreementID { get; set; }
        public string ProgramOfferingCode { get; set; }
        public string OfferingLevelCode { get; set; }
        public string LicensePoolID { get; set; }
        public string ParentMasterOfferingID { get; set; }
        public string StartEffectiveDate { get; set; }
        public string ExpectedEndEffectiveDate { get; set; }
    }
}
