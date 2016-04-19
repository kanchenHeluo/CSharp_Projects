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
    public class AzureSuggestTest
    {
        private string apiKey = string.Empty;
        private string searchNameSpace = string.Empty;
        private TimeSpan timeout;
        private object[] parameters;
        ISearchProvider searcher;
        readonly ISearchFactory factory = new SearchFactory();

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
        public void TestSuggestion()
        {
            string indexname = "agreementreference";
            string searchTerm = "572";
            string[] selectedColumns = { "*" };
            var results = searcher.Suggest<AzureSuggestionResult<Web.UI.Repositories.Models.AgreementRef>>(searchTerm, indexname, selectedColumns);
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Suggestions.Count() >= 0);
        }

        //[TestMethod]
        //public void TestSuggestion2()
        //{
        //    string indexname = "pricing";
        //    //String query = "$top=10";

        //    ISearchFactory factory = new SearchFactory();
        //    SearchProvider searcher = factory.CreateProvider<SearchProvider, AzureStrategy>(parameters);
        //    IEnumerable<dynamic> results = searcher.Suggest<dynamic>("6NH", indexname, null, "$filter=ProgramOffering ne 'ACP'");
        //    Assert.IsNotNull(results);
        //    Assert.IsTrue(results.Count() >= 0);
        //}
    }
}
