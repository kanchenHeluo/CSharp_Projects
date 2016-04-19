using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Global.Search.Providers;
using Global.Search.Strategies;

namespace Global.Search
{
    public class SearchService
    {
        /// <summary>
        /// Search provider factory
        /// </summary>
        private static readonly ISearchFactory factory = new SearchFactory();

        /// <summary>
        /// Search with built query string which supported by the selected search provider strategy
        /// </summary>
        /// <typeparam name="T">results</typeparam>
        /// <param name="query">well formatted query string uri</param>
        /// <param name="indexname">index to search on</param>
        /// <returns>collection of results</returns>
        public async Task<ISearchResult<T>> Search<T>(string query, string indexname)
        {
            string apiKey = "AABB9C2904CDABEC6B24E8B0F54D9916";
            string searchNameSpace = "products";
            TimeSpan timeout = TimeSpan.FromMinutes(1);

            // order is important for the constructor
            object[] parameters = new object[] { searchNameSpace, apiKey, timeout };
            ISearchProvider searchProvider = factory.CreateProvider<SearchProvider, AzureStrategy>(parameters);
            return await searchProvider.Search<T>(query, indexname);
        }
    }
}
