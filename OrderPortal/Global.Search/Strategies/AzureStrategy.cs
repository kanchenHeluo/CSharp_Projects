using Global.Search.Common;
using Global.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Configuration;

namespace Global.Search.Strategies
{
    public class AzureStrategy : ISearchStrategy
    {
       
        /// <summary>
        /// http client to communicate to search provider
        /// </summary>
        private static HttpClient serviceClient;
        
        /// <summary>
        /// base uri to search provider
        /// </summary>
        private  Uri baseUri;
        private Uri postUri;
        private string indexname;

        /// <summary>
        /// specfied api version
        /// </summary>
        private string apiVersion = "api-version=2014-07-31-Preview";


        private JsonSerializerSettings serializationSettings;
        /// <summary>
        /// constructer to initilize the search strategy
        /// </summary>
        /// <param name="searchServiceNamespace">name for the azure search</param>
        /// <param name="apiKey">api key for authentication</param>
        /// <param name="timeout">time out for azure search requests</param>
        public AzureStrategy(string searchServiceNamespace, string apiKey, TimeSpan timeout)
        {
            Configure(searchServiceNamespace, apiKey, timeout);
        }

        private void Configure(string searchServiceNamespace, string apiKey, TimeSpan timeout)
        {
            serviceClient = new HttpClient();
            if (string.IsNullOrWhiteSpace(searchServiceNamespace))
            {
                throw new ArgumentNullException("serviceUri");
            }
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException("searchServiceApiKey");
            }
            baseUri = new Uri(string.Format("https://{0}.Search.windows.net/", searchServiceNamespace));
            serviceClient.BaseAddress = baseUri;
            serviceClient.DefaultRequestHeaders.Add("api-key", apiKey);
            serviceClient.DefaultRequestHeaders.Add("Accept", "application/json");
            serviceClient.Timeout = timeout.Duration();
          
        }

        public AzureStrategy(string configKey)
        {
           var searchSettings = ConfigurationManager.AppSettings[configKey];
           var settings=searchSettings.With(c => c.Split(";".ToCharArray()).With(a => a.ToList()));
          if(settings.Count==3)
          {
              indexname = settings[2];
              Configure(settings[1], settings[0], TimeSpan.FromMinutes(1));
          }
            

        }     

        public async Task<ISearchResult<T>> SearchAsync<T>(string queryUri, string indexName, string search, string indexType = "")
        {
            indexName = string.IsNullOrWhiteSpace(indexName) ? indexname : indexName;
            if (string.IsNullOrWhiteSpace(queryUri))
            {
                throw new ArgumentNullException("query");
            }
            if (string.IsNullOrWhiteSpace(indexName))
            {
                throw new ArgumentNullException("indexName");
            }
            string requestUri = (search == "" || search==null) ? string.Format("indexes/{0}/docs?{1}&{2}", indexName, queryUri, apiVersion) : string.Format("indexes/{0}/docs?{1}&{2}&{3}", indexName, queryUri, apiVersion, search);

            HttpResponseMessage response = await serviceClient.GetAsync(requestUri);
            EnsureSuccessfulSearchResponse(response);
            StreamContent serializedContent = (StreamContent)response.Content;
            string content = serializedContent.ReadAsStringAsync().Result;
            var output = JsonConvert.DeserializeObject<AzureSearchResult<T>>(content);

            try
            {
                return output;
            }
            catch (JsonSerializationException ex)
            {
                throw new GlobalSearchException("The response could not be deserialized into the requested type.", ex);
            }
        }

