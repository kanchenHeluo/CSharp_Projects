using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.UI.Repositories.DomainModels;

namespace Web.UI.Models
{
    public class OrderSearchResult : OrderHeader
    {

        public string StatusName { get; set; }

        public string DirectCustomerName { get; set; }

        public string InDirectCustomerName { get; set; }

        public string SalesLocationName { get; set; }

        public string ProgramName { get; set; }
    }
}