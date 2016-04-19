using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ResourceInfo
    {
        public List<SubscriptionInfo> Resources { get; set; }

        public ResourceInfo()
        {
            Resources = new List<SubscriptionInfo>();
        }

        public void AddItem(SubscriptionInfo subscription)
        {
            Resources.Add(subscription);
        }
    }
    public class SubscriptionInfo
    {
        public string AppName { get; set; }
        public string EntityName { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public string WorkflowName { get; set; }
        public int WorkflowTypeId { get; set; }
        public string crmResourceUrl { get; set; }
        public string crmSecondKey { get; set; }
        public string dbPrimaryKey { get; set; }
        public string LastRunName { get; set; } //for run history
    }
}
