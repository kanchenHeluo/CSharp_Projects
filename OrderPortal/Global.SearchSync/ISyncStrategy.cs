using Global.Search.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.SearchSync
{
    public interface ISyncStrategy
    {
        /// <summary>
        /// Event called when receiving an other row in the stream of data.
        /// </summary>
        /// <param name="row">row to be processed</param>
        void OnNextSync(Dictionary<string, object> row);

        /// <summary>
        /// Operation to create an index on the underlying storage provider
        /// </summary>
        /// <typeparam name="T">type of schema</typeparam>
        /// <param name="indexSchema">schema for the index</param>
        void CreateIndex<T>(T indexSchema);

        /// <summary>
        /// Set the configuration for the actions and the index which is being operated on.
        /// </summary>
        /// <param name="index">index which the strategy should operate on</param>
        /// <param name="onError">on Error event handler</param>
        /// <param name="onCompleted">on Completed event handler</param>
        /// <param name="keyColumnName">name of the column which is the key, if null all data will override, else will check if the data exists first and ignore if it does</param>
        void SetConfiguration(string index, Action<GlobalSearchSyncException> onError, Action onCompleted, string keyColumnName = null);

        /// <summary>
        /// Method used to handle the stream of data for an error when subscribing
        /// </summary>
        /// <param name="ex">the exception thrown from the stream</param>
        void OnError(Exception ex);

        /// <summary>
        /// Method to use to handle the on completed event for the stream
        /// </summary>
        void OnCompleted();

        /// <summary>
        /// Get the index based on the indexname
        /// </summary>
        /// <typeparam name="T">type of the schema</typeparam>
        /// <param name="indexName">name of the index</param>
        /// <returns>the schema</returns>
        T GetIndex<T>(string indexName);

        /// <summary>
        /// Check if a schema exists for the specified index
        /// </summary>
        /// <param name="indexName">the index name</param>
        /// <returns>true if exists</returns>
        bool SchemaExists(string indexName);

        /// <summary>
        /// Delete the index and all data associated with the index based on the name
        /// </summary>
        /// <param name="indexName">the index name</param>
        void DeleteIndex(string indexName);
    }
}
