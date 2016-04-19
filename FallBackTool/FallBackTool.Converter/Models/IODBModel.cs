using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallBackTool.Converter.Models
{
    public class IODBModel : IDBModel
    {
        #region Properties
        public string ActionCode { get; set; }
        public long InternalOrder { get; set; }
        public Guid InternalOrderGuid { get; set; }

        [Required]
        public string InternalOrderNbr { get; set; }
        public string InternalOrderStatusCode { get; set; }
        public string GLCompanyCode { get; set; }
        public string InternalOrderDesc { get; set; }
        public string ProfitCenterCode { get; set; }
        public DateTime ProfitCenterEndDate { get; set; }
        public string PhysicalOrderCompleteInd { get; set; }
        public string PhysicalOrderClosedInd { get; set; }
        public string MarkedDeleteInd { get; set; }

        public DateTime InternalOrderReleaseDate { get; set; }
        public DateTime InternalOrderCompletionDate { get; set; }
        public DateTime InternalOrderWorkBeginDate { get; set; }
        public DateTime InternalOrderWorkEndDate { get; set; }
        public string InternalOrderManagerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string TaxJurisdictionCode { get; set; }

        #endregion
    }
}
