using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Repositories.Models
{
    [Serializable]
    //TODO: remove this class, replace with general search param
    public class AgreementSearchCriteria
    {
        public string CustomerAccountName { get; set; }
        public string CustomerAccountNumber { get; set; }
        public string AgreementNumber { get; set; }
        public DateRange UsageStartDate { get; set; }
    }
}