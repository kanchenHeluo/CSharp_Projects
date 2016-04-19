using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Search.Models
{
    public class RequestCriteria
    {
        public string query{ get; set; }
        public Dictionary<string, string> requestParams{ get; set; }
        public string Url{ get; set; }
    }
}
