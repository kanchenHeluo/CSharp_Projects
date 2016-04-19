using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Global.Search;
using System.Linq;
using System.Collections;
using Global.Search.Providers;
using Global.Search.Strategies;
using System.Collections.Generic;
using System.Configuration;
using Global.SearchService;
using PlainElastic.Net.Queries;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Global.Search.Common;
namespace GlobalSearch.UnitTest
{
    public class AgreementReference
    {
        public string AgreementId { get; set; }
        public string AgreementNumber { get; set; }
        public string AgreementTypeCode { get; set; }
        public string IsAffiliate { get; set; }
        public string CanHaveAffiliate { get; set; }
        public string MainAgreementId { get; set; }
        public string MainAgreementNumber { get; set; }
        public string StartEffectiveDate { get; set; }
        public string ResolvedEndDate { get; set; }
        public string RenewedFlag { get; set; }
        public string EndCustomerId { get; set; }
        public string EndCustomerNumber { get; set; }
        public string ProgramCode { get; set; }
        public string LicenseAgreementTypeCode { get; set; }
        public string PricingCountryCode { get; set; }
        public string PricingCurrencyCode { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public string SalesLocationCode { get; set; }
        public string DirectCustomerId { get; set; }
        public string DirectCustomerNumber { get; set; }
        public string CustomerTypeCode { get; set; }
        public string CustomerName { get; set; }
        public string OrganizationGUID { get; set; }
    }
    public class testing
    {
        public string ColId { get; set; }
        public string AgreementId { get; set; }
        public string AgreementNumber { get; set; }
        public string ProgramCode { get; set; }
    }
    [TestClass]
    public class AzureSearchTest
    {   
        private string apiKey = string.Empty;
        private string searchNameSpace = string.Empty;
        private TimeSpan timeout;
        private object[] parameters;
        readonly ISearchFactory factory = new SearchFactory();
        private static ISearchProvider searcher;
        private static ISearchProvider elasticsearch;


        [TestInitialize]
        public void TestSetup()
        {
            // order is important to the constructor
            parameters = new object[] { "NgvlSearchConfig" };
            searcher = factory.CreateProvider<SearchProvider, AzureStrategy>(parameters);

            elasticsearch = factory.CreateProvider<ElasticSearchProvider, AzureStrategy>(parameters);
        }

      
        [TestMethod]
        public void TestSearch()
        {

            string agreementNumber = "5918671";

            string indexname = "stepupcataloglineitem";
            String query = "$filter=AgreementNumber%20eq%20'" + agreementNumber + "'";

            var results = searcher.Search<Dictionary<string, string>>(query, indexname);
            Assert.IsNotNull(results);
            //Assert.IsTrue(results.SearchResults.Count() > 0);
        }


        [TestMethod]
        public async void NgvlAgreementSearchTest()
        {
            string query = AzureSearchQuery.Instance.WithQuery("(Cos*)+(In*)").WithCount(true).WithSearchMode(SearchMode.All).
                WithSearchFields("AccountName1,AccountNumber").BuildQuery();        
            var results = await searcher.Search<dynamic>(query);           
            Assert.IsTrue(results.SearchResults.Count() > 0);
        }


        [TestMethod]
        public void NgvlAgreementSearchNoEnglishTest()
        {
            string query = AzureSearchQuery.Instance.WithQuery("谷").WithCount(true).WithSearchMode(SearchMode.All).
                WithSearchFields("AccountName1,AccountNumber").BuildQuery();
            var results = searcher.SearchAsync<dynamic>(query).Result;
            dynamic result=  results.SearchResults.FirstOrDefault();
            Assert.IsTrue(results.SearchResults.Count() > 0);
            Assert.AreEqual(result.AccountName1,"谷川商店");
           
        }

        [TestMethod]
        public void NgvlAgreementSearchPaginationTest()
        {
            string query = AzureSearchQuery.Instance.WithQuery("*").WithCount(true).WithSearchMode(SearchMode.All).
                WithSearchFields("AccountName1,AccountNumber").Skip(10).Take(10).BuildQuery();
            var results = searcher.SearchAsync<dynamic>(query).Result;         
            Assert.IsTrue(results.SearchResults.Count() > 0);
        }

        [TestMethod]
        public void TestSearch2()
        {
            string query = new QueryBuilder<AgreementReference>()
  .Query(q => q
      .Bool(b => b
         .Must(m => m.
             Term(t => t.Field(user => user.CustomerName).Value("byte")

      )
      .Match(mt => mt.Field(agg => agg.DirectCustomerNumber).Query("04876032")

         )
      )
  )
  )
  .BuildBeautified();

            var results = elasticsearch.Search<AgreementReference>(query, "sample", "agreementsreference");
            Assert.IsNotNull(results);
            //Assert.IsTrue(results);
        }
        [TestMethod]
        public void TestSearchPricing()
        {
            //  string agreementNumber = "5918671";

            string indexname = "productsut";
            String query = "$top=10";

            //AzureSearchResult<Dictionary<string, string>> results = searcher.Search<AzureSearchResult<Dictionary<string, string>>>(query, indexname);
            //Assert.IsNotNull(results);
            //Assert.IsTrue(results.SearchResults.Count() > 0);
        }


        [TestMethod]
        public void TestSearchRenewals()
        {
            //  string agreementNumber = "5918671";
            string agreementNumber = "2789859";
            string indexname = "renewals";
            String query = "$filter=AgreementId%20eq%20'" + agreementNumber + "'";

            //AzureSearchResult<Dictionary<string, string>> results = searcher.Search<AzureSearchResult<Dictionary<string, string>>>(query, indexname);
            //Assert.IsNotNull(results);
            //Assert.IsTrue(results.SearchResults.Count() > 0);
        }
        [TestMethod]
        public void SendTestMethod()
        {
            
            testing sample = new testing();            
            sample.AgreementId = "333";
            sample.AgreementNumber = "765376";
            sample.ProgramCode = "E88";
            var results = searcher.UpdateAsync(sample).Result;
        }




    
    }
}
