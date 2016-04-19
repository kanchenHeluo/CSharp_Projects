using CRMRestService.Common;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace CRMRestService.Repositry
{
    public class OnpremRepositry : RepositryBase
    {
        public OnpremRepositry()
        {
            crmClient = new CRMClient(AuthenticationHelper.GetHandler(), setting.CrmResourceURL);
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
                    outputData = CreateWithSetState(requestObj);
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
                if (requestObj.Properties().Where(p => p.Name == Constants.PropertyName_StateCode).Count() > 0)
                {
                    UpdateWithSetState(requestObj, requestObj.Value<string>(crmPrimaryKey));
                }
                else
                {
                    outputData = crmClient.UpdateData(requestObj.ToString(), setting.CrmEntityName, requestObj.Value<string>(crmPrimaryKey)).Result;
                    if (!string.IsNullOrEmpty(outputData))
                        throw new Exception(outputData);
                }
            }
        }

        #region private functions
        private string CreateWithSetState(JObject requestObj)
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
                crmClient.SetState(setting.CrmEntityName, statecode, statuscode, outputData).Wait();
            }

            return outputData;
        }

        private void UpdateWithSetState(JObject requestObj, string guid)
        {
            string statecode = requestObj.Property(Constants.PropertyName_StateCode).Value.Value<string>(Constants.PropertyName_OptionSetValue);
            string statuscode = requestObj.Property(Constants.PropertyName_StatusCode).Value.Value<string>(Constants.PropertyName_OptionSetValue);
            requestObj.Remove(Constants.PropertyName_StateCode);
            requestObj.Remove(Constants.PropertyName_StatusCode);

            string outputData = crmClient.UpdateData(requestObj.ToString(), setting.CrmEntityName, guid).Result;
            if (!string.IsNullOrEmpty(outputData))
                throw new Exception(outputData);

            crmClient.SetState(setting.CrmEntityName, statecode, statuscode, guid).Wait();
        }
        #endregion

    }
}