using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.UI.Common;
using Web.UI.Repositories.Models;
using Microsoft.IT.Licensing.Entity.DomainData;
using System.Collections.ObjectModel;

namespace Web.UI.Models
{
    public class LineItemAttributes :ObjectBase
    {
        public IEnumerable<Web.UI.Repositories.Models.DomainItem> PurchaseUnitTypes { get; set; }

        public IEnumerable<Web.UI.Repositories.Models.DomainItem> PurchaseUnitQuantities { get; set; }

        public IEnumerable<BillingOption> BillingOptions { get; set; }

        public IEnumerable<Web.UI.Repositories.Models.DomainItem> ProgramOfferings { get; set; }

        public bool IsShipmentEnabled { get; set; }
        public bool IsOls { get; set; }
    
    }
}