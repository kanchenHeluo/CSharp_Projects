using AzureManager;
using Common;
using DBManager;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AlertTool
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //step1: get resources info
                Console.WriteLine("Started fetch resource info:" + DateTime.Now);
                DBHandler dbHandler = new DBHandler();
                ResourceInfo resourceInfo = new ResourceInfo();
                resourceInfo = dbHandler.GetResourceInfoList();
                Console.WriteLine("End fetch resource info:" + DateTime.Now);

                //step2: outter-get all runs (failed with detail, 1 sample record per time); inner-get failed runs(with detail);
                Console.WriteLine("retrieve start:" + DateTime.Now);
                AzureHandler azureHandler = new AzureHandler();

                foreach(var subinfo in resourceInfo.Resources){                    
                    try
                    {
                        GlobalSemaphore.SemaphoreInstance.WaitOne();
                        var t = new Thread(() => azureHandler.FetchLAHistory(subinfo));
                        MachineThread.MachineThreads.Add(t);
                        t.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("fetch for application: " + subinfo.AppName + " has exception.");
                    }                    
                }
            
                Thread.Sleep(1000);
                MachineThread.WaitUntilAllThreadsComplete();
                Console.WriteLine("retrieve end:" + DateTime.Now);

                //step3: save to database
                Console.WriteLine("db write start:" + DateTime.Now);
                foreach(History history in GlobalVariables.Histories){
                    dbHandler.SaveHistoryRecord(history); 
                }
                Console.WriteLine("db write end:" + DateTime.Now);
            }
            catch (Exception ex)
            {
                Console.WriteLine("main process exception. exception message: " + ex.Message);
                throw ex;
            }        
        }
    }
}
