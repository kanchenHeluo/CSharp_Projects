using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallBackTool.Converter.Models
{
    public class MPCContactCRMModel : ICRMModel
    {
        #region properties
        [DefaultValue("")]
        public string Address1_City { get; set; }
        [DefaultValue("")]
        public string Address1_Country { get; set; } 
        [DefaultValue("")]
        public string Address1_Line1 { get; set; }
        [DefaultValue("")]
        public string Address1_Line2 { get; set; }
        [DefaultValue("")]
        public string Address1_Line3 { get; set; }
        [DefaultValue("")]
        public string Address1_PostalCode { get; set; }
        [DefaultValue("")]
        public string Address1_StateOrProvince { get; set; }
        [DefaultValue("")]
        public Guid ContactId { get; set; }
        [DefaultValue("")]
        public string EMailAddress1 { get; set; }
        [DefaultValue("")]
        public string EmployeeId { get; set; }
        [DefaultValue("")]
        public string FirstName { get; set; }
        [DefaultValue("")]
        public string JobTitle { get; set; }
        [DefaultValue("")]
        public string LastName { get; set; }
        [DefaultValue("")]
        public string MiddleName { get; set; }
        [DefaultValue("")]
        public string MobilePhone { get; set; }
        [DefaultValue("")]
        public string Telephone1 { get; set; }
        [DefaultValue("")]
        public string Action { get; set; }
        #endregion

        public string ToString()
        {            
            string result = JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.None,
                TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple,
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
            var jsonResult = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(result);
            //jsonResult.Remove("Action");
            result = JsonConvert.SerializeObject(jsonResult);

            return result;
        }

    }
}
