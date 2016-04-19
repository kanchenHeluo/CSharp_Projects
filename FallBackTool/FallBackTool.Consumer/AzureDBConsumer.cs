using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FallBackTool.Consumer
{
    public class AzureDBConsumer
    {
        private SqlConnection conn;
        private string connectionstr;

        public AzureDBConsumer()
        {
            connectionstr = ConfigurationManager.ConnectionStrings["AzureDB"].ConnectionString;
            ValidConnection();
        }
        ~AzureDBConsumer() {
            //conn.Close();
            //conn.Dispose();
        }

        private SqlConnection ValidConnection()
        {
            if (conn == null || conn.State == ConnectionState.Closed)
            {
                Console.WriteLine();
                Console.WriteLine("[AzureDB Consumer]:Start initialize DB connection...");
                conn = new SqlConnection(connectionstr);

                conn.Open();
                Console.WriteLine("[AzureDB Consumer]:DB Connection initialized");
            }

            return conn;
        }

        public void Consume(DataTable dt)
        {
            Console.WriteLine("[AzureDB Consumer]:Data merge in prograss...");
            using (var command = conn.CreateCommand())
            {
                string tempName = string.Empty;
                switch (ConfigurationManager.AppSettings["DBName"])
                {
                    case "GL04Account":
                        tempName = "Account";
                        break;
                    case "GL04InternalOrder":
                        tempName = "InternalOrder";
                        break;
                    default:
                        tempName = "Contact";
                        break;
                }
                DataTable temp = dt.Copy();
                //while(temp.Rows.Count>5)
                //{
                //    temp.Rows.RemoveAt(5);
                //}
                //temp.AcceptChanges();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "CRMAzure.spMerge" + tempName + "Gap";
                SqlParameter para = command.Parameters.AddWithValue("@sourceTb", temp);
                para.SqlDbType = SqlDbType.Structured;
                para.TypeName = "CRMAzure.tp" + tempName + "DeltaTableType";

                int count = int.Parse(command.ExecuteScalar().ToString());
                Console.WriteLine(string.Format("[AzureDB Consumer]:Data merge complete. {0} records be affected.", count.ToString()));
                ILog log = LogManager.GetLogger(typeof(AzureDBConsumer));
                log.Info(string.Format("[AzureDB Consumer]:Data merge complete. {0} records be affected.", count.ToString()));
            }
        }

    }
}
