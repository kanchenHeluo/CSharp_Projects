using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Global.Search.Common;
using System.Data.SqlClient;
using Global.Search;


namespace Global.SearchSync.Publishers
{
    public class AzureSearchSyncPublisher : ISyncPublisher
    {
        /// <summary>
        /// The Sync strategy which will push and update search indexes for Azure Search API
        /// </summary>
        private static ISyncStrategy strategy;

        public AzureSearchSyncPublisher(ISyncStrategy strategy)
        {
            AzureSearchSyncPublisher.strategy = strategy;
        }

        /// <summary>
        /// Sync data in the format of an observable data stream to be published to update the search catalog,
        /// refer to <see cref="http://msdn.microsoft.com/en-us/library/hh242977(v=vs.103).aspx">using Rx</see> to help with using IObservable
        /// </summary>
        /// <param name="data">data stream</param>
        /// <param name="onError">error handler</param>
        /// <param name="onCompleted">completed handler</param>
        /// <param name="index">name of the index to operate on</param>
        /// <param name="keyColumnName">name of the column which is the key, if null all data will override, else will check if the data exists first and ignore if it does</param>
        /// <returns>the subscription to dispose when completed or errored</returns>
        public IDisposable UpdateData(IObservable<Dictionary<string, object>> data, Action<GlobalSearchSyncException> onError, Action onCompleted, string index, string keyColumnName = null)
        {
            if (string.IsNullOrWhiteSpace(index))
            {
                throw new ArgumentNullException("index");
            }
            strategy.SetConfiguration(index, onError, onCompleted, keyColumnName);
            return data.Subscribe(strategy.OnNextSync, strategy.OnError, strategy.OnCompleted);
        }

        /// <summary>
        /// Sync data in the format of an enumerable polling collection to be published to update the search catalog
        /// </summary>
        /// <param name="data">data stream</param>
        /// <param name="onError">error handler</param>
        /// <param name="onCompleted">completed handler</param>
        /// <param name="index">name of the index to operate on</param>
        /// <param name="keyColumnName">name of the column which is the key, if null all data will override, else will check if the data exists first and ignore if it does</param>
        /// <returns>the subscription to dispose when completed or errored</returns>
        public IDisposable UpdateData(IEnumerable<Dictionary<string, object>> data, Action<GlobalSearchSyncException> onError, Action onCompleted, string index, string keyColumnName = null)
        {
            return UpdateData(data.ToObservable(), onError, onCompleted, index, keyColumnName);
        }

        /// <summary>
        /// Given an index schema update/create the search provider with the index defined.
        /// </summary>
        /// <typeparam name="T">AzureSearchSchema, throw otherwise</typeparam>
        /// <param name="indexSchema">the index schema</param>
        public void CreateIndex<T>(T indexSchema)
        {
            if (indexSchema == null)
            {
                throw new ArgumentNullException("indexSchema");
            }
            if (typeof(T) != typeof(AzureSearchSchema)) 
            {
                throw new InvalidOperationException("Only AzureSearchSchema is supported.");
            }
            else
            {
                AzureSearchSchema schema = indexSchema as AzureSearchSchema;                
                strategy.CreateIndex<AzureSearchSchema>(schema);
            }
        }

        /// <summary>
        /// Method to check whether an index Schema exists
        /// </summary>
        /// <param name="indexName">the index name</param>
        /// <returns>true if yes</returns>
        public bool SchemaExists(string indexName)
        {
            if (string.IsNullOrWhiteSpace(indexName))
            {
                throw new ArgumentNullException("indexName");
            }
            return strategy.SchemaExists(indexName);
        }

        /// <summary>
        /// Delete an index and all documents associated to the specified index.
        /// </summary>
        /// <param name="indexName">index name</param>
        public void DeleteIndex(string indexName)
        {
            if (string.IsNullOrWhiteSpace(indexName))
            {
                throw new ArgumentNullException("indexName");
            }
            strategy.DeleteIndex(indexName);
        }

        /// <summary>
        /// Get the index based on the specfied indexName
        /// </summary>
        /// <typeparam name="T">type of the schema</typeparam>
        /// <param name="indexName">index name</param>
        /// <returns>the schema</returns>
        public T GetIndex<T>(string indexName)
        {
            if (string.IsNullOrWhiteSpace(indexName))
            {
                throw new ArgumentNullException("indexName");
            }
            return strategy.GetIndex<T>(indexName);
        }

