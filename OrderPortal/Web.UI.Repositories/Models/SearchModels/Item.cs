using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.Models
{
    public class Item
    {

        public string ColId { get; set; }
        public string ItemID { get; set; }
        public string PartNumber { get; set; }
        public string ItemName { get; set; }
        public string ItemLegalName { get; set; }
        public string MaxcimPartDescription { get; set; }
        public string ItemStatusCode { get; set; }
        public string ItemStatusName { get; set; }
        public string PoolCode { get; set; }
        public string PoolName { get; set; }
        public string ProductFamilyCode { get; set; }
        public string ProductFamilyName { get; set; }
        public string ProductTypeCode { get; set; }
        public string ProductTypeName { get; set; }
        public string ReplacesPartNumber { get; set; }
        public string ReplacedByPartNumber { get; set; }
        public string RecurringMaintPartNumber { get; set; }
    }
}
