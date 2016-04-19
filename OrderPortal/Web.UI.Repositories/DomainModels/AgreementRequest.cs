using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.DomainModels
{
    public class AgreementRequest
    {
        public string PartnerNumber { get; set; }//login pcn
         public string EndCustomerNumber { get; set; }
         public string AgreementNumber { get; set; }
         public string EndCustomerName { get; set; }
         public DateTime LookUpDate { get; set; }
        public string SalesLocation { get; set; }
    }
    public class AgreementDetailsRequest
    {
            public string[] AgreementNumbers { get; set; }
            public Guid Guid { get; set; }

    }
}
