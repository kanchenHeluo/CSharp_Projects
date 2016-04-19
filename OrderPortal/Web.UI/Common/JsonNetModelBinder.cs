using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Web.UI.Common
{
    public class JsonNetModelBinder : DefaultModelBinder
    {
        /// <summary>
        /// override model binder to acomplish multiple type of json model parsing. the mvc provided model binder cannot parse nested classes
        /// </summary>
        /// <param name="controllerContext"></param>
        /// <param name="bindingContext"></param>
        /// <returns></returns>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return base.BindModel(controllerContext, bindingContext);
            if (controllerContext.HttpContext.Request.ContentType.IndexOf("json", StringComparison.InvariantCultureIgnoreCase) == -1)
            {
                return base.BindModel(controllerContext, bindingContext);
            }
            if (((System.Web.Routing.Route)(controllerContext.RouteData.Route)).Defaults.ContainsKey(bindingContext.ModelName))
            {
                var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                if (value != null)
                {
                    var js = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue };
                    return js.Deserialize(value.AttemptedValue, bindingContext.ModelType);
                }
                return null;
            }
            controllerContext.HttpContext.Request.InputStream.Position = 0;
            var sr = new StreamReader(controllerContext.HttpContext.Request.InputStream);
            var jsString = sr.ReadToEnd();
            if (!jsString.Contains("{"))
            {
                var settings = new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                };
                controllerContext.HttpContext.Request.InputStream.Position = 0;
                using (var reader = new StreamReader(controllerContext.HttpContext.Request.InputStream))
                {
                    var jreader = new JsonTextReader(reader);

                    var ser = JsonSerializer.Create(settings);
                    ser.Converters.Add(new IsoDateTimeConverter());
                    ser.Converters.Add(new PlainJsonStringConverter());
                    return ser.Deserialize(jreader, bindingContext.ModelType);
                }
            }
            
            if (jsString.IndexOf("[") == 0)
            {
                var items = JArray.Parse(jsString);
                var val = items.ToObject(bindingContext.ModelType);
                return val;
            }
            else
            {
                var item = JObject.Parse(jsString);
                if (((IDictionary<string, JToken>)item).ContainsKey(bindingContext.ModelName))
                {
                    var val = item[bindingContext.ModelName].ToObject(bindingContext.ModelType);
                    return val;
                }
                else
                {
                    var val = item.ToObject(bindingContext.ModelType);
                    return val;
                }
            }
            
        }
    }

    public class PlainJsonStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value == null ? null : reader.Value.ToString();
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue((string)value);
        }
    }
}