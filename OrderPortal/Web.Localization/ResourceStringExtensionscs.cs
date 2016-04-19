using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Localization
{
    public static class ResourceStringExtensionscs
    {
        public static string Localize(this string key)
        {
            return key == null ? null : ResourceManager.GetString(key);
        }
    }
}
