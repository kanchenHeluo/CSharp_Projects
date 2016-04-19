using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.DomainModels
{
    public class ProductRequest :SearchRequest
    {
        public string PartNumber { get; set; }
        public string ProductTypeCode { get; set; }
        public string ProductFamilyCode { get; set; }
        public string PurchaseOrderTypeCode { get; set; }
        public int AgreementId { get; set; }
        public string AgreementNumber { get; set; }
        public string PoolIds { get; set; }
        public string ItemName { get; set; }
        public string ProgramOfferings { get; set; }
        public string ProgramCode { get; set; }

        public string SortColumn { get; set; }
        /// <summary>
        /// text that will Search in PartNumber,ItemName and ProductFmailyName
       
        public int LocaleId { get; set; }
        /// <summary>
        /// Page Size For ELastic Serch Index 
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Page Number For ELastic Serch Index 
        /// </summary>
        public int PageNumber { get; set; }
        public Boolean IncludeDetails { get; set; }
        public DateTime UsageDate { get; set; }
    }

   
}
