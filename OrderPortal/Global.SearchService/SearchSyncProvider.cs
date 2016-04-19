using Global.Search;
using Global.Search.Common;
using Global.SearchSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.SearchService
{
    public class SearchSyncProvider : ISearchSyncProvider
    {
        public SearchSyncProvider(ISearchProvider searchProvider, ISyncPublisher syncPublisher)
        {
            if (searchProvider == null)
            {
                throw new ArgumentNullException("searchProvider");
            }
            if (syncPublisher == null)
            {
                throw new ArgumentNullException("syncPublisher");
            }
            this.SearchProvider = searchProvider;
            this.SyncPublisher = syncPublisher;
        }

        private ISearchProvider SearchProvider { get; set; }
        private ISyncPublisher SyncPublisher { get; set; }

        /// <summary>
        /// output search results deserialized to the specified type
        /// </summary>
        /// <typeparam name="T">output type</typeparam>
        /// <param name="sqlQuery">search sqlQuery</param>
        /// <param name="indexname">index name</param>
        /// <returns>search results</returns>
        public async Task<ISearchResult<T>> Search<T>(string query, string indexname, string indexType)
        {
            return await SearchProvider.Search<T>(query, indexname);
        }
        public async Task<ISearchResult<T>> Search<T>(string query, string indexname, string search, string indexType)
        {
            return await SearchProvider.Search<T>(query, indexname, search, indexType);
        }

        /// <summary>
        /// output search Suggestions based on user's input, azure suggestion has minimum of 3 chars required before
        /// results are returned.
        /// </summary>
        /// <typeparam name="T">the type of the suggestion result</typeparam>
        /// <param name="searchText">user input</param>
        /// <param name="indexName">index name</param>
        /// <param name="selectedColumns">select subset properties, if null only key and suggested term will return</param>
        /// <param name="ODataFilter">OData formatted filter on suggestions</param>
        /// <returns>search suggestions</returns>
        public T Suggest<T>(string searchText, string indexName, string[] selectedColumns = null, string ODataFilter = null)
        {
            return SearchProvider.Suggest<T>(searchText, indexName, selectedColumns, ODataFilter);
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
        public IDisposable UpdateData(IObservable<Dictionary<string, object>> data, Action<GlobalSearchSyncException> onError, Action onCompleted, string index, string KeyColumnName = null)
        {
            return SyncPublisher.UpdateData(data, onError, onCompleted, index, KeyColumnName);
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
        public IDisposable UpdateData(IEnumerable<Dictionary<string, object>> data, Action<GlobalSearchSyncException> onError, Action onCompleted, string index, string KeyColumnName = null)
        {
            return SyncPublisher.UpdateData(data, onError, onCompleted, index, KeyColumnName);
        }

        /// <summary>
        /// Get the index for the specified index name
        /// </summary>
        /// <typeparam name="T">the type of the index</typeparam>
        /// <param name="indexName">the index name</param>
        /// <returns>the schema</returns>
        public T GetIndex<T>(string indexName)
        {
            return SyncPublisher.GetIndex<T>(indexName);
        }

        /// <summary>
        /// Create an index on the underlying search provider
        /// </summary>
        /// <typeparam name="T">type of the schema</typeparam>
        /// <param name="indexSchema">the schema for the search provider</param>
        public void CreateIndex<T>(T indexSchema)
        {
            SyncPublisher.CreateIndex(indexSchema);
        }

        /// <summary>
        /// Check if a schema exists 
        /// </summary>
        /// <param name="indexName">the name of the schema</param>
        /// <returns>true if it exists</returns>
        public bool SchemaExists(string indexName)
        {
            return SyncPublisher.SchemaExists(indexName);
        }

        /// <summary>
        /// Delete an index on the search provider
        /// </summary>
        /// <param name="indexName">the name of the schema</param>
        public void DeleteIndex(string indexName)
        {
            SyncPublisher.DeleteIndex(indexName);
        }

        /// <summary>
        /// Check to see if a document exists in underlying search provider
        /// </summary>
        /// <param name="key">key value of the document</param>
        /// <param name="indexName">index in which the document resides</param>
        /// <returns>true if exists</returns>
        public bool Exists(string key, string indexName)
        {
            return SearchProvider.Exists(key, indexName);
        }

        /// <summary>
        /// Method to query a Sql DB and push the data to the scheduler to upload to the underlying provider. Suggested usage for not overwriting documents involves small data sets as this requires additional web request to sqlQuery for index schemas and an exist method for the documents.
        /// </summary>
        /// <param name="sqlQuery">sql query</param>
        /// <param name="sqlConnectionString">connection string</param>
        /// <param name="indexName">index name to upload to</param>
        /// <param name="onCompleted">on completed event handler</param>
        /// <param name="onError">on error event handler</param>
        /// <param name="overwriteDocuments">default true, overwrites any document in azure search, if set to false a perf hit will happen do to checking if the doc exists and skipping if it does</param>
        public void PushAndSendData(string sqlQuery, string sqlConnectionString, string indexName, Action<GlobalSearchSyncException> onError, Action onCompleted, bool overwriteDocuments = true)
        {
            SyncPublisher.PushAndSendData(sqlQuery, sqlConnectionString, indexName, onError, onCompleted, overwriteDocuments);
        }

        /// <summary>
        /// Method to Sync/Upload data to the index
        /// </summary>
        /// <param name="provider">Search Provider</param>
        /// <param name="indexname">Index toi upload the documents</param>
        public void Sync(ISearchProvider provider, string indexname)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="index"></param>
        /// <param name="service"></param>
        /// <param name="apikey"></param>
        /// <returns></returns>
        public Task<string> UpdateAsync(object content, string index, string service, string apikey)
        {
            throw new NotImplementedException();
        }


        public Task<ISearchResult<T>> SearchAsync<T>(string query, string indexname = "", string search = "", string indexType = "")
        {
            throw new NotImplementedException();
        }
    }
}
