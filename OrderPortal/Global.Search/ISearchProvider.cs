using Global.Search.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Search
{
    public interface ISearchProvider
    {


        Task<ISearchResult<T>> Search<T>(string query, string indexname = "", string search = "", string indexType = "");

        Task<ISearchResult<T>> SearchAsync<T>(string query, string indexname = "", string search = "", string indexType = "");
        /// <summary>
        /// output search Suggestions based on user's input, azure suggestion has minimum of 3 chars required before
        /// results are returned.
        /// </summary>
        /// <typeparam name="T">the type of the suggestion result</typeparam>
        /// <param name="searchText">user input</param>
        /// <param name="indexName">index name</param>
        /// <param name="selectedColumns">select subset properties</param>
        /// <param name="ODataFilter">OData formatted filter on suggestions</param>
        /// <returns>search suggestions</returns>
        T Suggest<T>(string searchText, string indexName, string[] selectedColumns = null, string ODataFilter = null);



        /// <summary>
        /// Method to check if document(record) exists
        /// </summary>
        /// <param name="key">Unique key of a document</param>
        /// <param name="indexName">index name</param>
        /// <returns>True -> if document is found else false</returns>
        bool Exists(string key, string indexName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="index"></param>
        /// <param name="service"></param>
        /// <param name="apikey"></param>
        /// <returns></returns>
        Task<string> UpdateAsync(object content, string index = "", string service = null, string apikey = null);
    }
}
