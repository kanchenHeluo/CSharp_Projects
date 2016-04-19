using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GlobalSync.UnitTest.UnitTest.DB;
using Global.SearchSync;
using System.Collections.Generic;
using Global.SearchSync.Publishers;
using Global.SearchSync.Strategies;
using Global.Search.Common;

namespace GlobalSync.UnitTest.UnitTest
{
    [TestClass]
    public class AzureSearchUpdateSchemaTest
    {
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

        }


        [TestMethod]
        public void TestUpdateSchema()
        {
            ISyncFactory factory = new SyncFactory();
            ISyncPublisher publisher = factory.CreatePublisher<AzureSearchSyncPublisher, AzureSyncStrategy>(parameters);
            var schema = GetSchema();
            schema.Name = "tempschema";
            publisher.CreateIndex<AzureSearchSchema>(schema);

            Assert.IsTrue(publisher.SchemaExists(schema.Name), string.Format("{0} doesn't exist", schema.Name));

            try
            {
                publisher.DeleteIndex(schema.Name);
            }
            catch (Exception) 
            {
            }
        }

        [TestMethod]
        public void TestExistsSchema()
        {
            ISyncFactory factory = new SyncFactory();
            ISyncPublisher publisher = factory.CreatePublisher<AzureSearchSyncPublisher, AzureSyncStrategy>(parameters);
            bool exists = publisher.SchemaExists("productsut");
            Assert.IsTrue(exists, "productsut doesn't exist");

        }

        [TestMethod]
        public void TestGetSchema()
        {
            ISyncFactory factory = new SyncFactory();
            ISyncPublisher publisher = factory.CreatePublisher<AzureSearchSyncPublisher, AzureSyncStrategy>(parameters);
            AzureSearchSchema schema = publisher.GetIndex<AzureSearchSchema>("productsut");
            Assert.IsTrue(schema != null, "productsut doesn't exist");

        }

        [TestMethod]
        public void TestDoesntExistSchema()
        {
            ISyncFactory factory = new SyncFactory();
            ISyncPublisher publisher = factory.CreatePublisher<AzureSearchSyncPublisher, AzureSyncStrategy>(parameters);
            bool exists = publisher.SchemaExists("somecrazyindex");
            Assert.IsFalse(exists, "somecrazyindex exists");
        }

        [TestMethod]
        public void TestDeleteIndex()
        {
            ISyncFactory factory = new SyncFactory();
            ISyncPublisher publisher = factory.CreatePublisher<AzureSearchSyncPublisher, AzureSyncStrategy>(parameters);
            var template = GetSchema();
            template.Name = "tempdeleteindex";
            try
            {
                publisher.CreateIndex<AzureSearchSchema>(template);
            }
            catch (Exception)
            {
                Assert.Fail("{0} index could not be created to then be deleted", template.Name);
            }
            
            publisher.DeleteIndex(template.Name);

            Assert.IsFalse(publisher.SchemaExists(template.Name), "index still exists");
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

    }
}
