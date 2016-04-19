using Global.Search.Common;
using Global.Search;
using Global.Search.Providers;
using Global.Search.Strategies;
using Global.SearchService;
using Global.SearchSync;
using Global.SearchSync.Publishers;
using Global.SearchSync.Strategies;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ManualSearchTool
{

 

    internal class ViewModel : INotifyPropertyChanged
    {
        public ViewModel()
        {
            searchApiKey = "8A1586734634FD36EA88766A083AD3F5";
            searchNameSpace = "ngvlagreements";
            searchApiIndex = "agreements";
            sqlConnectionString = "Data Source=DS2EcxLisA2.parttest.extranettest.microsoft.com;Initial Catalog=Agreements;Trusted_Connection=Yes";
            sqlQuery = "select * from AzureSearchAgreement;";
            schemaJsonString = @"{""name"":""productsut"",""fields"":[{""name"":""PartNumber"",""type"":""String"",""key"":true,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""ItemName"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""ProductTypeCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""ProductTypeName"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""PoolName"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false},{""name"":""PoolCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false},{""name"":""ProductLineCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""ProductLineName"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""ProductEditionCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""ProductEditionName"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""PurchaseTypeCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false},{""name"":""PurchaseOptionCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false},{""name"":""PurchasingAccountTypeCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false},{""name"":""DistributionChannelTypeCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":true},{""name"":""CountryCode"",""type"":""String"",""key"":false,""searchable"":true,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false},{""name"":""StartEffectiveDate"",""type"":""DateTimeOffset"",""key"":false,""searchable"":false,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false},{""name"":""EndEffectiveDate"",""type"":""DateTimeOffset"",""key"":false,""searchable"":false,""filterable"":true,""sortable"":false,""facetable"":true,""retrievable"":true,""suggestions"":false}]}";
            IsLoaded = true;
        }
      

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _sqlConnectionString;

        public string sqlConnectionString
        {
            get { return _sqlConnectionString; }
            set
            {
                _sqlConnectionString = value;
                NotifyPropertyChanged();
            }
        }

        private string _sqlQuery;

        public string sqlQuery
        {
            get { return _sqlQuery; }
            set
            {
                _sqlQuery = value;
                NotifyPropertyChanged();
            }
        }

        private string _searchApiIndex;

        public string searchApiIndex
        {
            get { return _searchApiIndex; }
            set
            {
                _searchApiIndex = value;
                NotifyPropertyChanged();
            }
        }
        private string _searchNameSpace;

        public string searchNameSpace
        {
            get { return _searchNameSpace; }
            set
            {
                _searchNameSpace = value;
                NotifyPropertyChanged();
            }
        }

        private bool _IsLoaded;

        public bool IsLoaded
        {
            get { return _IsLoaded; }
            set 
            { 
                _IsLoaded = value;
                NotifyPropertyChanged();
            }
        }

        private string _searchApiKey;

        public string searchApiKey
        {
            get { return _searchApiKey; }
            set 
            { 
                _searchApiKey = value;
                NotifyPropertyChanged();
            }
        }

        private string _searchValue;
        public string searchValue
        {
            get { return _searchValue; }
            set
            {
                _searchValue = value;
                NotifyPropertyChanged();
            }
        }
        private string _searchColumnName;
        public string searchColumnName
        {
            get { return _searchColumnName; }
            set
            {
                _searchColumnName = value;
                NotifyPropertyChanged();
            }
        }
        private string _loadingUpdate;

        public string loadingUpdate
        {
            get { return _loadingUpdate; }
            set 
            { 
                _loadingUpdate = value;
                NotifyPropertyChanged();
            }
        }

        private string _schemaJsonString;

        public string schemaJsonString
        {
            get { return _schemaJsonString; }
            set 
            { 
                _schemaJsonString = value;
                NotifyPropertyChanged();
            }
        }


        private TimeSpan _timespanElapsed;

        public TimeSpan timespanElapsed
        {
            get { return _timespanElapsed; }
            set 
            { 
                _timespanElapsed = value;
                NotifyPropertyChanged();
            }
        }
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public async void syncData()
        {
            IsLoaded = false;
      
            
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            dispatcherTimer.Start();

               
            
            await Task.Run(() => {
                ISearchFactory factory = new SearchFactory();
                ISyncFactory syncFactory = new SyncFactory();
                object[] p = new object[] { searchNameSpace, searchApiKey, TimeSpan.FromMinutes(1) };
                ISearchProvider search = factory.CreateProvider<SearchProvider, AzureStrategy>(p);
                object[] syncParameters = new object[] { searchNameSpace, searchApiKey, TimeSpan.FromMinutes(1), (uint)4, true, (uint?)999 };
                ISyncPublisher syncPublisher = syncFactory.CreatePublisher<AzureSearchSyncPublisher, AzureSyncStrategy>(syncParameters);
                var sync = new SearchSyncProvider(search, syncPublisher);
               
                var index = sync.GetIndex<AzureSearchSchema>(searchApiIndex);
                loadingUpdate = "got created index";
                loadingUpdate = "start query and upload";
                sync.PushAndSendData(sqlQuery, sqlConnectionString, searchApiIndex, OnError, OnCompleted);
          
            });
          
        }
        public async void updateData()
        {
            
            IsLoaded = false;


            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            dispatcherTimer.Start();

            await Task.Run(() =>
            {
                ISearchFactory factory = new SearchFactory();
                ISyncFactory syncFactory = new SyncFactory();
                object[] p = new object[] { searchNameSpace, searchApiKey, TimeSpan.FromMinutes(1) };
                ISearchProvider search = factory.CreateProvider<SearchProvider, AzureStrategy>(p);
                object[] syncParameters = new object[] { searchNameSpace, searchApiKey, TimeSpan.FromMinutes(1), (uint)4, true, (uint?)999 };
                ISyncPublisher syncPublisher = syncFactory.CreatePublisher<AzureSearchSyncPublisher, AzureSyncStrategy>(syncParameters);
                var sync = new SearchSyncProvider(search, syncPublisher);
                
                var schema = JsonConvert.DeserializeObject<AzureSearchSchema>(schemaJsonString);
                loadingUpdate = "interpreted index";
                //sync.CreateIndex<AzureSearchSchema>(schema);
                //loadingUpdate = "created new index";
                var index = sync.GetIndex<AzureSearchSchema>(searchApiIndex);
                loadingUpdate = "got created index";
                loadingUpdate = "start query and upload";
                sync.PushAndSendData(sqlQuery, sqlConnectionString, searchApiIndex, OnError, OnCompleted);

            });

        }

        public async void searchData()
        {
            IsLoaded = false;


            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);

            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            dispatcherTimer.Start();


            await Task.Run(() =>
            {
                ISearchFactory factory = new SearchFactory();
                ISyncFactory syncFactory = new SyncFactory();
                object[] p = new object[] { searchNameSpace, searchApiKey, TimeSpan.FromMinutes(1) };
                ISearchProvider search = factory.CreateProvider<SearchProvider, AzureStrategy>(p);
                object[] syncParameters = new object[] { searchNameSpace, searchApiKey, TimeSpan.FromMinutes(1), (uint)4, true, (uint?)999 };
                ISyncPublisher syncPublisher = syncFactory.CreatePublisher<AzureSearchSyncPublisher, AzureSyncStrategy>(syncParameters);
                var sync = new SearchSyncProvider(search, syncPublisher);
                object[] parameters = new object[] { searchNameSpace, searchApiKey, TimeSpan.FromMinutes(1) };
                 ISearchFactory factory1 = new Global.Search.SearchFactory();
                 SearchProvider azureProvider;
                 azureProvider = factory1.CreateProvider<SearchProvider, AzureStrategy>(parameters);
               
                var index = sync.GetIndex<AzureSearchSchema>(searchApiIndex);
                
                string azureQuery = "search=" + searchValue + "&searchMode=all&searchFields=" + searchColumnName ;
                var output =(  azureProvider.SearchAsync<dynamic>(azureQuery, searchApiIndex, "", ""));
                MessageBoxResult result = MessageBox.Show(output.Result.Content.ToString(), "Confirmation");
                OnCompleted();
            });
          
           
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            timespanElapsed.Add(TimeSpan.FromSeconds(1));
        }

        private void OnCompleted()
        {
            IsLoaded = true;
            dispatcherTimer.Stop();
            MessageBox.Show("Completed");
        }

        private void OnError(Global.Search.Common.GlobalSearchSyncException obj)
        {
            IsLoaded = true;
            dispatcherTimer.Stop();
            MessageBox.Show(string.Format("{0} \n\r {1} \n\r {2}", obj.Message, obj.failedRow, obj.InnerException == null ? string.Empty : obj.InnerException.StackTrace));
        }
    }
}
