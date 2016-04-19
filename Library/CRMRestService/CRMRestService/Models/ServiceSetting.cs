using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMRestService.Models
{
    public class ServiceSetting
    {
        public ServiceSetting()
        {
            CrmResourceURL = ConfigurationManager.AppSettings["ida:CrmResourceURL"];
            CrmEntityName = ConfigurationManager.AppSettings["EntityName"];
            CrmSecondPrimaryKey = ConfigurationManager.AppSettings["CrmSecondPrimaryKey"];

            Retry = 0; //not enable this feature 
            MicrosoftAccountEnabled = (ConfigurationManager.AppSettings["MicrosoftAccountEnabled"] == null) ? false : bool.Parse(ConfigurationManager.AppSettings["MicrosoftAccountEnabled"]);
            CRMVersion = ConfigurationManager.AppSettings["CRMVersion"];
            Tenant = ConfigurationManager.AppSettings["ida:Tenant"];
            AadInstance = ConfigurationManager.AppSettings["ida:AadInstance"];
            ClientId = ConfigurationManager.AppSettings["ida:ClientId"];
            User = ConfigurationManager.AppSettings["ida:user"];
            Password = ConfigurationManager.AppSettings["ida:password"];
        }

        #region properties
        public Guid CacheAccountId { get; set; }

        public string CrmResourceURL { get; set; }
        public string CrmEntityName { get; set; }
        public string CrmSecondPrimaryKey { get; set; }

        public int Retry { get; set; }
        public bool MicrosoftAccountEnabled { get; set; }
        public string CRMVersion { get; set; }
        public string Tenant { get; set; }
        public string AadInstance { get; set; }
        public string ClientId { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        #endregion

    }
}
