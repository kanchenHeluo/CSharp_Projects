using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Web.UI.Providers;

namespace Web.UI.UnitTests
{
    [TestClass]
    public class RedisCacheTest
    {

        private RedisCacheProvider cacheProvider = new RedisCacheProvider();
        /// <summary>
        /// Method to test Set
        /// </summary>
        [TestMethod]
        public void TestSet()
        {
            string key = "testkey";
            string value = "testvalueString";
            cacheProvider.Set(key, value);
        }

        /// <summary>
        /// Method to test Get 
        /// </summary>
        [TestMethod]
        public void TestGet()
        {
            string value = cacheProvider.Get<string>("testkey");
            Assert.AreEqual("testvalueString", value);
        }

        
    }
}
