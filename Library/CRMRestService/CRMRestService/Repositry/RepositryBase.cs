using System;
using System.Linq;
using System.Xml.Linq;
using CRMRestService.Interface;
using CRMRestService.Models;
using CRMRestService.Common;
using Newtonsoft.Json.Linq;

namespace CRMRestService.Repositry
{
    public abstract class RepositryBase : IRepositry
    {
        #region properties
        protected static ServiceSetting setting;
        protected CRMClient crmClient;

        protected string crmPrimaryKey
        {
            get { return setting.CrmEntityName + "Id"; }
        }

        public Guid AssociateAccountId
        {
            get { return setting.CacheAccountId; }
            set { setting.CacheAccountId = value; }
        }

        private int retry = 0;
        #endregion

        /// <summary>
        /// default setting will be used, and initialize from app.config
        /// </summary>
        public RepositryBase()
        {
            //
        }

        public static IRepositry CreateRepositry()
        {
            setting = new ServiceSetting();
            return CreateRepositry(setting);
        }
        public static IRepositry CreateRepositry(ServiceSetting serviceSetting)
        {
            setting = serviceSetting;

            if (setting.CRMVersion.Equals("onprem"))
            {
                return new OnpremRepositry();
            }
            else if (setting.CRMVersion.Equals("online"))
            {
                return new OnlineRepositry();
            }
            else
            {
                throw new Exception("no such repositry for " + setting.CRMVersion);
            }
        }

        public void Execute(JObject entityObj)
        {
            try
            {
                // associate "microsoft" company/account
                entityObj = AssociateAccount(entityObj);

                //specific logic to verify primaryKey and action
                entityObj = VerifyPrimaryKeyAndAction(entityObj);

                ProcessRequest(entityObj);
            }
            catch (Exception ex)
            {
                if (retry < setting.Retry)
                {
                    retry++;
                    Execute(entityObj);
                }
                else
                    throw ex;
            }
        }

        protected abstract void ProcessRequest(JObject requestObj);

        #region private/protected functions
        private JObject AssociateAccount(JObject entityObj, string name = "microsoft")
        {
            if (setting.CrmEntityName == "Contact" && setting.MicrosoftAccountEnabled)
            {
                if (AssociateAccountId == Guid.Empty)
                {
                    //check if microsoft account already there
                    string result = crmClient.ReadData("Account", "Name", name, "AccountId").Result;
                    result = ReadOutput(result, "AccountId");
                    AssociateAccountId = ValidGuid(result);
                    if (AssociateAccountId == Guid.Empty)
                    {
                        AssociateAccountId = Guid.NewGuid();
                        string jsonData = "{\"AccountId\":\"" + AssociateAccountId.ToString() + "\",\"Name\":\"" + name + "\"}";
                        result = crmClient.CreateData(jsonData, "Account").Result;
                        result = ReadOutput(result, "AccountId");
                        AssociateAccountId = Guid.Parse(result);
                    }
                }

                if (entityObj.Properties().Where(p => p.Name == Constants.PropertyName_Company).Count() > 0)
                    entityObj.Remove(Constants.PropertyName_Company);
                entityObj.Add(Constants.PropertyName_Company, JToken.FromObject(new AccountReference() { Id = AssociateAccountId.ToString(), LogicalName = "account" }));
            }
            return entityObj;
        }

        private JObject VerifyPrimaryKeyAndAction(JObject entityObj)
        {
            /*
             * logic is like this:
             * 1. if no secondkey, 
             *  - if update, directly use input data
             *  - if create, in order to prevant duplicate, use primarykey to get Guid, then based on response to switch create/update
             * 2. if has secondkey,
             *  - if primarykey no value, use secondkey to get Guid, then based on response to switch create/update
             *  - if primarykey has value, switch to update
             *  
             * so map/configure like this:
             * 1. for data from Azure db, 
             *  - if application use secondkey, primarykey map to real Guid column (empty value will get Guid first)
             *  - if application no secondkey, primarykey map to old Guid column (create action will get Guid first)
             * 2. for data from on-prem staging, action is correct
             *  - if application use secondkey, primarykey keep empty value (each record will get Guid first)
             *  - if application no secondkey, primarykey map to old Guid column (create action will get Guid first)
             * 3. for no-db source, no action
             *  - if application use secondkey, primarykey keep empty value (each record will get Guid first)
             *  - if application no secondkey, primarykey map to old Guid column, action keep create (each record will get Guid first)
             */

            Guid recordGuid = Guid.Parse(entityObj.Value<string>(crmPrimaryKey));
            int action = int.Parse(entityObj.Value<string>(Constants.PropertyName_Action));

            if (string.IsNullOrEmpty(setting.CrmSecondPrimaryKey))
            {
                if (action == Constants.CRMCreate)
                {
                    string outputData = crmClient.ReadData(setting.CrmEntityName, crmPrimaryKey, recordGuid.ToString(), crmPrimaryKey).Result;
                    outputData = ReadOutput(outputData, crmPrimaryKey);
                    recordGuid = ValidGuid(outputData);

                    entityObj.Property(crmPrimaryKey).Value = (recordGuid == Guid.Empty) ? new JValue(entityObj.Value<string>(crmPrimaryKey)) : new JValue(recordGuid.ToString());
                    entityObj.Property(Constants.PropertyName_Action).Value = (recordGuid == Guid.Empty) ? new JValue(Constants.CRMCreate.ToString()) : new JValue(Constants.CRMUpdate.ToString());
                }
            }
            else
            {
                if (recordGuid == Guid.Empty)
                {
                    string outputData = crmClient.ReadData(setting.CrmEntityName, setting.CrmSecondPrimaryKey, entityObj.GetValue(setting.CrmSecondPrimaryKey), crmPrimaryKey).Result;
                    outputData = ReadOutput(outputData, crmPrimaryKey);
                    recordGuid = ValidGuid(outputData);

                    entityObj.Property(crmPrimaryKey).Value = new JValue(recordGuid.ToString());
                    entityObj.Property(Constants.PropertyName_Action).Value = (recordGuid == Guid.Empty) ? new JValue(Constants.CRMCreate.ToString()) : new JValue(Constants.CRMUpdate.ToString());
                }
                else
                {
                    entityObj.Property(Constants.PropertyName_Action).Value = new JValue(Constants.CRMUpdate.ToString());
                }
            }

            return entityObj;
        }

        protected string ReadOutput(string outputData, string keyName)
        {
            CatchError(outputData);

            string result = string.Empty;
            //feed namespace
            XNamespace rss = "http://www.w3.org/2005/Atom";
            // d namespace
            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            // m namespace
            XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";
            try
            {
                XDocument xdoc = XDocument.Parse(outputData, LoadOptions.None);
                XElement element = xdoc.Root.Descendants(m + "properties").FirstOrDefault();
                if (element != null)
                {
                    foreach (var entry in element.Elements())
                    {
                        if (entry.Name == d + keyName)
                        {
                            result = entry.Value;
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception(outputData);
            }

            return result;
        }

        private void CatchError(string outputData)
        {
            if (outputData.Contains("<error"))
            {
                int start = outputData.IndexOf("<error");
                int end = outputData.IndexOf("</error>") + "</error>".Length;
                string message = outputData.Substring(start, (end - start));
                throw new Exception(message);
            }
        }

        private Guid ValidGuid(string value)
        {
            Guid id = Guid.Empty;
            if (!string.IsNullOrEmpty(value))
                id = Guid.Parse(value);
            return id;
        }
        #endregion

    }
}