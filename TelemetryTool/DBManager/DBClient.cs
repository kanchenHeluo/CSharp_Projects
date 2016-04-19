using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public class DBClient
    {
        #region properties

        public SqlConnection conn = null;
        private SqlTransaction transaction;
        private string dbConnStr;

        #endregion

        #region Constructor and DeConstructor
        public DBClient()
        {
            int cnt = 0;
            while(cnt < 3){
                try
                {
                    this.dbConnStr = ConfigurationManager.ConnectionStrings["AzureDB"].ConnectionString;
                    conn = new SqlConnection(dbConnStr);
                    conn.Open();
                    break;
                }
                catch (Exception ex)
                {
                    cnt++;
                }
            }
            
        }
        /*
        ~DBClient()
        {
            conn.Dispose();
        }*/
        #endregion 

        #region Data Handling
        public void ExecWithNonReturn(string sql)
        {
            transaction = conn.BeginTransaction();
            using (SqlCommand cmd = new SqlCommand(sql, conn, transaction))
            {
                cmd.ExecuteNonQuery();
            }
            transaction.Commit();
        }

        public DataTable ExecSPReturnTable(string sp)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter da = new SqlDataAdapter())
            {
                da.SelectCommand = new SqlCommand(sp, conn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.CommandTimeout = 200;
                DataSet ds = new DataSet();
                da.Fill(ds, "result_name");

                dt = ds.Tables["result_name"];
            }
            return dt;
        }
        #endregion
    }
}
