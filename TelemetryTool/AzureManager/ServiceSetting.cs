using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureManager
{
    public class ServiceSetting
    {
        public ServiceSetting()
        {
            Tenant = ConfigurationManager.AppSettings["ida:Tenant"];
            AadInstance = ConfigurationManager.AppSettings["ida:AadInstance"];
            ClientId = ConfigurationManager.AppSettings["ida:ClientId"];
            User = ConfigurationManager.AppSettings["ida:User"];
            Password = ConfigurationManager.AppSettings["ida:Password"];
        }

        #region properties
        public string Tenant { get; set; }
        public string AadInstance { get; set; }
        public string ClientId { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        #endregion
    }
    
}
