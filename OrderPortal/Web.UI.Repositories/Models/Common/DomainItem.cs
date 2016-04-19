using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Repositories.Models
{
    public class DomainItem :IDomainObject
    {
        public string Name { get; set; }
        public string Code { get; set; }       
        public string Category { get; set; }
        public string ProgramCode { get; set; }
    }
}