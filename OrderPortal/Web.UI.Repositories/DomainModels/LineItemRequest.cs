using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.UI.Repositories.DomainModels
{
    public class LineItemRequest
    {
        public string PartNumber { get; set; }
        public int ItemId { get; set; }
        public int AgreementId { get; set; }
        public DateTime LookupDate { get; set; }
        public int LocaleId { get; set; }
        public List<string> ProductTypeCodes { get; set; }
        public List<string> ProgramCodes { get; set; }
        public List<string> PurchaseOrderTypes { get; set; }
        public List<string> ProductFamilyCodes { get; set; }
   
    }
}