        /// <summary>
        /// method to verify success for the response
        /// </summary>
        /// <param name="response">http response</param>
        private async  void EnsureSuccessfulSearchResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                string error = response.Content == null ? null : await response.Content.ReadAsStringAsync();
                throw new GlobalSearchException("Search request failed: " + error);
            }
        }

        #region Suggest
        /// <summary>
        /// Method which will suggest search terms based on user input
        /// http://msdn.microsoft.com/en-us/library/azure/dn798936.aspx
        /// </summary>
        /// <param name="searchText">user input used to formulate suggestions</param>
        /// <returns>collection of dynamic type suggestions</returns>
        public T Suggest<T>(string searchText, string indexName, string[] selectedColumns = null, string ODataFilter = null)
        {
            if (string.IsNullOrWhiteSpace(indexName))
            {
                throw new ArgumentNullException("indexName");
            }
            if (searchText.Length < 3 || searchText.Length > 25)
            {
                // currently a requirement for azure search
                // we don't throw here because input may not be complete
                return default(T);
            }

            string requestUri = string.Format("indexes/{0}/docs/suggest?{1}&search={2}", indexName, apiVersion, Uri.EscapeDataString(searchText));
            StringBuilder uri = new StringBuilder(requestUri);

            uri = BuildSuggestionSelectUri(uri, selectedColumns);
            uri = BuildSuggestionFilterUri(uri, ODataFilter);
            HttpResponseMessage response = serviceClient.GetAsync(uri.ToString()).Result;
            EnsureSuccessfulSearchResponse(response);
            StreamContent serializedContent = (StreamContent)response.Content;
            string content = serializedContent.ReadAsStringAsync().Result;
            try
            {
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch (JsonSerializationException ex)
            {
                throw new GlobalSearchException("The response could not be deserialized into the requested type.", ex);
            }            
        }

        /// <summary>
        /// Sub routine to add an OData filter to the uri to help filter suggestions, will do logic to validate and concat correctly.
        /// </summary>
        /// <param name="uri">request Uri</param>
        /// <param name="ODataFilter">filter in the format of "$filter=column eq val and ..." or "column eq val and ..." </param>
        /// <returns>updated uri</returns>
        private static StringBuilder BuildSuggestionFilterUri(StringBuilder uri, string ODataFilter)
        {
            if (string.IsNullOrWhiteSpace(ODataFilter))
            {
                return uri;
            }

            if (ODataFilter.IndexOf("$filter=") == -1)
            {
                // filter doesn't exist, so concat it
                uri.AppendFormat("&filter={0}", ODataFilter);
            }
            else if (ODataFilter.IndexOf("$filter=") > 0)
            {
                // filter is malformed
                throw new ArgumentException("ODataFilter needs to be formatted to contain $filter= at the start of the string or not at all.");
            }
            else
            {
                // filter is formed correctly
                uri.AppendFormat("&{0}", ODataFilter);
            }
            return uri;
        }

        /// <summary>
        /// build the suggestion uri with the appropriate selected columns
        /// </summary>
        /// <param name="uri">the uri string</param>
        /// <param name="selectedColumns">selected columns for the suggest to return</param>
        /// <returns>the update string builder</returns>
        private static StringBuilder BuildSuggestionSelectUri(StringBuilder uri, string[] selectedColumns)
        {
            if (selectedColumns != null && selectedColumns.Length > 0)
            {
                uri.Append("&$select=");
                int idx = 0;
                foreach (string col in selectedColumns)
                {
                    if (string.IsNullOrWhiteSpace(col))
                    {
                        continue;
                    }

                    uri.Append(col);
                    if (idx != selectedColumns.Length - 1)
                    {
                        uri.Append(",");
                    }
                    idx++;
                }
            }
            return uri;
        }
        #endregion       

        #region Exists
        /// <summary>
        /// Method to check if document(record) exists in the collection
        /// </summary>
        /// <param name="key">Unique key of the document</param>
        /// <param name="indexName">index name</param>
        /// <returns>True - > if Document exists, else false</returns>
        public bool Exists(string key, string indexName)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException("key");
            }
            if (string.IsNullOrWhiteSpace(indexName))
            {
                throw new ArgumentNullException("indexName");
            }
            string requestUri = string.Format("indexes/{0}/docs/{1}?{2}", indexName, key, apiVersion);

            HttpResponseMessage response = serviceClient.GetAsync(requestUri).Result;
            return response.IsSuccessStatusCode;
        }
        #endregion

        public async Task<string> SendAsync(object data, string index = null, string service = null, string apikey = null)
        {
            index=string.IsNullOrWhiteSpace(index)?this.indexname:index;
            List<object> DataList = new List<object>();
            DataList.Add(data);
            var batch = new
            {
                value = DataList
            };
            if (index != null && service != null && apikey != null)
            {
                Intialize(service, apikey, TimeSpan.FromMinutes(100));
            }
            string content = JsonConvert.SerializeObject(batch, serializationSettings);
            HttpContent httpContent = new StringContent(content, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            string requestUri = string.Format("indexes/{0}/docs/index?{1}", index, apiVersion);
            var response =await serviceClient.PostAsync(requestUri, httpContent);
            EnsureSuccessfulSearchResponse(response);
            return response.StatusCode.ToString();

        }
      
        private void Intialize(string searchServiceNamespace, string apiKey, TimeSpan timeout)
        {
            serviceClient = new HttpClient();
            if (string.IsNullOrWhiteSpace(searchServiceNamespace))
            {
                throw new ArgumentNullException("serviceUri");
            }
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ArgumentNullException("searchServiceApiKey");
            }
            postUri = new Uri(string.Format("https://{0}.Search.windows.net/", searchServiceNamespace));
            serviceClient.BaseAddress = postUri;
            serviceClient.DefaultRequestHeaders.Add("api-key", apiKey);
            serviceClient.Timeout = timeout.Duration();
            serializationSettings = new JsonSerializerSettings
            {
        #if DEBUG
                Formatting = Formatting.Indented, // for readability, change to None for compactness
#else
                Formatting = Formatting.None, // for readability, change to None for compactness
#endif
                ContractResolver = new DefaultContractResolver(),
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            serializationSettings.Converters.Add(new StringEnumConverter());

        }

    }
}
