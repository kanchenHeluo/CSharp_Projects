using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Repositories.Models
{
    [Serializable]
    public class DateRange
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}