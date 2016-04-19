using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;
using Web.UI.Cache;

namespace Web.UI.Providers
{
    public class InMemoryCacheProvider : ICacheProvider
    {
        #region Private
        /// <summary>
        /// In Memory Cache 
        /// </summary>
        private ObjectCache Cache { get { return MemoryCache.Default; } }
        #endregion

        #region Public Method
        /// <summary>
        /// Gets object from cache
        /// </summary>
        /// <param name="key">cache key</param>
        /// <returns>Returns cached object if available, else reurns null</returns>
        public object Get(string key)
        {
            return Cache[key];
        }

        /// <summary>
        /// Sets object to cache
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="data">object to cache</param>
        /// <param name="cacheTime>expiration time of cache</param>
        public void Set(string key, object data, int cacheTime)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key, data), policy);
        }

        /// <summary>
        /// Checks whether an items is available
        /// </summary>
        /// <param name="key"></param>
        /// <returns><c>true</c> if item is cached else <c>false</c></returns>
        public bool IsCached(string key)
        {
            return (Cache[key] != null);
        }

        /// <summary>
        /// Invalidates item in cache
        /// </summary>
        /// <param name="key">Invalidates item in cache</param>
        public void Invalidate(string key)
        {
            Cache.Remove(key);
        }
        #endregion


        public T Get<T>(string key) 
        {
            return (T)Cache[key];
        }
    }
}