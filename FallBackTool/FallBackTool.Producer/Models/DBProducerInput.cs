using log4net;
using System;
using System.Configuration;

namespace FallBackTool.Producer.Models
{
    public class DBProducerInput
    {
        public DBProducerInput()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["StagingDB"].ConnectionString;
            DBName = GetValuefromConfigure("DBName");
            TableName = GetValuefromConfigure("TableName");
            PrimaryKey = GetValuefromConfigure("PrimaryKey");
        }

        #region properties
        public string ConnectionString { get; set; }
        public string DBName { get; set; }
        public string TableName { get; set; }
        public string PrimaryKey { get; set; }

        public string LastSyncDate { get; set; }
        public string LastSyncId { get; set; }
        public string LastSyncVersion { get; set; }
        public string FilterKey { get; set; }
        public string ValueList { get; set; }
        #endregion

        private string GetValuefromConfigure(string element)
        {
            return ConfigurationManager.AppSettings[element];
        }

        public bool ValidInputs()
        {
            bool valid = true;
            if (string.IsNullOrEmpty(ConnectionString))
                valid = false;
            else if (string.IsNullOrEmpty(DBName))
                valid = false;
            else if (string.IsNullOrEmpty(TableName))
                valid = false;
            else if (string.IsNullOrEmpty(PrimaryKey))
                valid = false;

            if (valid)
                Console.WriteLine("[DB Producer]:DB Configure valid.");
            else
            {
                Console.WriteLine("DB Configure invalid.");
                throw new Exception("DB Configure invalid");
            }
            return valid;
        }
    }
}
