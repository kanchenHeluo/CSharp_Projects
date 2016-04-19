using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Search.Providers
{
    public class SearchProvider : ISearchProvider
    {
        /// <summary>
        /// search strategy used internal to search and suggest
        /// </summary>
        private static ISearchStrategy searchStrategy;

        /// <summary>
        /// constructor to initialize the provider
        /// </summary>
        /// <param name="strategy">search strategy</param>
        public SearchProvider(ISearchStrategy strategy)
        {
            if (strategy == null)
            {
                throw new ArgumentNullException("strategy");
            }

            searchStrategy = strategy;
        }

      
        public async Task<ISearchResult<T>> SearchAsync<T>(string query, string indexname="",string search="", string indexType = "")
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                // assume search all? What should we do here?
            }
            return await searchStrategy.SearchAsync<T>(query, indexname,search,indexType);
        }
        /// <summary>
        /// Check if a document exists in the collection
        /// </summary>
        /// <param name="key">unique key of the document</param>
        /// <param name="indexName">index of the document</param>
        /// <returns>True -> If document exists in the index else False</returns>
        public bool Exists(string key, string indexName)
        {
            if (string.IsNullOrEmpty(key)) 
            {
                // no key passed -> cannot retrieve the doc
                throw new ArgumentNullException("key");
            }
            return searchStrategy.Exists(key, indexName);
        }

        /// <summary>
        /// Suggestions based on the search input of a user.
        /// </summary>
        /// <param name="searchText">search input from user</param>
        /// <returns>suggestions for search terms</returns>
        public T Suggest<T>(string searchText, string indexName, string[] selectedColumns = null, string ODataFilter = null)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                // if we have no user input, how can we suggest?
                return default(T);
            }            
            return searchStrategy.Suggest<T>(searchText, indexName, selectedColumns, ODataFilter);
        }
        public async Task<string> UpdateAsync(object content, string index, string service=null, string apikey=null)
        {
            return await  searchStrategy.SendAsync(content, index, service, apikey);
        }

        public async Task<ISearchResult<T>> Search<T>(string query, string indexname = "", string search = "", string indexType = "")
        {
            return await SearchAsync<T>(query, indexname, search, indexType);
        }
    }
}
