using Global.SearchSync;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace GlobalSync.UnitTest.UnitTest.DB
{
    public class ChangeDbProvider
    {
        private static string _connectionString;
        private static string _query;
        private static string[] keyColumnNames;
        private static string _keyColumnName;
        public ChangeDbProvider(string connectionString, string query, string[] keyColumnNames, string keyColumnName)
        {
            _connectionString = connectionString;
            _query = query;
            _keyColumnName = keyColumnName;
            ChangeDbProvider.keyColumnNames = keyColumnNames;
        }


        public IObservable<Dictionary<string, object>> ComputeSet()
        {
            SqlConnection con = new SqlConnection(_connectionString);
            try
            {
                var results = IteratedRows(con, keyColumnNames).ToObservable();

                return results;
            }            
            catch(SqlException)
            {
                throw;
            }
            finally
            {
                con.Close();
            }
        }

        private IEnumerable<Dictionary<string, object>> IteratedRows(SqlConnection con, string[] keyColumnNames)
        {
            
            con.Open();

            SqlCommand cmd = new SqlCommand(_query, con);


            using (SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
            {
                while (reader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();

                    for (int i = 0; i < reader.VisibleFieldCount; i++)
                    {
                        object value = reader.GetValue(i);
                        row[reader.GetName(i)] = value is DBNull ? null : value;
                    }

                    Utility.GenerateKeyHash(_keyColumnName, keyColumnNames, row);

                    

                    // Yield rows as we get them and avoid buffering them so we can easily handle
                    // large datasets without memory issues
                    yield return row;
                }
            }
        }

        
    }
}
