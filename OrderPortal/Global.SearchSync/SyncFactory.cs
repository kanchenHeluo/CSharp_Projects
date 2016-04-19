using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.SearchSync
{
    /// <summary>
    /// Create a new instance of a search provider 
    /// </summary>
    /// <typeparam name="T">Search Publisher</typeparam>
    /// <param name="parameters">the constructor parameters for the strategy</param>
    /// <returns>the concrete search publisher type</returns>
    public class SyncFactory : ISyncFactory
    {
        public T CreatePublisher<T, S>(object[] parameters)
            where T : ISyncPublisher
            where S : ISyncStrategy
        {
            ISyncStrategy strategy = (S)Activator.CreateInstance(typeof(S), parameters);
            return (T)Activator.CreateInstance(typeof(T), new object[] { strategy });
        }
    }
}
