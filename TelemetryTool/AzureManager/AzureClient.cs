using Common;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace AzureManager
{
    public class AzureClient
    {
        private History history { get; set; }
        private Dictionary<string, string> entityKeyDict { get; set; }

        private string selectedKey { get; set; }

        public AzureClient()
        {
            entityKeyDict = new System.Collections.Generic.Dictionary<string, string>();
        }
        public AzureClient(SubscriptionInfo sub)
        {
            selectedKey = "contactid"; 

            history = new History();
            history.AppName = sub.AppName;
            history.Entity = sub.EntityName;
            history.SubscriptionId = new Guid(sub.SubscriptionId);
            history.ResourceGroupName = sub.ResourceGroupName;
            history.WorkflowName = sub.WorkflowName;
            history.WorkflowTypeId = sub.WorkflowTypeId;

            entityKeyDict = new System.Collections.Generic.Dictionary<string, string>();
            if (!entityKeyDict.ContainsKey(sub.AppName.ToLower()))
            {
                entityKeyDict.Add(sub.AppName.ToLower(), sub.dbPrimaryKey.ToLower());
            }
        }

        # region authentication
        public static string GetAToken()
        {
            ServiceSetting setting = new ServiceSetting();
            var authenticationContext = new AuthenticationContext("https://login.windows.net/" + setting.Tenant);
            var ucredential = new UserCredential(setting.User, setting.Password);
            var result = authenticationContext.AcquireToken("https://management.core.windows.net/", setting.ClientId, ucredential);

            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the JWT token");
            }

            string token = result.AccessToken;

            return token;
        }

        # endregion

        private string httpHandle(string url)
        {
            string requestUri = string.Format(CultureInfo.InvariantCulture, url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);


            string content = String.Empty;
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        StreamReader sr = new StreamReader(responseStream, Encoding.UTF8);
                        content = sr.ReadToEnd();
                    }
                }
            }
            return content;
        }
        private string httpHandleWithAuth(string url)
        {
            string content = String.Empty;
            while (true)
            {
                string requestUri = string.Format(CultureInfo.InvariantCulture, url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
                request.Headers.Add("x-ms-version", "2012-03-01");
                request.Method = "GET";
                request.ContentType = "application/json";

                try
                {
                    string token = GetAToken();
                    request.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + token);

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            if (responseStream != null)
                            {
                                StreamReader sr = new StreamReader(responseStream);
                                content = sr.ReadToEnd();
                            }
                        }
                    }
                    break;

                }
                catch (WebException ex)
                {
                    var response = (HttpWebResponse)ex.Response;
                    if ((int)response.StatusCode == 429 || ex.Message.Contains("429") || ex.Status == WebExceptionStatus.Timeout)
                    {
                        Console.WriteLine("httpHandleWithAuth Exception with status code: " + response.StatusCode + "; Will Retry; Exception message:" + ex.Message);
                        Thread.Sleep(10000);
                    }
                    else
                    {
                        Console.WriteLine("httpHandleWithAuth with url:" + url + "; Exception Message:" + ex.Message);
                        throw ex;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("httpHandleWithAuth with url:" + url + "; Exception Message:" + ex.Message);
                    throw ex;
                }
            }
            return content;
        }
        private void apiInfoHandle(JObject obj, ref ApiDetail apiDetail)
        {
            if (obj.Properties().Any(p => p.Name == "inputsLink"))
            {
                string inputUrl = ((JObject)obj["inputsLink"])["uri"].ToString().ToString().Replace("'", "@");
                apiDetail.Input = httpHandle(inputUrl).Replace("'", "@");
            }
            if (obj.Properties().Any(p => p.Name == "outputsLink"))
            {
                string outputUrl = ((JObject)obj["outputsLink"])["uri"].ToString().ToString().Replace("'", "@");
                apiDetail.Output = httpHandle(outputUrl).Replace("'", "@");
            }
            else
            {
                apiDetail.Input = "no input";
                apiDetail.Output = "no output";
                //apiDetail.Output = (obj["error"]).ToString().Replace("'", "@");
            }
            

            
            if (obj.Properties().Any(p => p.Name == "trackingId"))
            {
                apiDetail.TrackingId = new Guid(obj["trackingId"].ToString());
            }
            else
            {
                apiDetail.TrackingId = Guid.Empty;
            }

            if (obj.Properties().Any(p => p.Name == "status"))
            {
                apiDetail.Status = obj["status"].ToString();
            }
            else
            {
                apiDetail.Status = "Succeeded";
            }
            if (obj.Properties().Any(p => p.Name == "startTime"))
            {
                apiDetail.StartTime = Convert.ToDateTime(obj["startTime"].ToString());
            }
            if (obj.Properties().Any(p => p.Name == "endTime"))
            {
                apiDetail.EndTime = Convert.ToDateTime(obj["endTime"].ToString());
            }
        }        
        private string runInfoHandle(JObject run, ref Record runRecord)
        {
            runRecord.RunName = run["name"].ToString();
            runRecord.CorrelationId = new Guid(run["properties"]["correlationId"].ToString());
            runRecord.StartTime = Convert.ToDateTime(run["properties"]["startTime"].ToString());
            runRecord.EndTime = Convert.ToDateTime(run["properties"]["endTime"].ToString());
            runRecord.Duration = ((TimeSpan)runRecord.EndTime.Subtract(runRecord.StartTime)).TotalMilliseconds;
            runRecord.Status = run["properties"]["status"].ToString();
            return runRecord.RunName;
        }
        private void handleTriggerHistory(JObject run, ref Record runRecord)
        {
            try
            {
                JObject triggerObj = (JObject)run["properties"]["trigger"];
                ApiDetail triggerOut = new ApiDetail();
                triggerOut.CorrelationId = runRecord.CorrelationId;

                if (triggerObj.Property("name") != null)
                {
                    triggerOut.SourceName = triggerObj["name"].ToString();
                }
                //trigger info  
                
                apiInfoHandle(triggerObj, ref triggerOut);
                runRecord.TriggerHistory = triggerOut;
                runRecord.TriggerOutput = triggerOut.Output;
                
                if (!String.IsNullOrEmpty(triggerOut.Output) && !String.Equals(triggerOut.Output, "no output"))
                {
                    string trOutput = triggerOut.Output.ToLower();
                    JObject trObj = JObject.Parse(trOutput);
                    
                    if (trObj != null && trObj.Property("body") != null)
                    {
                        JObject outStr = null;
                            
                        if (((JObject)trObj["body"]).Property("rows") != null)
                        {
                            JArray rowObj = ((JArray)((JObject)trObj["body"])["rows"]);
                            if (rowObj.Count > 0)
                            {
                                outStr = (JObject)((JArray)((JObject)trObj["body"])["rows"])[0];
                            }
                        }
                        else if (((JObject)trObj["body"]).Property("content") != null)
                        {
                            Object obj = ((JObject)trObj["body"])["content"];
                            if (!string.IsNullOrEmpty(obj.ToString()))
                            {
                                outStr = JObject.Parse(obj.ToString());
                            }
                        }
                        runRecord.RecordId = (outStr != null && outStr[selectedKey] != null) ? outStr[selectedKey].ToString() : String.Empty;                            
                                
                    }
                                         
                }                          
            }
            catch (Exception ex)
            {
                Console.WriteLine("Analyze Trigger History Exception with run info:" + run.ToString() + "; Exception message:" + ex.Message);
                throw ex;
            }

        }
        private void handleActionsHistory(ref Record runRecord)
        {            
            string url = "https://management.azure.com/subscriptions/" + history.SubscriptionId + "/resourceGroups/" + history.ResourceGroupName + "/providers/Microsoft.Logic/workflows/" + history.WorkflowName + "/runs/" + runRecord.RunName + "/actions?api-version=2015-02-01-preview";
            try
            {
                string runResponse = httpHandleWithAuth(url);

                JObject actionsObj = (JObject)JsonConvert.DeserializeObject(runResponse);
                JArray actions = (JArray)actionsObj["value"];
                for (int j = 0; j < actions.Count; j++)
                {
                    JObject actionObj = (JObject)((JObject)actions[j])["properties"];
                    ApiDetail actionOut = new ApiDetail();

                    actionOut.SourceName = "";
                    if (((JObject)actions[j]).Property("name") != null)
                    {
                        actionOut.SourceName = ((JObject)actions[j])["name"].ToString();
                    }
                    actionOut.CorrelationId = runRecord.CorrelationId;
                    apiInfoHandle(actionObj, ref actionOut);
                    runRecord.ActionHistory.Add(actionOut);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fetch Action History Exception with url:" + url + "; Exception message:" + ex.Message);
                throw ex;

            }
        }

            
        
        private List<Record> getLogicAppRuns(string lastRunName)
        {
            List<Record> runlist = new List<Record>();
            string url = "https://management.azure.com/subscriptions/" + history.SubscriptionId + "/resourceGroups/" + history.ResourceGroupName + "/providers/Microsoft.Logic/workflows/" + history.WorkflowName + "/runs?api-version=2015-02-01-preview";
            //string url = "https://management.azure.com/subscriptions/0e2d2609-d3d7-4fb0-8561-398d4f308021/resourceGroups/ResourceGroup4OnePayRoll/providers/Microsoft.Logic/workflows/LogicApp4Contact/runs?api-version=2015-02-01-preview";
            try
            {
                int maxRound = Convert.ToInt32(ConfigurationManager.AppSettings["MaxRounds"].ToString());
                bool sampleFlag = true;
                if (entityKeyDict.ContainsKey(history.Entity.ToLower()))
                {
                    selectedKey = entityKeyDict[history.Entity.ToLower()];
                }
                string response = httpHandleWithAuth(url);

                int round = 1;
                while (!String.IsNullOrEmpty(response) && response.Contains("value"))
                {
                    round++;
                    JObject runsObj = (JObject)JsonConvert.DeserializeObject(response);
                    JArray runs = (JArray)runsObj["value"];
                    for (int i = 0; i < runs.Count; i++)
                    {
                        Record runRecord = new Record();
                        runRecord.URL = url;

                        JObject run = (JObject)runs[i];
                        string status = run["properties"]["status"].ToString();
                        if (status.Equals("Running"))
                        {
                            continue;
                        }
                        string runName = runInfoHandle(run, ref runRecord);
                        if (runName.Equals(lastRunName))
                        {
                            response = String.Empty;
                            break;
                        }
                        string pros = run["properties"].ToString();
                        if (pros.Contains("trigger"))
                        {
                            handleTriggerHistory(run, ref runRecord);
                        }

                        if (status.Equals("Failed"))
                        {                            
                            handleActionsHistory(ref runRecord);
                        }
                        else if (sampleFlag)
                        {
                            handleActionsHistory(ref runRecord);
                            sampleFlag = false;
                        }

                        runlist.Add(runRecord);
                    }
                    if (round < maxRound && response.Contains("nextLink") && runs.Count == Constants.BatchCnt)
                    {
                        url = runsObj["nextLink"].ToString();
                        response = httpHandleWithAuth(url);
                    }
                    else
                    {
                        response = String.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Get Logic Apps Exception with url " + url + "; Exception Message:" + ex.Message);
                throw ex;
            }
            
            return runlist;
        }
        private List<Record> getFailedRuns(string lastRunName)
        {
            List<Record> runlist = new List<Record>();
            string url = "https://management.azure.com/subscriptions/" + history.SubscriptionId + "/resourceGroups/" + history.ResourceGroupName + "/providers/Microsoft.Logic/workflows/" + history.WorkflowName + "/runs?api-version=2015-02-01-preview&$filter=Status eq 'Failed'";
            try
            {
                int errorMaxRound = Convert.ToInt32(ConfigurationManager.AppSettings["MaxErrorRounds"].ToString());
                
                string response = httpHandleWithAuth(url);
                List<ApiDetail> failedItems = new List<ApiDetail>();

                int round = 1;
                List<Record> runsList = new List<Record>();
                while (!String.IsNullOrEmpty(response) && response.Contains("value"))
                {
                    round++;
                    JObject runsObj = (JObject)JsonConvert.DeserializeObject(response);
                    JArray runs = (JArray)runsObj["value"];
                    for (int i = 0; i < runs.Count; i++)
                    {
                        Record runRecord = new Record();
                        runRecord.URL = url;
                        // run info                    
                        JObject run = (JObject)runs[i];
                        string status = run["properties"]["status"].ToString();
                        string runName = runInfoHandle(run, ref runRecord);
                        if (runName.Equals(lastRunName))
                        {
                            break;
                        }
                        string pros = run["properties"].ToString();
                        if (pros.Contains("trigger"))
                        {
                            JObject obj = (JObject)run["properties"]["trigger"];
                            string outputUrl = String.Empty;
                            if (obj["outputsLink"] != null)
                                outputUrl = ((JObject)obj["outputsLink"])["uri"].ToString().ToString().Replace("'", "@");
                            runRecord.TriggerOutput = outputUrl;
                            
                        }
                        if (runRecord.StartTime < DateTime.UtcNow.AddHours(-1))
                        {
                            round = errorMaxRound;
                            break;
                        }
                        runlist.Add(runRecord);
                    }
                    if (round < errorMaxRound && response.Contains("nextLink") && runs.Count == Constants.BatchCnt)
                    {
                        url = runsObj["nextLink"].ToString();
                        response = httpHandleWithAuth(url);
                    }
                    else
                    {
                        response = String.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("failed items exception with url:" + url + ";Exception message:" + ex.Message);
                throw ex;
            }
            return runlist;
        }
        public History FetchLAHistory(string lastrunname)
        {
            if (history.WorkflowTypeId == Constants.Outter)
            {
                history.Runs = getLogicAppRuns(lastrunname);
            }
            else
            {
                history.Runs = getFailedRuns(lastrunname);
            }
            return history;
        }
    }
}
