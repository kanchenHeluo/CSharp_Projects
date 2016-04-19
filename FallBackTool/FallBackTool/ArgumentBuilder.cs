using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallBackTool
{
    public class ArgumentBuilder
    {
        public ArgumentBuilder()
        {
            Initialize();

            Valid = true;

            OutputInitializedInfo();
        }
        public ArgumentBuilder(string[] args)
        {
            try
            {
                Initialize();
                if (ValidArguments(args))
                {
                    foreach (string arg in args)
                    {
                        string[] property = arg.Replace(" ", string.Empty).Split(new char[] { ':' });
                        switch (property[0].ToLower())
                        {
                            case "/enablepullfromstaging":
                                EnablePullFromStaging = bool.Parse(property[1]);
                                break;
                            case "/enablemergetoazuredb":
                                EnableMergeToAzureDB = bool.Parse(property[1]);
                                break;
                            case "/enableprocesstocrm":
                                EnableProcessToCRM = bool.Parse(property[1]);
                                break;

                            case "/lastsyncversion":
                                LastSyncVersion = property[1];
                                break;
                            case "/lastsyncdate":
                                LastSyncDate = property[1];
                                break;
                            case "/lastsyncid":
                                LastSyncId = property[1];
                                break;
                            case "/filterkey":
                                FilterKey = property[1];
                                break;
                            case "/valuelist":
                                ValueList = property[1];
                                break;
                            default:
                                throw new Exception(string.Format("Unsupported argument:{0}", property[0]));
                        }
                    }

                    OutputInitializedInfo();
                }
            }
            catch (Exception ex)
            {
                Valid = false;
                Console.WriteLine(string.Format("[Exception] ", ex.Message));
                Console.WriteLine();
                ILog log = LogManager.GetLogger(ex.GetType());
                log.Error("[Exception]:", ex);

                OutputStandardArguments();
            }
        }

        #region proterties
        public bool Valid { get; set; }

        public bool EnablePullFromStaging { get; set; }
        public bool EnableMergeToAzureDB { get; set; }
        public bool EnableProcessToCRM { get; set; }

        public string LastSyncVersion { get; set; }
        public string LastSyncDate { get; set; }
        public string LastSyncId { get; set; }
        public string FilterKey { get; set; }
        public string ValueList { get; set; }
        #endregion

        #region functions
        private void Initialize()
        {
            EnablePullFromStaging = true;
            EnableMergeToAzureDB = false;
            EnableProcessToCRM = true;
        }

        private void OutputInitializedInfo()
        {
            if (EnablePullFromStaging)
            {
                string db = ConfigurationManager.ConnectionStrings["StagingDB"].ConnectionString.Split(';')[0].Substring(12) + "-" + ConfigurationManager.AppSettings["DBName"];
                Console.WriteLine("[Data Source]:" + db);
                Console.WriteLine("[Data Table]:" + ConfigurationManager.AppSettings["TableName"]);
            }
            if (EnableMergeToAzureDB)
            {
                string adb = ConfigurationManager.ConnectionStrings["AzureDB"].ConnectionString.Split(';')[0].Substring(11);
                Console.WriteLine("[Azure SQL Server]:" + adb);
            }
            if (EnableProcessToCRM)
            {
                Console.WriteLine("[CRM URL]:" + ConfigurationManager.AppSettings["ida:CrmResourceURL"]);
                Console.WriteLine("[CRM Entity]:" + ConfigurationManager.AppSettings["EntityName"]);
            }
            Console.WriteLine();
        }

        private bool ValidArguments(string[] args)
        {
            bool isValid = true;

            foreach (string arg in args)
            {
                if (!arg.StartsWith("/"))
                {
                    isValid = false;
                    break;
                }

                if (arg.ToLower().Equals("/help"))
                {
                    isValid = false;
                    break;
                }

                if (arg.IndexOf(":") < 0)
                {
                    isValid = false;
                    break;
                }
            }

            Valid = isValid;
            if(!isValid)
                OutputStandardArguments();
            return isValid;
        }

        public static void OutputStandardArguments()
        {
            Console.WriteLine("Arguments:");
            Console.WriteLine("   /help" + "                               show standard argument list");
            Console.WriteLine("   /EnablePullFromStaging:True/False" + "   whether enable feature that pulling data from staging database. Default value is enabled(True).");
            Console.WriteLine("   /EnableMergeToAzureDB:True/False" + "    whether enable feature that merging data into Azure database. Default value is disabled(False).");
            Console.WriteLine("   /EnableProcessToCRM:True/False" + "      whether enable feature that processing data to CRM. Default value is enabled(True).");
            Console.WriteLine("   /LastSyncVersion:[Value]" + "            using when pulling data from staging database, with [Value] to query data changes. The [Value] should be valid and previous sync successed change_tracking version on that table. For example, if last time success sync version is 1021, you want to sync all the changes after 1021, then input 1021 as lastsyncversion.");
            Console.WriteLine("   /LastSyncDate:[DateValue]" + "           using when pulling data from staging database, only support by UtilityContact table, with [DateValue] to query data changes. The [DateValue] should be valid and previous sync successed date value on that table. For example, if last time success sync data lastmodified on '2016-1-10', you want to sync all the changes after '2016-1-10', then input '2016-1-10' as lastsyncdate.");
            Console.WriteLine("   /LastSyncId:[Id]" + "                    using when pulling data from staging database, with record [Id] to query data changes. The [Id] should be valid and that record not updated after that version. For example, if last time success sync one record id is 1041, you want to sync all the changes after that point, and you know this record not change after that point, then input 1041 as lastsyncid.");
            Console.WriteLine("   /FilterKey:[KeyName]" + "                using when pulling data from staging database, with specific key and values to filter result, work with parameter \"/ValueList:[Values]\" together. The [KeyName] should be the filter key of table column name.");
            Console.WriteLine("   /ValueList:[Values]" + "                 using when pulling data from staging database, with specific key and values to filter result, work with parameter \"/FilterKey:[KeyName]\" together. The [Values] should be a collection of values and split by ','. For example, when filter by names the arguments should be \"/FilterKey:name /ValueList:'Allen','Tom'\"");

            #if DEBUG
            Console.ReadKey();
            #endif
        }
        #endregion

    }
}
