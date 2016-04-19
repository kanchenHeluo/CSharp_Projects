using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Search
{
    public class SearchFactory : ISearchFactory
    {
        /// <summary>
        /// create new instance of a search provider
        /// </summary>
        /// <typeparam name="T">type of search provider</typeparam>
        /// <typeparam name="S">type of search strategy</typeparam>
        /// <param name="parameters">parameters for the constructor of the search strategy</param>
        /// <returns>search provider</returns>
        public T CreateProvider<T, S>(object[] parameters)
            where T : ISearchProvider
            where S : ISearchStrategy
        {
            ISearchStrategy strategy = (S)Activator.CreateInstance(typeof(S), parameters);
            return (T)Activator.CreateInstance(typeof(T), new object[]{ strategy } );
        }


        public T CreateProvider<T, S>(string configName)
            where T : ISearchProvider
            where S : ISearchStrategy
        {
            ISearchStrategy strategy = (S)Activator.CreateInstance(typeof(S), configName);
            return (T)Activator.CreateInstance(typeof(T), new object[] { configName });
        }
    }
}
