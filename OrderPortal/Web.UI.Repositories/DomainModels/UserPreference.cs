using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.DomainModels
{
    public class UserPreference
    {
        public string ApplicationId { get; set; }
        public string RequestId { get; set; }
        public string AccessorGuid { get; set; }
        public string UserCredential { get; set; }
        public string ApplicationGuid { get; set; }
        public string Module { get; set; }
        public string Language { get; set; }
        public string AddressFormat { get; set; }
        public string DateFormat { get; set; }
    }
}
