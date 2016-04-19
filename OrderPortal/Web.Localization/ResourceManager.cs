using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Localization
{
    public class ResourceManager
    {
        public static string GetString(string key)
        {
            var str = Strings.ResourceManager.GetString(key);
            return String.IsNullOrEmpty(str) ? key : str;
        }
    }
}
