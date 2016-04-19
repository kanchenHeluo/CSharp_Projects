using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using Microsoft.Azure.WebJobs;
using System;

namespace PurgeDataJob
{
    public class Functions
    {
        // This function will be triggered based on the schedule you have set for this WebJob
        // This function will enqueue a message on an Azure Queue called queue
        [NoAutomaticTrigger]
        public static void ManualTrigger(TextWriter log, int value, [Queue("queue")] out string message)
        {
            log.WriteLine("Function is invoked with value={0}", value);
            message = value.ToString();
            log.WriteLine("Following message will be written on the Queue={0}", message);
        }

        [NoAutomaticTrigger]
        public static void PurgeHistoryData(TextWriter log, int days)
        {
            try
            {
                PurgeLogic(log, days);
            }
            catch (SqlException)
            {
                PurgeLogic(log, days);
            }
        }

        private static void PurgeLogic(TextWriter log, int days)
        {
            DateTime day = DateTime.UtcNow.AddDays(-days);
            Console.WriteLine("begin copy:"+DateTime.Now.ToString());
            CopyLogic(log, "RunHistory", days);
            Console.WriteLine("end copy:" + DateTime.Now.ToString());
            log.WriteLine("Data in {0} days ago be moved to backup table", days.ToString());
        }

        private static void CopyLogic(TextWriter log, string tablename, int days)
        {
            string connstr = ConfigurationManager.ConnectionStrings["AzureDB"].ConnectionString;
            using (var conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "[dbo].[spPurgeHistoryData]";
                cmd.Parameters.Add("@days", SqlDbType.Int);
                cmd.Parameters["@days"].Value = days;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 900;   
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();                
            }
        }

        /*
        private static void CopyLogic(TextWriter log, string tablename, DateTime day)
        {
            Console.WriteLine("day:"+day.ToString("d"));
            string connstr = ConfigurationManager.ConnectionStrings["AzureDB"].ConnectionString;
            using (var conn = new SqlConnection(connstr))
            {
                conn.Open();
                Console.WriteLine("connected.");
                log.WriteLine("Start execute sp, at {0}.", DateTime.UtcNow);
                string sql = string.Empty;
                if (tablename.ToLower() == "runhistory")
                    sql = @"select [SubscriptionId], [AppName], [Entity], [WorkflowName], [WorkflowTypeId], [Status], [StartTime], [EndTime], [Duration], 
		                                    [CorrelationId], [RunName], [TriggerOutput], [RecordId], [URL], [CreatedOn]
	                                        from " + tablename + " where StartTime < '" + day.ToString("d") + "'";
                else
                    sql = @"select [CorrelationId], [TrackingId], [StartTime], [EndTime], [Input], [Output], [Status], [SourceName], [Outter], [CreatedOn]
	                                        from " + tablename + " where StartTime < '" + day.ToString("d") + "'";
                Console.Write("sql query:"+sql);
                SqlCommand command = new SqlCommand(sql, conn);
                command.CommandTimeout = 900;                    
                DataTable dt = new DataTable();
                dt.Load(command.ExecuteReader());
                                
                Console.WriteLine("dt count:"+dt.Rows.Count);
                Console.WriteLine("start bulk copy:"+DateTime.UtcNow.ToString());

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.UseInternalTransaction, null))
                {
                    try
                    {
                        bulkCopy.DestinationTableName = "Bak" + tablename;
                        bulkCopy.BatchSize = 3000;
                        bulkCopy.WriteToServer(dt);                       
                        bulkCopy.Close();
                    }
                    catch (Exception)
                    {
                        if (bulkCopy != null)
                        {
                            bulkCopy.Close();
                        }
                        if (conn != null)
                        {
                            conn.Close();
                        }
                        throw;
                    }
                }
            }
        }

        private static void DeleteLogic(TextWriter log, int days)
        {
            string connstr = ConfigurationManager.ConnectionStrings["AzureDB"].ConnectionString;
            using (var conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (var command = conn.CreateCommand())
                {
                    log.WriteLine("Start execute sp, at {0}.", DateTime.UtcNow);

                    command.CommandTimeout = 900;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "spPurgeHistoryData";
                    command.Parameters.AddWithValue("@days", days);
                    int result = command.ExecuteNonQuery();
                    log.WriteLine("{0} rows affected.", result);
                }
            }

            log.WriteLine("Data in {0} days ago be removed", days.ToString());
        }
        */
    }
}
