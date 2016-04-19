using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CRMRestService.Repositry;
using CRMRestService.Models;
using CRMRestService.Common;
using CRMRestService.Interface;
using FallBackTool.Consumer.Common;
using FallBackTool.Converter.Models;
using log4net;

namespace FallBackTool.Consumer
{
    public class CRMConsumer
    {
        private ServiceSetting setting;

        private string crmPrimaryKey
        {
            get { return setting.CrmEntityName + "Id"; }
        }

        public CRMConsumer()
        {
            setting = new ServiceSetting();
        }
        public CRMConsumer(ServiceSetting cinput)
        {
            setting = cinput;
        }

        public void Consume(List<ICRMModel> crmList)
        {
            ProcessData(crmList);

            int retryCnt = Convert.ToInt32(ConfigurationManager.AppSettings["Retry"]);
            int currentCount = 1;
            while (ApplicationCache.retryList.Count > 0 && retryCnt > 0)
            {
                Console.WriteLine("[Retrying " + (currentCount) + " ]: Processing Records To CRM " + DateTime.Now.ToString());

                ProcessData(BuildRetryList(ApplicationCache.retryList));

                Console.WriteLine("[Completed][Retrying " + (currentCount) + " ]: data " + ApplicationCache.retryList.Count + DateTime.Now.ToString());
                retryCnt--;
                currentCount++;
            }

            if (ApplicationCache.retryList.Count > 0)
            {
                ILog failed = LogManager.GetLogger(typeof(CRMConsumer));
                failed.Error("[Exception] Processing record failed with retry. List:");
                foreach (ICRMModel item in ApplicationCache.retryList)
                {
                    failed.Error(item.ToString());
                }
            }

            Console.WriteLine("[Completed] consume data finished.");
            ILog log = LogManager.GetLogger(typeof(CRMConsumer));
            log.Info("[Completed] consume data finished.");
        }

        #region private functions
        private void ProcessData(List<ICRMModel> crmList)
        {
            ApplicationCache.retryList = new List<ICRMModel>();

            Console.WriteLine();
            Console.WriteLine("[ProcessData]: Start Processing Records To CRM " + DateTime.Now.ToString());
            ILog log = LogManager.GetLogger(typeof(CRMConsumer));
            log.Info("[ProcessData]: Start Processing Records To CRM " + DateTime.Now.ToString());
            ApplicationCache.totalRecords = crmList.Count;
            ApplicationCache.processedRecords = 0;
            ApplicationCache.percent = 0;

            foreach (ICRMModel record in crmList)
            {
                StartThread(record);
            }
            MachineThread.WaitUntilAllThreadsComplete();

            Console.WriteLine("[Completed][ProcessData]: data " + crmList.Count + DateTime.Now.ToString());
            log.Info("[Completed][ProcessData]: data " + crmList.Count + DateTime.Now.ToString());
        }
        private void StartThread(ICRMModel inputjson)
        {
            GlobalSemaphore.SemaphoreInstance.WaitOne();
            var t = new Thread(() => SingleThreadExecute(inputjson));
            MachineThread.MachineThreads.Add(t);
            t.Start();
        }
        private void SingleThreadExecute(ICRMModel inputjson)
        {
            var entityObj = JsonConvert.DeserializeObject<JObject>(inputjson.ToString());
            //entityObj.Add(Constants.PropertyName_Action, new JValue(inputjson.Action));

            if (setting.MicrosoftAccountEnabled)
                setting.CacheAccountId = ApplicationCache.accountId;
            setting.Retry = 0;
            try
            {
                var crmRepositry = RepositryBase.CreateRepositry(setting);
                //crmRepositry.AssociateAccountId = WebApiApplication.accountId;
                
                if (!ValidateInput(entityObj))
                    throw new Exception("input object error: " + inputjson.ToString());

                crmRepositry.Execute(entityObj);
                ApplicationCache.accountId = crmRepositry.AssociateAccountId;
            }
            catch (Exception ex)
            {
                ApplicationCache.retryList.Add(inputjson);
                Console.WriteLine("[Exception][3]: Exception:" + ex.Message + "; RecordGuid:" + entityObj.Property(crmPrimaryKey).Value);
                ILog log = LogManager.GetLogger(ex.GetType());
                log.Warn("[Warning] Processing record failed:", ex);
            }
            finally
            {
                ApplicationCache.processedRecords++;
                var temp = int.Parse((ApplicationCache.processedRecords * 10 / ApplicationCache.totalRecords).ToString("d"));
                if (temp > ApplicationCache.percent)
                {
                    ApplicationCache.percent = temp;
                    Console.WriteLine("Processed " + temp.ToString() + "0% of all data.");
                    ILog log = LogManager.GetLogger(typeof(CRMConsumer));
                    log.Info("Processed " + temp.ToString() + "0% of all data.");
                }
                GlobalSemaphore.SemaphoreInstance.Release();
            }
        }
        private List<ICRMModel> BuildRetryList(List<ICRMModel> list)
        {
            List<ICRMModel> retry = new List<ICRMModel>();
            foreach (ICRMModel item in list)
            {
                retry.Add(item);
            }
            return retry;
        }

        private bool ValidateInput(JObject input)
        {
            bool valid = true;

            if (input.Properties().Count() > 0)
            {
                if (input.Properties().Where(p => p.Name == Constants.PropertyName_Action).Count() == 0 || string.IsNullOrEmpty(input.Value<string>(Constants.PropertyName_Action)))
                    valid = false;
                else if (input.Properties().Where(p => p.Name == crmPrimaryKey).Count() == 0)
                    valid = false;
                else if (!string.IsNullOrEmpty(setting.CrmSecondPrimaryKey) && (input.Properties().Where(p => p.Name == setting.CrmSecondPrimaryKey).Count() == 0 || string.IsNullOrEmpty(input.Value<string>(setting.CrmSecondPrimaryKey))))
                    valid = false;

                if (string.IsNullOrEmpty(input.Value<string>(crmPrimaryKey)))
                    input.Property(crmPrimaryKey).Value = Guid.Empty.ToString();
                if (input.Properties().Where(p => p.Value.Type == JTokenType.Object).Count() > 0)
                {
                    List<JProperty> temp = input.Properties().Where(p => p.Value.Type == JTokenType.Object).ToList();
                    foreach (JProperty property in temp)
                    {
                        if (property.Value.Values().Count() > 1 && string.IsNullOrEmpty(property.Value.Value<string>("Id")))
                            input.Property(property.Name).Remove();
                    }
                }
            }
            else
                valid = false;

            return valid;
        }
        #endregion

    }
}
