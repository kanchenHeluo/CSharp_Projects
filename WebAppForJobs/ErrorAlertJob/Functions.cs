using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;

namespace ErrorAlertJob
{
    public class Functions
    {

        // This function will be triggered based on the schedule you have set for this WebJob
        [NoAutomaticTrigger]
        public static void CheckFailedHistory(TextWriter log, [Queue("erroralertjoblastrun")] out string message)
        {
            string lastrun = GetLastRunTime();
            log.WriteLine("Start run at {0}.", DateTime.UtcNow);
            log.WriteLine("Get last run time from queue, value= {0} ", lastrun);

            DataTable result = new DataTable();
            try
            {
                result = GetFailedHistory(log, lastrun);
            }
            catch (SqlException)
            {
                result = GetFailedHistory(log, lastrun);
            }
            log.WriteLine("Complete read data from DB, {0} records query out, at {1}.", result.Rows.Count, DateTime.UtcNow);
            
            if (result.Rows.Count > 0)
            {
                // filter out errors which not come from httplistener
                // it can update after drop httplistener approach
                List<DataRow> sdt = result.AsEnumerable().Where(r => !(r["WorkflowName"].ToString().Trim().ToLower().StartsWith("logicapp4crmconnector"))).ToList();
                if (sdt.Count > 0)
                {
                    log.WriteLine("Query from DB, {0} failed history be found", sdt.Count.ToString());
                    BuildEmailContent(sdt, lastrun);
                    log.WriteLine("Email send out");
                }
                else
                    log.WriteLine("Only httpListener failed history in this timespan.");
            }
            else
                log.WriteLine("No failed history in this timespan.");

            message = DateTime.UtcNow.ToString();
            log.WriteLine("Complete run at {0}.", DateTime.UtcNow);
            log.WriteLine("Datetime {0} be written to queue as last run time.", message);
        }

        private static string GetLastRunTime()
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue lastrunQueue = queueClient.GetQueueReference("erroralertjoblastrun");
            lastrunQueue.CreateIfNotExists();
            var message = lastrunQueue.GetMessage();
            string value = (message == null) ? string.Empty : message.AsString;
            if (message != null)
                lastrunQueue.DeleteMessage(message);

            return value;
        }

