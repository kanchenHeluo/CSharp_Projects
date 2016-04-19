using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class MachineThread
    {
        public static List<Thread> MachineThreads = new List<Thread>();
        public static void WaitUntilAllThreadsComplete()
        {
            foreach (Thread machineThread in MachineThread.MachineThreads.ToList())
            {
                machineThread.Join();
            }
        }
    }
}
