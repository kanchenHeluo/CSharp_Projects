using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.UI.Cache;
using StackExchange.Redis;
using Web.UI.Common;
using System.Configuration;

namespace Web.UI.Providers
{
    public class RedisCacheProvider
    {
        #region Private
        private IDatabase Cache
        {          
            get
            {
                //TODO : Move this line to ConfigData
                string connection = ConfigurationManager.AppSettings["EcitRedisCacheConnection"];
                return ConnectionMultiplexer.Connect(connection).GetDatabase();
            }
        }
        #endregion

        #region Public
        /// <summary>
        /// Method to retrieve a value from the cache
        /// </summary>
        /// <typeparam name="T">Type of value to be retrieved</typeparam>
        /// <param name="key">key name whose value is to be retrieved</param>
        /// <returns>value stored at the input key</returns>
        public T Get<T>(string key)
        {
            return Cache.Get<T>(key);            
        }
        /// <summary>
        /// Method to set a key->value entry into the cache
        /// </summary>
        /// <param name="key">key where the value is to be stored</param>
        /// <param name="value">value that is to be stored at the key</param>
        public void Set(string key,object value)
        {
             Cache.Set(key, value);
        }
        #endregion
    }
}