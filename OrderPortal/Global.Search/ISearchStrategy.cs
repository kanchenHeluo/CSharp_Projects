using Global.Search.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Search
{
    public interface ISearchStrategy
    {

     

        Task<ISearchResult<T>> SearchAsync<T>(string query, string indexname="", string search="",string indextype = "");
        /// <summary>
        /// Method which will suggest search terms based on user input
        /// </summary>
        /// <param name="searchText">user input used to formulate suggestions</param>
        /// <param name="indexName">name of the index queried on</param>
        /// <param name="selectedColumns">selected columns</param>
        /// <param name="ODataFilter">generated OData filter to limit result sets</param>        
        /// <returns>collection of dynamic type suggestions</returns>
        T Suggest<T>(string searchText, string indexName, string[] selectedColumns = null, string ODataFilter = null);

        /// <summary>
        /// Method to check whether a document exists based on its key in the index specified
        /// </summary>
        /// <param name="key">key of the document</param>
        /// <param name="indexName">index name</param>
        /// <returns>true if yes</returns>
        bool Exists(string key, string indexName);


       // IDisposable UpdateData(IObservable<Dictionary<string, object>> data, string index, bool overwriteDocuments);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="index"></param>
        /// <param name="service"></param>
        /// <param name="apikey"></param>
        /// <returns></returns>
         Task<string> SendAsync(object content, string index,string service=null,string apikey=null);
    }
}
