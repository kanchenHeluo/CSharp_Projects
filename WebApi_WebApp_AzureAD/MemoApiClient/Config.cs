using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoApiClient
{
    internal static class Config
    {
        internal static readonly string AADInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        internal static readonly string ClientId = ConfigurationManager.AppSettings["ida:ClientId"];
        internal static readonly string AppKey = ConfigurationManager.AppSettings["ida:AppKey"];
        internal static readonly string ServiceResourceId = ConfigurationManager.AppSettings["app:ServiceResourceId"];

        internal static readonly string ServiceBaseAddress = ConfigurationManager.AppSettings["app:ServiceBaseAddress"];
        internal static readonly Uri ServiceBasedUri = new Uri(ServiceBaseAddress);
        internal static readonly Uri MemoApiUri = new Uri(ServiceBasedUri, "/api/Memo");
    }
}
