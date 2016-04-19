using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Repositories.Models
{
    [Serializable]
    //TODO: remove this class
    public class AgreementValidateParams
    {
        public string AgreementNumber { get; set; }
        public DateRange UsageStartDate { get; set; }
    }
}