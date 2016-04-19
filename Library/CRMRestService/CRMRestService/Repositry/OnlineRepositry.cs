using CRMRestService.Common;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CRMRestService.Repositry
{
    public class OnlineRepositry : RepositryBase
    {
        public OnlineRepositry()
        {
            crmClient = new CRMClient(AuthenticationHelper.AuthToken(setting), setting.CrmResourceURL);
        }

        protected override void ProcessRequest(JObject entityObj)
        {
            var requestObj = entityObj.DeepClone() as JObject;
            int action = int.Parse(requestObj.Value<string>(Constants.PropertyName_Action));
            requestObj.Remove(Constants.PropertyName_Action);

            string outputData = String.Empty;
            if (action == Constants.CRMCreate)
            {
                if (requestObj.Properties().Where(p => p.Name == Constants.PropertyName_StateCode).Count() > 0)
                {
                    outputData = CreateWithUpdate(requestObj);
                }
                else
                {
                    outputData = crmClient.CreateData(requestObj.ToString(), setting.CrmEntityName).Result;
                    outputData = ReadOutput(outputData, crmPrimaryKey);
                }

                entityObj.Property(crmPrimaryKey).Value = new JValue(outputData);
            }
            else if (action == Constants.CRMUpdate)
            {
                outputData = crmClient.UpdateData(requestObj.ToString(), setting.CrmEntityName, requestObj.Value<string>(crmPrimaryKey)).Result;
                if (!string.IsNullOrEmpty(outputData))
                    throw new Exception(outputData);
            }
        }

        #region private functions
        private string CreateWithUpdate(JObject requestObj)
        {
            string outputData = string.Empty;

            string statecode = requestObj.Property(Constants.PropertyName_StateCode).Value.Value<string>(Constants.PropertyName_OptionSetValue);
            string statuscode = requestObj.Property(Constants.PropertyName_StatusCode).Value.Value<string>(Constants.PropertyName_OptionSetValue);
            requestObj.Remove(Constants.PropertyName_StateCode);
            requestObj.Remove(Constants.PropertyName_StatusCode);

            outputData = crmClient.CreateData(requestObj.ToString(), setting.CrmEntityName).Result;
            outputData = ReadOutput(outputData, crmPrimaryKey);

            if (statecode == Constants.StateCode_Inactive)
            {
                requestObj.Property(crmPrimaryKey).Value = new JValue(outputData);
                requestObj.Property(Constants.PropertyName_StateCode).Value = new JValue(statecode);
                requestObj.Property(Constants.PropertyName_StatusCode).Value = new JValue(statuscode);
                outputData = crmClient.UpdateData(requestObj.ToString(), setting.CrmEntityName, requestObj.Value<string>(crmPrimaryKey)).Result;
                if (!string.IsNullOrEmpty(outputData))
                    throw new Exception(outputData);
            }

            return outputData;
        }
        #endregion

    }
}