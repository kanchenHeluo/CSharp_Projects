using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Search
{
    public interface ISearchFactory
    {
        /// <summary>
        /// Create a new instance of a search provider 
        /// </summary>
        /// <typeparam name="T">Search Provider</typeparam>
        /// <returns>the concrete search provider type</returns>
        T CreateProvider<T,S>(object[] parameters) where T : ISearchProvider where S : ISearchStrategy;

        T CreateProvider<T, S>(string configName)
            where T : ISearchProvider
            where S : ISearchStrategy;  
    }
}
