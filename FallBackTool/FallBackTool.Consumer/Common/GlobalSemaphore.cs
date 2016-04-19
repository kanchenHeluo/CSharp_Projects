using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FallBackTool.Consumer.Common
{
    public class GlobalSemaphore
    {
        public static Semaphore SemaphoreInstance = new Semaphore(Convert.ToInt32(ConfigurationManager.AppSettings["MaxThreadCnt"]), Convert.ToInt32(ConfigurationManager.AppSettings["MaxThreadCnt"]));
        //public static SemaphoreSlim SemaphoreInstance = new SemaphoreSlim(2, Constants.SemaphoreMaxNum);
    }
}
