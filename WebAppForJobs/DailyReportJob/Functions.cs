using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System;

namespace DailyReportJob
{
    public class Functions
    {
        // This function will be triggered based on the schedule you have set for this WebJob
        // This function will enqueue a message on an Azure Queue called queue
        [NoAutomaticTrigger]
        public static void DailyReport(TextWriter log, string reporton)
        {
            log.WriteLine("Start run at {0}.(UTC)", DateTime.UtcNow);
            DataTable result = new DataTable();
            int deltaContactCnt = 0;
            try
            {
                result = GetDailyData(log, reporton, ref deltaContactCnt);
               
            }
            catch (SqlException)
            {
                result = GetDailyData(log, reporton, ref deltaContactCnt);
                
            }
            log.WriteLine("Complete read data from DB, {0} records query out, at {1}.", result.Rows.Count, DateTime.UtcNow);

            if (result.Rows.Count > 0)
            {
                log.WriteLine("Query LogicApp daily detail from Log Table. Application count: {0}", result.Rows.Count.ToString());
                SendEmail(result, deltaContactCnt, reporton);
                log.WriteLine("Daily report Email send out");
            }
            log.WriteLine("Complete run at {0}.", DateTime.UtcNow);
        }

        private static DataTable GetDailyData(TextWriter log, string reporton, ref int deltaContactCnt)
        {
            DataTable result = new DataTable();

            foreach (ConnectionStringSettings setting in ConfigurationManager.ConnectionStrings)
            {
                if (setting.Name.StartsWith("StagingDB"))
                {
                    string connstr = setting.ConnectionString;
                    string serverName = connstr.Substring(11, connstr.IndexOf(".database.windows.net") - 11);
                    using (var conn = new SqlConnection(connstr))
                    {
                        conn.Open();
                        using (var command = conn.CreateCommand())
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandText = "CRMAzure.spGetDailyReport";
                            command.CommandTimeout = 500;
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@servername", serverName);
                            if (!string.IsNullOrEmpty(reporton))
                                command.Parameters.AddWithValue("@date", DateTime.Parse(reporton).Date);

                            log.WriteLine("Start execute sp, at {0}.", DateTime.UtcNow);
                            using (var adapter = new SqlDataAdapter(command))
                            {
                                adapter.Fill(result);
                            }
                        }

                        // obtain deltaContactCnt in reporton date time                         
                        DateTime date = string.IsNullOrEmpty(reporton) ? DateTime.UtcNow : Convert.ToDateTime(reporton);
                        DateTime dateEnd = date.AddDays(1);

                        string reporton_today = date.ToString("d");
                        string reporton_tommorrow = dateEnd.ToString("d");

                        string query = "select count(*) from utilitycontact where (lastmodified > '" + reporton_today + "' and lastmodified < '" + reporton_tommorrow + "') or (statuslastmodified > '" + reporton_today + "' and statuslastmodified < '"+reporton_tommorrow+"')";
                        Console.WriteLine("query:"+query);
                        using (var command = new SqlCommand(query, conn))
                        {
                            deltaContactCnt = (int)command.ExecuteScalar();
                            //Console.WriteLine("delta count:"+deltaContactCnt.ToString());
                        }                        
                    }
                }
            }

            return result;
        }

        private static void SendEmail(DataTable dt, int deltaContactCnt, string reporton)
        {
            string emailFromAddress = ConfigurationManager.AppSettings["Mailbox"];
            DateTime day = string.IsNullOrEmpty(reporton) ? DateTime.UtcNow.Date : DateTime.Parse(reporton).Date;

            MailMessage mail = new MailMessage();
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;
            mail.From = new MailAddress(emailFromAddress);
            string[] tolist = ConfigurationManager.AppSettings["SendTo"].Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string to in tolist)
            {
                mail.To.Add(to);
            }
            mail.Subject = string.Format("Azure Connector Daily report for applications in {0} on {1} (UTC)", ConfigurationManager.AppSettings["Environment"], day.ToString("d"));
            mail.Body = BuildEmailContent(dt, deltaContactCnt);

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

        private static string BuildEmailContent(DataTable dt, int deltaContactCnt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<span>Email Generated On:" + DateTime.UtcNow.ToString() + " (UTC)</span></br></br>");
            sb.Append("<span>Delta Daily Contact Count:" + deltaContactCnt.ToString() + "</span></br>");
            sb.Append("<table border='1'><tr><th>SQL Server</th><th>Application</th><th>Entity</th><th>Records processed(Successfully)</th><th>Records created</th><th>Records updated</th><th>Start Time (UTC)</th><th>Total Process Time</th><th>Records error</th><th>Retry success</th></tr>");
            foreach (DataRow dr in dt.Rows)
            {
                string timecost = dr["Duration"] == DBNull.Value ? "0 s" : string.Format("{0}h {1}min {2}s", TimeSpan.FromSeconds(double.Parse(dr["Duration"].ToString())).Hours, TimeSpan.FromSeconds(double.Parse(dr["Duration"].ToString())).Minutes, TimeSpan.FromSeconds(double.Parse(dr["Duration"].ToString())).Seconds);
                //string speed = dr["AvergeSpeed"] == DBNull.Value ? "N/A" : double.Parse(dr["AvergeSpeed"].ToString()).ToString("0.00");
                //string retry = int.Parse(dr["FailedRecordCnt"].ToString()) == 0 ? "N/A" : (bool.Parse(dr["RetriedSucceeded"].ToString()) ? "<span style='color:green'>Yes</span>" : "<span style='color:red'>No</span>");
                string retry = int.Parse(dr["FailedRecordCnt"].ToString()) == 0 ? "N/A" : "<span style='color:green'>Yes</span>";

                sb.Append(string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td align='center'>{3}</td><td align='center'>{4}</td><td align='center'>{5}</td><td align='center'>{6}</td><td align='center'>{7}</td><td align='center'>{8}</td><td>{9}</td></tr>",
                    dr["ServerName"], dr["AppName"].ToString().Trim(), dr["Entity"], dr["TotalRecordCnt"], dr["CreateCnt"], dr["UpdateCnt"], dr["StartTime"], timecost, dr["FailedRecordCnt"], retry));
            }
            sb.Append("</table>");

            return sb.ToString();
        }

    }
}
