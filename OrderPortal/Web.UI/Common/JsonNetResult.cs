using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.UI.Common
{
    /// <summary>
    /// Json.NET wrapper for JsonResult
    /// </summary>
    public class JsonNetResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data == null)
            {
                return;
            }

            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

#if DEBUG
            var json = JsonConvert.SerializeObject(Data, settings);
#endif
            var ser = JsonSerializer.Create(settings);
            ser.Converters.Add(new IsoDateTimeConverter() 
                        { 
                            DateTimeFormat = "MM/dd/yyyy" //TODO: put user perference in
                        });
            ser.Serialize(response.Output, Data);
        }
    }
}