        private static DataTable GetFailedHistory(TextWriter log, string lastrun)
        {
            string connstr = ConfigurationManager.ConnectionStrings["AzureDB"].ConnectionString;
            DataTable result = new DataTable();
            using (var conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    command.CommandTimeout = 200;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "GenerateAlertInfo";
                    command.Parameters.AddWithValue("@alertDuration", int.Parse(ConfigurationManager.AppSettings["Duration"]));
                    if (!string.IsNullOrEmpty(lastrun))
                    {
                        command.Parameters.AddWithValue("@alertSTime", lastrun);
                        log.WriteLine("Parameter @alertSTime: {0}", lastrun);
                    }

                    log.WriteLine("Start execute sp, at {0}.", DateTime.UtcNow);
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(result);
                    }
                }
            }
            return result;
        }

        private static void SendEmail(string appName, string emailBody, string lastrun)
        {
            string emailFromAddress = ConfigurationManager.AppSettings["Mailbox"];
            
            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;
            mail.From = new MailAddress(emailFromAddress);
            string[] tolist = ConfigurationManager.AppSettings["SendTo"].Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string to in tolist)
            {
                mail.To.Add(to);
            }
            mail.Subject = string.Format("[Azure Connecter Alert] LogicApp of {0} on {1} has issue", appName, ConfigurationManager.AppSettings["Environment"]);
            if (!string.IsNullOrEmpty(lastrun))
                mail.Subject += " at (UTC) " + lastrun;
            mail.Body = emailBody;

            SmtpClient client = new SmtpClient();
            //client.Port = 25;
            client.Port = 587;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(emailFromAddress, ConfigurationManager.AppSettings["MailBoxPWD"]);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = ConfigurationManager.AppSettings["SMTPHost"];
            client.Send(mail);
        }

        private static void BuildEmailContent(List<DataRow> dt, string lastrun)
        {
            string subscription = string.Empty;
            string appName = string.Empty;
            string entityName = string.Empty;
            string workflowName = string.Empty;
            StringBuilder sb = new StringBuilder(); ;
            Dictionary<string, StringBuilder> content = new Dictionary<string, StringBuilder>();

            foreach (DataRow dr in dt.OrderBy(r => r["AppName"]).ThenBy(r => r["Entity"]).ThenBy(r => r["WorkflowName"]))
            {
                if (string.IsNullOrEmpty(subscription))
                {
                    subscription = dr["SubscriptionId"].ToString();
                }

                if (appName != dr["AppName"].ToString().Trim())
                {
                    sb = new StringBuilder();
                    sb.Append("Subscription: " + subscription);
                    sb.Append("</br></br>");

                    appName = dr["AppName"].ToString().Trim();
                    entityName = string.Empty;
                    workflowName = string.Empty;
                    sb.Append("Application: " + appName + "&nbsp;&nbsp;&nbsp; ResourceGroup: ResourceGroup4" + appName + "</br>");

                    content.Add(appName, sb);
                }
                if (entityName != dr["Entity"].ToString().Trim())
                {
                    entityName = dr["Entity"].ToString().Trim();
                    sb.Append("Entity: " + entityName + "</br>");
                }
                if (workflowName != dr["WorkflowName"].ToString().Trim())
                {
                    workflowName = dr["WorkflowName"].ToString().Trim();
                    sb.Append("LogicApp: " + workflowName + string.Format("&nbsp;&nbsp;&nbsp; URL: https://ms.portal.azure.com/#resource/subscriptions/{0}/resourceGroups/ResourceGroup4{1}/providers/Microsoft.Logic/workflows/{2}/ </br></br>", subscription, appName, workflowName));

                    List<DataRow> drc = dt.Where(i => i["AppName"].ToString().Trim().Equals(appName)).Where(i => i["Entity"].ToString().Trim().Equals(entityName)).Where(i => i["WorkflowName"].ToString().Trim().Equals(workflowName))
                        .Where(i => (i["Status"].ToString().Trim() == "Failed" && !string.IsNullOrEmpty(i["RecordId"].ToString().Trim()))).ToList();
                    if (drc.Count > 0)
                    {
                        sb.Append("<span style='color:red'>Hanging</span> records:</br>");
                        sb.Append("<table border='1'><tr><th></th><th>RunStatus</th><th>CorrelationId</th><th>RecordId</th><th>StartTime (UTC)</th><th>Duration (sec)</th></tr>");
                        int j = 1;
                        foreach (DataRow ddr in drc.OrderByDescending(r => r["StartTime"]))
                        {
                            sb.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>", j++, ddr["Status"].ToString().Trim(), ddr["CorrelationId"], ddr["RecordId"], ddr["StartTime"], TimeSpan.FromMilliseconds(double.Parse(ddr["Duration"].ToString())).TotalSeconds.ToString("0.00")));
                        }
                        sb.Append("</table>");
                    }

                    drc = dt.Where(i => i["AppName"].ToString().Trim().Equals(appName)).Where(i => i["Entity"].ToString().Trim().Equals(entityName)).Where(i => i["WorkflowName"].ToString().Trim().Equals(workflowName))
                        .Where(i => (i["Status"].ToString().Trim() == "Failed" && string.IsNullOrEmpty(i["RecordId"].ToString().Trim()))).ToList();
                    if (drc.Count > 0)
                    {
                        sb.Append("<span style='color:brown'>Failed</span> runs:</br>");
                        sb.Append("<table border='1'><tr><th></th><th>RunStatus</th><th>CorrelationId</th><th>RecordId</th><th>StartTime (UTC)</th><th>Duration (sec)</th></tr>");
                        int j = 1;
                        foreach (DataRow ddr in drc.OrderByDescending(r => r["StartTime"]))
                        {
                            sb.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>", j++, ddr["Status"].ToString().Trim(), ddr["CorrelationId"], ddr["RecordId"], ddr["StartTime"], TimeSpan.FromMilliseconds(double.Parse(ddr["Duration"].ToString())).TotalSeconds.ToString("0.00")));
                        }
                        sb.Append("</table>");
                    }

                    drc = dt.Where(i => i["AppName"].ToString().Trim().Equals(appName)).Where(i => i["Entity"].ToString().Trim().Equals(entityName)).Where(i => i["WorkflowName"].ToString().Trim().Equals(workflowName))
                        .Where(i => i["Status"].ToString().Trim() != "Failed").ToList();
                    if (drc.Count > 0)
                    {
                        sb.Append("<span style='color:brown'>Long time</span> runs:</br>");
                        sb.Append("<table border='1'><tr><th></th><th>RunStatus</th><th>CorrelationId</th><th>RecordId</th><th>StartTime (UTC)</th><th>Duration (sec)</th></tr>");
                        int j = 1;
                        foreach (DataRow ddr in drc.OrderByDescending(r => r["StartTime"]))
                        {
                            sb.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td><span style='color:brown'>{5}</span></td></tr>", j++, ddr["Status"].ToString().Trim(), ddr["CorrelationId"], ddr["RecordId"], ddr["StartTime"], TimeSpan.FromMilliseconds(double.Parse(ddr["Duration"].ToString())).TotalSeconds.ToString("0.00")));
                        }
                        sb.Append("</table>");
                    }
                }
            }

            foreach(string key in content.Keys)
            {
                SendEmail(key, content[key].ToString(), lastrun);
            }
        }

    }
}
