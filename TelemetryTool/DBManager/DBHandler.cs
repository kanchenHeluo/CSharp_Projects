using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Data;
using System.Data.SqlClient;

namespace DBManager
{
    public class DBHandler
    {
        public DBClient DBClient { get; set; }
        public DBHandler()
        {
            DBClient = new DBClient();   
        }
        public ResourceInfo GetResourceInfoList()
        {
            ResourceInfo resourceInfo = new ResourceInfo();

            DataTable dt = DBClient.ExecSPReturnTable("[dbo].[GetResourceInfo]");

            foreach (DataRow r in dt.Rows)
            {
                SubscriptionInfo sub = new SubscriptionInfo();
                sub.SubscriptionId = r["subscriptionid"].ToString().Trim();
                sub.AppName = r["appname"].ToString().Trim();
                sub.WorkflowName = r["workflowname"].ToString().Trim();
                sub.WorkflowTypeId = Convert.ToInt32(r["workflowtypeid"].ToString().Trim());
                sub.ResourceGroupName = r["resourcegroupname"].ToString().Trim();
                sub.EntityName = r["entity"].ToString().Trim();
                sub.LastRunName = r["runname"].ToString().Trim();
                sub.crmResourceUrl = r["crmResourceUrl"].ToString().Trim();
                sub.crmSecondKey = r["crmSecondKey"].ToString().Trim();
                sub.dbPrimaryKey = r["dbPrimaryKey"].ToString().Trim();
                resourceInfo.AddItem(sub);
            }

            return resourceInfo;
        }
        private SqlBulkCopy PrepareRunBulk(string tableName, ref DataTable dt)
        {
            SqlBulkCopy bulkCopy = new SqlBulkCopy
                (
                    DBClient.conn,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.FireTriggers |
                    SqlBulkCopyOptions.UseInternalTransaction,
                    null
                );
            bulkCopy.DestinationTableName = tableName;
            bulkCopy.ColumnMappings.Add("SubscriptionId", "SubscriptionId");
            bulkCopy.ColumnMappings.Add("AppName", "AppName");
            bulkCopy.ColumnMappings.Add("Entity", "Entity");
            bulkCopy.ColumnMappings.Add("WorkflowName", "WorkflowName");
            bulkCopy.ColumnMappings.Add("WorkflowTypeId", "WorkflowTypeId");
            bulkCopy.ColumnMappings.Add("Status", "Status");
            bulkCopy.ColumnMappings.Add("StartTime", "StartTime");
            bulkCopy.ColumnMappings.Add("EndTime", "EndTime");
            bulkCopy.ColumnMappings.Add("Duration", "Duration");
            bulkCopy.ColumnMappings.Add("CorrelationId", "CorrelationId");
            bulkCopy.ColumnMappings.Add("RunName", "RunName");
            bulkCopy.ColumnMappings.Add("TriggerOutput", "TriggerOutput");
            bulkCopy.ColumnMappings.Add("RecordId", "RecordId");
            bulkCopy.ColumnMappings.Add("URL", "URL");

            dt.Columns.Add("SubscriptionId", typeof(Guid));
            dt.Columns.Add("AppName", typeof(string));
            dt.Columns.Add("Entity", typeof(string));
            dt.Columns.Add("WorkflowName", typeof(string));
            dt.Columns.Add("WorkflowTypeId", typeof(int));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("StartTime", typeof(DateTime));
            dt.Columns.Add("EndTime", typeof(DateTime));
            dt.Columns.Add("Duration", typeof(double));
            dt.Columns.Add("CorrelationId", typeof(Guid));
            dt.Columns.Add("RunName", typeof(string));
            dt.Columns.Add("TriggerOutput", typeof(string));
            dt.Columns.Add("RecordId", typeof(string));
            dt.Columns.Add("URL", typeof(string));
            return bulkCopy;
        }
        private void PrepareRunDT(History history, Record run, ref DataTable dt)
        {
            DataRow r = dt.NewRow();
            r["SubscriptionId"] = (Guid)history.SubscriptionId;
            r["AppName"] = history.AppName;
            r["Entity"] = history.Entity;
            r["WorkflowName"] = history.WorkflowName;
            r["WorkflowTypeId"] = history.WorkflowTypeId;
            r["Status"] = run.Status;
            r["StartTime"] = run.StartTime;
            r["EndTime"] = run.EndTime;
            r["Duration"] = run.Duration;
            r["CorrelationId"] = (Guid)run.CorrelationId;
            r["RunName"] = run.RunName;
            r["TriggerOutput"] = run.TriggerOutput;
            r["RecordId"] = run.RecordId;
            r["URL"] = run.URL;
            dt.Rows.Add(r);
        }
        private SqlBulkCopy PrepareBulk(string tableName, ref DataTable dt)
        {
            SqlBulkCopy bulkCopy = new SqlBulkCopy
                (
                    DBClient.conn,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.FireTriggers |
                    SqlBulkCopyOptions.UseInternalTransaction,

                    null
                );
            bulkCopy.DestinationTableName = tableName;
            bulkCopy.DestinationTableName = tableName;
            bulkCopy.ColumnMappings.Add("CorrelationId", "CorrelationId");
            bulkCopy.ColumnMappings.Add("TrackingId", "TrackingId");
            bulkCopy.ColumnMappings.Add("StartTime", "StartTime");
            bulkCopy.ColumnMappings.Add("EndTime", "EndTime");
            bulkCopy.ColumnMappings.Add("Input", "Input");
            bulkCopy.ColumnMappings.Add("Output", "Output");
            bulkCopy.ColumnMappings.Add("Status", "Status");
            bulkCopy.ColumnMappings.Add("SourceName", "SourceName");

            dt.Columns.Add("CorrelationId", typeof(Guid));
            dt.Columns.Add("TrackingId", typeof(Guid));
            dt.Columns.Add("StartTime", typeof(DateTime));
            dt.Columns.Add("EndTime", typeof(DateTime));
            dt.Columns.Add("Input", typeof(string));
            dt.Columns.Add("Output", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("SourceName", typeof(string));
            return bulkCopy;
        }
        private void PrepareDT(ApiDetail triDetail, ref DataTable dt)
        {
            DataRow r = dt.NewRow();
            r["CorrelationId"] = triDetail.CorrelationId;
            r["TrackingId"] = triDetail.TrackingId;
            r["StartTime"] = triDetail.StartTime;
            r["EndTime"] = triDetail.EndTime;
            r["Input"] = triDetail.Input;
            r["Output"] = triDetail.Output;
            r["Status"] = triDetail.Status;
            r["SourceName"] = triDetail.SourceName;
            dt.Rows.Add(r);

        }        
        public void SaveHistoryRecord(History history)
        {
            string triggerTBName = "TriggerHistory";
            string actionTBName = "ActionHistory";
            string runTBName = "RunHistory";

            DataTable dtTrigger = new DataTable();
            DataTable dtAction = new DataTable();
            DataTable dtRun = new DataTable();
            SqlBulkCopy bulkCopyTrigger = PrepareBulk(triggerTBName, ref dtTrigger);
            SqlBulkCopy bulkCopyAction = PrepareBulk(actionTBName, ref dtAction);
            SqlBulkCopy bulkCopyRun = PrepareRunBulk(runTBName, ref dtRun);

            int cnt = 0;
            while (cnt < history.Runs.Count)
            {
                int index;
                for (int i = 0; i < 450 && (i + cnt) < history.Runs.Count; i++)
                {
                    index = cnt + i;
                    Record run = history.Runs[index];
                    ApiDetail triggerHistory = run.TriggerHistory;
                    List<ApiDetail> actionHistory = run.ActionHistory;
                    PrepareRunDT(history, run, ref dtRun);
                    ApiDetail triDetail = run.TriggerHistory;
                    PrepareDT(triDetail, ref dtTrigger);
                    foreach (ApiDetail actionDetail in run.ActionHistory)
                    {
                        PrepareDT(actionDetail, ref dtAction);
                    }
                }
                cnt += 450;
                bulkCopyRun.WriteToServer(dtRun);
                dtRun.Rows.Clear();
                bulkCopyTrigger.WriteToServer(dtTrigger);
                dtTrigger.Rows.Clear();
                bulkCopyAction.WriteToServer(dtAction);
                dtAction.Rows.Clear();
            }

            bulkCopyTrigger.Close();
            bulkCopyAction.Close();
            bulkCopyRun.Close();
        }

    }
}
