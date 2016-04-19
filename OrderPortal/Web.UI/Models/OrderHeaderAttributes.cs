using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.UI.Common;
using Web.UI.Repositories.Models;

namespace Web.UI.Models
{
    public class OrderHeaderAttributes :ObjectBase
    {
        public List<DomainItem> PurchaseOrderTypes { get; set; }

        public List<DomainItem> IndirectPartners { get; set; }

        public List<DomainItem> DirectPartners { get; set; }

        public string AvailablePoolIds { get; set; }

        public string ProgramOfferingIds { get; set; }
    }
}