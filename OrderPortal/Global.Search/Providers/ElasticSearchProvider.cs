using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlainElastic;
using PlainElastic.Net.Queries;
using PlainElastic.Net;
using PlainElastic.Net.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Web.Script.Serialization;


namespace Global.Search.Providers
{
  
   public  class ElasticSearchProvider : ISearchProvider
    {
        /// <summary>
        /// search strategy used internal to search and suggest
        /// </summary>
        private static ISearchStrategy searchStrategy;

        /// <summary>
        /// constructor to initialize the provider
        /// </summary>
        /// <param name="strategy">search strategy</param>
        public ElasticSearchProvider(ISearchStrategy strategy)
        {
            //if (strategy == null)
            //{
            //    throw new ArgumentNullException("strategy");
            //}

           // searchStrategy = strategy;
        }
         public ISearchResult<T> Search<T>(string query, string indexname,string indextype)
        {            
            if (string.IsNullOrWhiteSpace(query))
            {
                // assume search all? What should we do here?
            }
            var connection = new ElasticConnection("uscbvcwebmsl01.redmond.corp.microsoft.com", 9200);
            var serializer = new JsonNetSerializer();

            var result = connection.Post(Commands.Search(indexname, indextype), query);
            var searchDocuments = serializer.ToSearchResult<T>(result).Documents;
            return new SearchResult<T>() { SearchResults = searchDocuments.ToList() };
          
        }

        public Task<ISearchResult<T>> Search<T>(string query, string indexname,string search,string indextype)
         {
             throw new NotImplementedException();
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

        public Task<string> UpdateAsync(object content, string index, string service = null, string apikey = null)
        {
            throw new NotImplementedException();
           
        }



        public Task<ISearchResult<T>> SearchAsync<T>(string query, string indexname = "", string search = "", string indexType = "")
        {
            throw new NotImplementedException();
        }
    }
}
