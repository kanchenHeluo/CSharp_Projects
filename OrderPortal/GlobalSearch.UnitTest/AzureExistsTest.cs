using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Global.Search;
using Global.Search.Providers;
using Global.Search.Strategies;
using System.Collections.Generic;
using System.Linq;
using Global.SearchService;

namespace GlobalSearch.UnitTest
{
    [TestClass]
    public class AzureExistsTest
    {
        private string apiKey = string.Empty;
        private string searchNameSpace = string.Empty;
        private TimeSpan timeout;
        private object[] parameters;
        readonly ISearchFactory factory = new SearchFactory();
        static ISearchProvider searcher;
        [TestInitialize]
        public void TestSetup()
        {
            apiKey = "AABB9C2904CDABEC6B24E8B0F54D9916";
            searchNameSpace = "products";
            timeout = TimeSpan.FromMinutes(1);
            // order is important to the constructor
            parameters = new object[] { searchNameSpace, apiKey, timeout };
            searcher = factory.CreateProvider<SearchProvider, AzureStrategy>(parameters);
        }

        [TestMethod]
        public void TestDocExists()
        {
            string indexname = "productsut";
            string key = "QUFBLTAzNzg4VFggQlNDQ09NUlNMTUggNy8xMi8yMDE0IDEyOjAwOjAwIEFN";
            
            bool docExists = searcher.Exists(key, indexname);
            Assert.IsTrue(docExists);
        }
        [TestMethod]
        public void TestDocNotExists()
        {
            string indexname = "productsut";
            string key = "randomKey";
            bool docExists = searcher.Exists(key, indexname);
            Assert.IsFalse(docExists);
        }
    }
}
