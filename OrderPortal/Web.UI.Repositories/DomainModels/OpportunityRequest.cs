using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.Models
{
    public class OpportunityRequest
    {
               
         public string EndCustomerNumber { get; set; }
         public string AgreementNumber { get; set; }
         public int AgreementId { get; set; }
         public int POAgreementId { get; set; }
         public DateTime LookupDate { get; set; }
         public string LocaleCode { get; set; }
         public string OrgGuid { get; set; }
         public string PublicCustomerNumber { get; set; }
         public string POAgreementNumber { get; set; }
         public string ProductSearchString { get; set; }
         public int PageSize { get; set; }
         public int PageNumber { get; set; }
         public string SortColumn { get; set; }
    }
}
