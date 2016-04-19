using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.SearchSync
{
    public interface ISyncFactory
    {
        /// <summary>
        /// Create a new instance of a search provider 
        /// </summary>
        /// <typeparam name="T">Search Publisher</typeparam>
        /// <typeparam name="S">Type of the Sync Strategy used</typeparam>
        /// <param name="parameters">the constructor parameters for the strategy</param>
        /// <returns>the concrete search publisher type</returns>
        T CreatePublisher<T, S>(object[] parameters)
            where T : ISyncPublisher
            where S : ISyncStrategy;  
    }
}
