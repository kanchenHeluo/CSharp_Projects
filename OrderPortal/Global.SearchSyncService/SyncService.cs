using Global.Search;
using Global.Search.Providers;
using Global.Search.Strategies;
using Global.SearchSync;
using Global.SearchSync.Publishers;
using Global.SearchSync.Strategies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Global.Search.Common;

namespace Global.SearchSyncService
{
    public partial class SyncService : ServiceBase
    {
        ManualResetEvent CompletedEvent = new ManualResetEvent(false);

        #region Fields
        private ISyncFactory syncFactory;
        private ISyncPublisher syncPublisher;

        private ISearchFactory searchFactory;
        private ISearchProvider search;

        private readonly string searchApiKey = "06C5F6D0F0A6729B4B3A79E77C522337";
        private readonly string searchNameSpace = "productcatalog";
        private readonly string searchApiIndex = "productsut";
        private readonly string schemaJsonString = @"{""name"":""productsut"",""fields"":[{""name"":""PartNumber"",""type"":""String"",""key"":true,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""ItemName"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""ProductTypeCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""ProductTypeName"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""PoolName"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false},{""name"":""PoolCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false},{""name"":""ProductLineCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""ProductLineName"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""ProductEditionCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""ProductEditionName"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""PurchaseTypeCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false},{""name"":""PurchaseOptionCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false},{""name"":""PurchasingAccountTypeCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false},{""name"":""DistributionChannelTypeCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""CountryCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false},{""name"":""StartEffectiveDate"",""type"":""DateTimeOffset"",""key"":false,""searchable"":false,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false},{""name"":""EndEffectiveDate"",""type"":""DateTimeOffset"",""key"":false,""searchable"":false,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false}]}";
        #endregion
        public SyncService()
        {
            InitializeComponent();
            // Initialize the Provider and the Strategy
            searchFactory = new SearchFactory();
            object[] p = new object[] { searchNameSpace, searchApiKey, TimeSpan.FromMinutes(1) };
            search = searchFactory.CreateProvider<SearchProvider, AzureStrategy>(p);
            syncFactory = new SyncFactory();
            object[] syncParameters = new object[] { searchNameSpace, searchApiKey, TimeSpan.FromMinutes(1), (uint)4, true, (uint?)999 };
            syncPublisher = syncFactory.CreatePublisher<AzureSearchSyncPublisher, AzureSyncStrategy>(syncParameters);
        }

        /// <summary>
        /// OnStart for the Windows service
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {

            // instantiate a timer = 15mins & start and execute sync() of publish
            Timer frequency = new Timer(new TimerCallback(ExecuteSync), null, TimeSpan.FromSeconds(10.0), TimeSpan.FromMinutes(15));
        }

        /// <summary>
        /// Method to sync the data to the index
        /// </summary>
        /// <param name="state">State of the object that is passed to the Mehod when it is fired</param>
        private void ExecuteSync(object state)
        {
            var sync = new SearchSyncProvider(search, syncPublisher);
            if (sync.SchemaExists(searchApiIndex))
            {
                sync.DeleteIndex(searchApiIndex);
            }
            var schema = JsonConvert.DeserializeObject<AzureSearchSchema>(schemaJsonString);
            sync.CreateIndex<AzureSearchSchema>(schema);
            var index = sync.GetIndex<AzureSearchSchema>(searchApiIndex);
            syncPublisher.Sync(search, searchApiIndex);
        }


        protected override void OnStop()
        {
            CompletedEvent.Set();
        }
    }
}
