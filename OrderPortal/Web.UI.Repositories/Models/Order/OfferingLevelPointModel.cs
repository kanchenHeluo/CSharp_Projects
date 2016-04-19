using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Repositories.Models
{
    [Serializable]
    public class OfferingLevelPointModel
    {
        public string OfferingLevel { get; set; }

        public int Point { get; set; }

        public string Pool { get; set; }

        public decimal UnitPrice { get; set; }
    }
}