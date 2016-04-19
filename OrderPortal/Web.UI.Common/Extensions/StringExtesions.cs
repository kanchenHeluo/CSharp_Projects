using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Web.Common.Extensions
{
    public static class StringExtensions
    {
        [DebuggerStepThrough]
        public static string Format(this string src, params object[] args)
        {
            if (src == null)
            {
                return null;
            }
            return String.Format(src, args);
        }
    }
}
