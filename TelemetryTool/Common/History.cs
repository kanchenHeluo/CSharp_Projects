using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class History
    {
        public Guid SubscriptionId { get; set; }
        public string AppName { get; set; }
        public string Entity { get; set; }

        public string ResourceGroupName { get; set; }
        public string WorkflowName { get; set; }
        public int WorkflowTypeId { get; set; }
        public List<Record> Runs { get; set; }

        public History()
        {
            this.Runs = new List<Record>();
        }

    }
    public class Record
    {
        public string Status { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Duration { get; set; }
        public Guid CorrelationId { get; set; }
        public string RunName { get; set; }
        public string TriggerOutput { get; set; }
        public string RecordId { get; set; }
        public string URL { get; set; }
        public List<ApiDetail> ActionHistory { get; set; }
        public ApiDetail TriggerHistory { get; set; }

        public Record()
        {
            StartTime = DateTime.Today.AddYears(-100);
            EndTime = DateTime.Today.AddYears(-100);
            Duration = 0;
            RecordId = string.Empty;
            TriggerOutput = string.Empty;
            RunName = string.Empty;
            URL = string.Empty;
            ActionHistory = new List<ApiDetail>();
            TriggerHistory = new ApiDetail();
        }
    }

    public class ApiDetail
    {
        public Guid CorrelationId { get; set; }
        public Guid TrackingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string Status { get; set; }
        public string SourceName { get; set; }

        public ApiDetail()
        {
            this.CorrelationId = Guid.Empty;
            this.TrackingId = Guid.Empty;
            this.StartTime = DateTime.Today.AddYears(-100);
            this.EndTime = DateTime.Today.AddYears(-100);
            this.Status = string.Empty;
            this.SourceName = string.Empty;
            this.Output = string.Empty;
        }
    }
}
