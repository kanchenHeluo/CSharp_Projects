using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Global.SearchSync;
using Global.SearchSync.Publishers;
using Global.SearchSync.Strategies;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using GlobalSync.UnitTest.UnitTest.DB;
using Global.Search.Common;
using GlobalSync.UnitTest.UnitTest;
using Global.Search;
using Global.Search.Providers;
using Global.Search.Strategies;

namespace GlobalSync.UnitTest
{

    [TestClass]
    public class AzureSearchSyncTest
    {



        ChangeDbProvider dataProvder;
        private string apiKey = string.Empty;
        private string searchNameSpace = string.Empty;
        private TimeSpan timeout;
        private object[] parameters;
        [TestInitialize]
        public void TestSetup()
        {
            apiKey = "AABB9C2904CDABEC6B24E8B0F54D9916";
            searchNameSpace = "products";
            timeout = TimeSpan.FromMinutes(1);
            // bool batchUpdates = false, TimeSpan? slackTimespan = null, uint? batchSize = null
            // order is important to the constructor
            uint? batchSize = 999;
            parameters = new object[] { searchNameSpace, apiKey, timeout, (uint)4, true, batchSize };
            string[] keyColumnNames = new string[] { "PartNumber", "PurchaseTypeCode", "PurchaseOptionCode", "PurchasingAccountTypeCode", "DistributionChannelTypeCode", "CountryCode", "StartEffectiveDate" };
            string connectionString = "Data Source=vzj4t7zguz.database.windows.net;Initial Catalog=moptappr;User ID=ketanp@vzj4t7zguz;Password=badboss1!";
            string query = "select * from ProductFilteredForOrders;";
            dataProvder = new ChangeDbProvider(connectionString, query, keyColumnNames, "generatedkey");
        }

        [TestMethod]
        public void TestUpdateData()
        {
            ISyncFactory factory = new SyncFactory();
            ISyncPublisher publisher = factory.CreatePublisher<AzureSearchSyncPublisher, AzureSyncStrategy>(parameters);

            publisher.UpdateData(dataProvder.ComputeSet(), OnError, OnCompleted, "productsut");

        }

        /// <summary>
        /// Highly recommend Ignore for now because the X19 longer to upload 34k documents by just using an Exist method
        /// </summary>
        [TestMethod]
        [Ignore]        
        public void TestUpdateDataExists()
        {
            ISyncFactory factory = new SyncFactory();
            ISyncPublisher publisher = factory.CreatePublisher<AzureSearchSyncPublisher, AzureSyncStrategy>(parameters);

            publisher.UpdateData(dataProvder.ComputeSet(), OnError, OnCompleted, "productsut", "generatedkey");

        }

        [TestMethod]
        public void TestGetIndex()
        {
            ISyncFactory factory = new SyncFactory();
            ISyncPublisher publisher = factory.CreatePublisher<AzureSearchSyncPublisher, AzureSyncStrategy>(parameters);
            if (!publisher.SchemaExists("productsut"))
            {
                publisher.CreateIndex<AzureSearchSchema>(GetSchema());
            }
            else
            {
                publisher.DeleteIndex("productsut");
                publisher.CreateIndex<AzureSearchSchema>(GetSchema());
            }
            AzureSearchSchema sch = publisher.GetIndex<AzureSearchSchema>("productsut");

            Assert.IsNotNull(sch);
        }
        [TestMethod]
        public void SyncTesT()
        {
            ISyncFactory factory = new SyncFactory();            
            ISyncPublisher publisher = factory.CreatePublisher<AzureSearchSyncPublisher, AzureSyncStrategy>(parameters);

            ISearchFactory searchfactory = new SearchFactory();
            object[] p = new object[] { searchNameSpace, apiKey, TimeSpan.FromMinutes(1) };
            var search = searchfactory.CreateProvider<SearchProvider, AzureStrategy>(p);
            publisher.Sync(search, "productsut");
        }
        private AzureSearchSchema GetSchema()
        {
            AzureSearchSchema schema = new AzureSearchSchema();
            schema.Name = "productsut";
            var list = new List<SearchColumn>()
                {       
                    new SearchColumn(DataType.String)
                    {
                        Name = "generatedkey",
                        Key = true,
                    },
                    new SearchColumn(DataType.String)
                    {
                        Name = "PartNumber",
                        Suggestions = true,
                    },
                    new SearchColumn(DataType.String)
                    {
                        Name = "ItemName",
                        Suggestions = true,
                    },
                    new SearchColumn(DataType.String)
                    {
                        Name = "ProductTypeCode",
                        Suggestions = true,
                    },
                    new SearchColumn(DataType.String)
                    {
                        Name = "ProductTypeName",
                        Suggestions = true,
                    },
                    new SearchColumn(DataType.String)
                    {
                        Name = "PoolName"
                    },
                    new SearchColumn(DataType.String)
                    {
                        Name = "PoolCode",
                    },
                    new SearchColumn(DataType.String)
                    {
                        Name = "ProductLineCode",
                        Suggestions = true,
                    },
                    new SearchColumn(DataType.String)
                    {
                        Name = "ProductLineName",
                        Suggestions = true,
                    },
                    new SearchColumn(DataType.String)
                    {
                        Name = "ProductEditionCode",
                        Suggestions = true,
                    },
                    new SearchColumn(DataType.String)
                    {
                        Name = "ProductEditionName",
                        Suggestions = true,
                    },
                    new SearchColumn(DataType.String)
                    {
                        Name = "PurchaseTypeCode",
                    },
                    new SearchColumn(DataType.String)
                    {
                        Name = "PurchaseOptionCode",
                    },
                    new SearchColumn(DataType.String)
                    {
                        Name = "PurchasingAccountTypeCode",
                    },
                    new SearchColumn(DataType.String)
                    {
                        Name = "DistributionChannelTypeCode",
                        Suggestions = true,
                    },
                    new SearchColumn(DataType.String)
                    {
                        Name = "CountryCode",
                    },
                    new SearchColumn(DataType.DateTimeOffset)
                    {
                        Name = "StartEffectiveDate",                        
                    },
                    new SearchColumn(DataType.DateTimeOffset)
                    {
                        Name = "EndEffectiveDate",
                    }
                };
            schema.Add(list);
            return schema;
        }

        private void OnCompleted()
        {
            Assert.IsTrue(true);
        }

        private void OnError(Exception obj)
        {
            Assert.Fail(obj.Message);
        }

      


    }
}
