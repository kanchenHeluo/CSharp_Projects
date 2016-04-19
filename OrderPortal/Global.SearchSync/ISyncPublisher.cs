using Global.Search.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global.Search;

namespace Global.SearchSync
{
    public interface ISyncPublisher 
    {
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
        IDisposable UpdateData(IObservable<Dictionary<string, object>> data, Action<GlobalSearchSyncException> onError, Action onCompleted, string index, string keyColumnName = null);

        /// <summary>
        /// Sync data in the format of an enumerable polling collection to be published to update the search catalog
        /// </summary>
        /// <param name="data">data stream</param>
        /// <param name="onError">error handler</param>
        /// <param name="onCompleted">completed handler</param>
        /// <param name="index">name of the index to operate on</param>
        /// <param name="keyColumnName">name of the column which is the key, if null all data will override, else will check if the data exists first and ignore if it does</param>
        /// <returns>the subscription to dispose when completed or errored</returns>
        IDisposable UpdateData(IEnumerable<Dictionary<string, object>> data, Action<GlobalSearchSyncException> onError, Action onCompleted, string index, string keyColumnName = null);

        /// <summary>
        /// Method to query a Sql DB and push the data to the scheduler to upload to the underlying provider.
        /// </summary>
        /// <param name="sqlQuery">sql query</param>
        /// <param name="sqlConnectionString">connection string</param>
        /// <param name="indexName">index name to upload to</param>
        /// <param name="onCompleted">on completed event handler</param>
        /// <param name="onError">on error event handler</param>
        /// <param name="overwriteDocuments">default true, overwrites any document in azure search, if set to false a perf hit will happen do to checking if the doc exists and skipping if it does</param>
        void PushAndSendData(string sqlQuery, string sqlConnectionString, string indexName, Action<GlobalSearchSyncException> onError, Action onCompleted, bool overwriteDocuments = true);

        /// <summary>
        /// Create an index on the underlying search provider
        /// </summary>
        /// <typeparam name="T">type of the schema</typeparam>
        /// <param name="indexSchema">the schema for the search provider</param>
        void CreateIndex<T>(T indexSchema);
        
        /// <summary>
        /// Check if a schema exists 
        /// </summary>
        /// <param name="indexName">the name of the schema</param>
        /// <returns>true if it exists</returns>
        bool SchemaExists(string indexName);

        /// <summary>
        /// Get the index for the specified index name
        /// </summary>
        /// <typeparam name="T">the type of the index</typeparam>
        /// <param name="indexName">the index name</param>
        /// <returns>the schema</returns>
        T GetIndex<T>(string indexName);

        /// <summary>
        /// Delete an index on the search provider
        /// </summary>
        /// <param name="indexName">the name of the schema</param>
        void DeleteIndex(string indexName);
        /// <summary>
        /// Sync data to index
        /// </summary>
        /// <param name="provider">Search Provider for syncing data</param>
        /// <param name="indexname">index name where data is to be synced</param>
        void Sync(ISearchProvider provider, string indexename);
    }
}
