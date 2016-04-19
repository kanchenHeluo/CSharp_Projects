using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global.Search.Providers;
using Global.Search.Strategies;
using Global.Search;
using Global.SearchSync;
using Global.SearchSync.Strategies;
using Global.SearchSync.Publishers;
using System.Data.SqlClient;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Global.Search.Common;
namespace Global.SearchService
{
    public class SearchService
    {
        /// <summary>
        /// Search provider factory
        /// </summary>
        private static readonly ISearchFactory factory = new SearchFactory();

        private static readonly ISyncFactory syncFactory = new SyncFactory();

        private static string apiKey;
        private static string searchNameSpace;
        private static TimeSpan timeout;
        private static ISearchProvider searchProvider;
        private static ISyncPublisher syncPublisher;
        public SearchService(string apiKey, string searchNameSpace, TimeSpan timeout, uint retries = 4, bool batchUpdates = false, uint? batchSize = 999)
        {
            SearchService.apiKey = apiKey;
            SearchService.searchNameSpace = searchNameSpace;
            SearchService.timeout = timeout;

            object[] parameters = new object[] { searchNameSpace, apiKey, timeout };
            searchProvider = factory.CreateProvider<SearchProvider, AzureStrategy>(parameters);

            object[] syncParameters = new object[] { searchNameSpace, apiKey, timeout, retries, batchUpdates, batchSize };
            syncPublisher = syncFactory.CreatePublisher<AzureSearchSyncPublisher, AzureSyncStrategy>(syncParameters);
        }


       

        /// <summary>
        /// Search with built query string which supported by the selected search provider strategy
        /// </summary>
        /// <typeparam name="T">results</typeparam>
        /// <param name="query">well formatted query string uri</param>
        /// <param name="indexname">index to search on</param>
        /// <returns>collection of results</returns>
        public async Task<ISearchResult<T>> Search<T>(string query, string indexname)
        {            
            return await searchProvider.Search<T>(query, indexname);
        }

        /// <summary>
        /// Suggestions based on the search input of a user.
        /// </summary>
        /// <param name="searchText">search input from user</param>
        /// <param name="indexName">name of the index</param>
        /// <param name="selectedColumns">selected columns the data, null only includes key</param>
        /// <param name="ODataFilter">Filter based on Azure's Search REST Api</param>
        /// <returns>suggestions for search terms</returns>
        public T Suggest<T>(string searchText, string indexName, string[] selectedColumns = null, string ODataFilter = null)
        {
            return searchProvider.Suggest<T>(searchText, indexName, selectedColumns, ODataFilter);
        }

        /// <summary>
        /// provide a stream of data to upload to the index specified
        /// </summary>
        /// <param name="data">stream of data</param>
        /// <param name="onError">on error event</param>
        /// <param name="onCompleted">on completed event</param>
        /// <param name="index"></param>
        public void UploadData(IObservable<Dictionary<string, object>> data, Action<GlobalSearchSyncException> onError, Action onCompleted, string index)
        {
            syncPublisher.UpdateData(data, onError, onCompleted, index);
        }        

        /// <summary>
        /// Delete an index in the search provider, requires an Admin key
        /// </summary>
        /// <param name="indexName">name of the index</param>
        public void DeleteIndex(string indexName)
        {
            syncPublisher.DeleteIndex(indexName);
        }

        /// <summary>
        /// Create an index in the search provider given a schema. Requires an admin key
        /// </summary>
        /// <typeparam name="T">type of the schema</typeparam>
        /// <param name="schema">schema to upload</param>
        public void CreateIndex<T>(T schema)
        {
            syncPublisher.CreateIndex<T>(schema);            
        }

        /// <summary>
        /// Checks if a schema exists for the given index name on the searh provider.
        /// </summary>
        /// <param name="indexName">name of the index</param>
        /// <returns>true if yes</returns>
        public bool SchemaExists(string indexName)
        {
            return syncPublisher.SchemaExists(indexName);
        }
       

    }
}
