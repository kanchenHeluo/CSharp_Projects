using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;
    using System.Web;

    /// <summary>
    /// Interface definition for cache provider
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Gets object from cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);


        /// <summary>
        /// Gets object from cache
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);


        /// <summary>
        /// Sets object to cache
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="data">object to cache</param>
        /// <param name="cacheTime>expiration time of cache</param>
        void Set(string key, object data, int cacheTime);

        /// <summary>
        /// Checks whether an items is available
        /// </summary>
        /// <param name="key"></param>
        /// <returns><c>true</c> if item is cached else <c>false</c></returns>
        bool IsCached(string key);

        /// <summary>
        /// Invalidates item in cache
        /// </summary>
        /// <param name="key">Invalidates item in cache</param>
        void Invalidate(string key);
    }
}