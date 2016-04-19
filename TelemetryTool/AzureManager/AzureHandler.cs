using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureManager
{
    public class AzureHandler
    {
        public AzureClient azClient;
        public AzureHandler()
        {
            azClient = new AzureClient();
        }

        public void FetchLAHistory(SubscriptionInfo sub)
        {
            try
            {                
                azClient = new AzureClient(sub);
                GlobalVariables.Histories.Add(azClient.FetchLAHistory(sub.LastRunName));
                GlobalSemaphore.SemaphoreInstance.Release();                
            }
            catch (Exception ex)
            {
                GlobalSemaphore.SemaphoreInstance.Release();
                Console.WriteLine("Fetch History:" + ex.Message);
            }
        }
    }
}