        /// <summary>
        /// Method to query a Sql DB and push the data to the scheduler to upload to the underlying provider.
        /// </summary>
        /// <param name="sqlQuery">sqlQuery to fetch the data from</param>
        /// <param name="sqlConnectionString">connection string</param>
        /// <param name="indexName">index name to upload to</param>
        /// <param name="onCompleted">on completed event handler</param>
        /// <param name="onError">on error event handler</param>
        /// <param name="overwriteDocuments">default true, overwrites any document in azure search, if set to false a perf hit will happen do to checking if the doc exists and skipping if it does</param>
        public void PushAndSendData(string sqlQuery, string sqlConnectionString, string indexName, Action<GlobalSearchSyncException> onError, Action onCompleted, bool overwriteDocuments = true)
        {
            if (string.IsNullOrWhiteSpace(sqlConnectionString))
            {
                onError(new GlobalSearchSyncException("SqlConnectionString is not valid", new ArgumentNullException("sqlConnectionString")));
                return;
            }
            if (string.IsNullOrWhiteSpace(sqlQuery))
            {
                onError(new GlobalSearchSyncException("SqlQuery is not valid", new ArgumentNullException("sqlQuery")));
                return;
            }
            if (!SchemaExists(indexName))
            {
                onError(new GlobalSearchSyncException("the index specified doesn't exist", new InvalidOperationException("the index specified doesn't exist")));
                return;
            }
            
            
            
            SqlConnection con = new SqlConnection(sqlConnectionString);
            try
            {
               
                string keyColumnName = null;
                if (!overwriteDocuments)
                {
                    AzureSearchSchema schema = GetIndex<AzureSearchSchema>(indexName);
                    keyColumnName = schema.GetKey().Name;
                }
               
                var results = IteratedRows(con, sqlQuery).ToObservable();
                UpdateData(results, onError, onCompleted, indexName, keyColumnName);
            }
            catch (SqlException sq)
            {
                onError(new GlobalSearchSyncException(sq.Message, sq));
                return;
            }
            catch (InvalidOperationException ex)
            {
                onError(new GlobalSearchSyncException(ex.Message, ex));
                return;
            }
            finally
            {
                con.Close();
            }
        }
        /// <summary>
        /// Method to sync data to the given index for the given Search Provider
        /// </summary>
        /// <param name="provider">SearchProvider whose index is to be synced for the data</param>
        /// <param name="indexname">Index name where the documents is to be updated</param>
        public void Sync(ISearchProvider provider, string indexname)
        {
            string connectionString = "Data Source=vzj4t7zguz.database.windows.net;Initial Catalog=moptappr;User ID=ketanp@vzj4t7zguz;Password=badboss1!";
            string sqlQuery = "select * from ProductFilteredForOrders;";
            try
            {
                PushAndSendData(sqlQuery, connectionString, indexname, (exception) => { throw exception; }, () => { }, false);
            }
            catch (SqlException sq)
            {
                throw new GlobalSearchSyncException(sq.Message, sq);
            }
            catch (InvalidOperationException ex)
            {
                throw new GlobalSearchSyncException(ex.Message, ex);
            }
            
        }

        /// <summary>
        /// Helper method to iterate on the columns recieve and yield rows as a sequence
        /// </summary>
        /// <param name="con">connection</param>
        /// <param name="sqlQuery">sqlQuery to fetch the data</param>
        /// <returns>collection of rows</returns>
        private IEnumerable<Dictionary<string, object>> IteratedRows(SqlConnection con, string query)
        {

            con.Open();

            SqlCommand cmd = new SqlCommand(query, con);


            using (SqlDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
            {
                while (reader.Read())
                {
                    Dictionary<string, object> row = new Dictionary<string, object>();

                    for (int i = 0; i < reader.VisibleFieldCount; i++)
                    {
                        object value = reader.GetValue(i);

                        value = value is DBNull ? null : value.ToString().TrimStart().TrimEnd();
                       // value = val;
                        row[reader.GetName(i)] = value is DBNull ? null : value;
                    }

                    // Yield rows as we get them and avoid buffering them so we can easily handle
                    // large datasets without memory issues
                    yield return row;
                }
            }
        }        
    }
}
