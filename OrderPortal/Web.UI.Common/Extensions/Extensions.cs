using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Web.Common.Extensions
{
   public static class Extensions
    {


       public static string SearchTokenizer(this string str)
       {

           return str.Replace(" ", "+");
       }

       public static  string[]  Trim(this string[] strs)
       {
           Array.ForEach<string>(strs, x => strs[Array.IndexOf<string>(strs, x)] = x.Trim());
           return strs;
       }
        public static T GetValue<T>(this System.Collections.Specialized.NameValueCollection collection, string key)
        {
            var v = collection[key];
            if (!string.IsNullOrWhiteSpace(v))
            {
                return (T)Convert.ChangeType(v, typeof(T), CultureInfo.InvariantCulture);

            }
            else
            {
                return default(T);
            }
        }

        public static T GetValue<T>(this NameValueCollection nameValuePairs, string configKey, T defaultValue) where T : IConvertible
        {
            T retValue = default(T);

            if (nameValuePairs.AllKeys.Contains(configKey))
            {
                string tmpValue = nameValuePairs[configKey];

                retValue = (T)Convert.ChangeType(tmpValue, typeof(T));
            }
            else
            {
                return defaultValue;
            }

            return retValue;
        }

        public static TOut TryGetNumber<T, TOut>(this T obj, Func<T, TOut> fun, TOut defaultValue = default(TOut)) where T : class
        {
            return obj != null ? fun(obj) : defaultValue;
        }

       public static string UriEncode(this string Url)
        {
            if (String.IsNullOrEmpty(Url))
                return null;
            else  return HttpUtility.UrlPathEncode(Url);
        }
    }
}
