using Global.Search.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Global.SearchSync.Strategies
{
    public class AzureSyncStrategy : ISyncStrategy
    {
        /// <summary>
        /// http client to communicate to search provider
        /// </summary>
        private static HttpClient serviceClient;

        /// <summary>
        /// base uri to search provider
        /// </summary>
        private readonly Uri baseUri;

        /// <summary>
        /// specfied api version
        /// </summary>
        private readonly string apiVersion = "api-version=2014-07-31-Preview";

        /// <summary>
        /// column name for the index which is being updated
        /// </summary>
        private static string keyColumnName = null;
        /// <summary>
        /// Batched updates
        /// </summary>
        private bool batched = false;

        /// <summary>
        /// A queue of type database row.
        /// </summary>
        private Queue<Dictionary<string, object>> batchedQueue;

        /// <summary>
        /// laste update for pushing a batch to the search
        /// </summary>
        private static DateTime lastUpdate;

        /// <summary>
        /// batch size for processing
        /// </summary>
        private static uint batchSize = 0;

        /// <summary>
        /// index to update
        /// </summary>
        private static string index = string.Empty;

        /// <summary>
        /// Serializer settings
        /// </summary>
        private JsonSerializerSettings serializationSettings;

        /// <summary>
        /// Number of retries before failing
        /// </summary>
        private static uint maxRetries = 4;

        /// <summary>
        /// Current number of retries, reset after successful transaction
        /// </summary>
        private static uint currentRetries = 0;

        private Action onCompleted { get; set; }

        private Action<GlobalSearchSyncException> onError { get; set; }

        /// <summary>
        /// Constructor for 
        /// </summary>
        /// <param name="searchServiceNamespace">azure search namespace</param>
        /// <param name="apiKey">api key</param>
        /// <param name="timeout">server timeout for the operation</param>
        /// <param name="batchUpdates">true if requested smart batching</param>
        /// <param name="slackTimespan">slack time before the batch is sent</param>
        /// <param name="batchSize">unsigned int for batch processing</param>
        public AzureSyncStrategy(string searchServiceNamespace, string apiKey, TimeSpan timeout,  uint retries = 4, bool batchUpdates = false, uint? batchSize = null)
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

         
            if(batchUpdates && !batchSize.HasValue)
            {
                throw new ArgumentNullException("batchSize");
            }

            currentRetries = 0;
            baseUri = new Uri(string.Format("https://{0}.Search.windows.net/", searchServiceNamespace));
            serviceClient.BaseAddress = baseUri;
            serviceClient.DefaultRequestHeaders.Add("api-key", apiKey);
            //serviceClient.DefaultRequestHeaders.Add("Accept", "application/json");
            
            serviceClient.Timeout = timeout.Duration();

            this.batched = batchUpdates;

            if (batchUpdates)
            {
                if (batchSize.Value <= 0 || batchSize.Value > 999)
                {
                    throw new ArgumentException("batchSize must be greater than 0 and less than 1000.");
                }
                else
                {
                    AzureSyncStrategy.batchSize = batchSize.Value;
                }
            }
            batchedQueue = new Queue<Dictionary<string, object>>();
            serializationSettings =  new JsonSerializerSettings
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
            lastUpdate = DateTime.Now;
        }

        /// <summary>
        /// set the index to perform the operations on
        /// </summary>
        /// <param name="index">index name</param>
        /// <param name="onCompleted">on completed event</param>
        /// <param name="onError">on error event</param>
        /// <param name="keyColumnName">name of the column which is the key, if null all data will override, else will check if the data exists first and ignore if it does</param>
        public void SetConfiguration(string index, Action<GlobalSearchSyncException> onError, Action onCompleted, string keyColumnName = null)
        {
            if (string.IsNullOrWhiteSpace(index))
            {
                throw new ArgumentNullException("index");
            }
            if (onError == null)
            {
                throw new ArgumentNullException("onError");
            }

            if (onCompleted == null)
            {
                throw new ArgumentNullException("onCompleted");
            }
            AzureSyncStrategy.keyColumnName = keyColumnName;
            AzureSyncStrategy.index = index;
            this.onError = onError;
            this.onCompleted = onCompleted;
        }

        /// <summary>
        /// Given a search object batch it and upload to azure search
        /// </summary>
        /// <param name="row">the data row</param>
        public void OnNextSync(Dictionary<string,object> row)
        {
            if (!string.IsNullOrEmpty(AzureSyncStrategy.keyColumnName))
            {
                object val = row[AzureSyncStrategy.keyColumnName];
                string keyValue = JsonConvert.SerializeObject(val);
                if (val != null && Exists(keyValue, AzureSyncStrategy.index))
                {
                    // if the value exists we are going to ignore it
                    return;
                }
            }
            
            if (this.batched)
            {                
                BatchedUpload(row);
            }
            else
            {
                AddActionandEnqueue(row);
                DequeueAll();
            }
        }

        /// <summary>
        /// Method to check if document(record) exists in the collection
        /// </summary>
        /// <param name="key">Unique key of the document</param>
        /// <param name="indexName">index name</param>
        /// <returns>True - > if Document exists, else false</returns>
        private bool Exists(string key, string indexName)
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

        /// <summary>
        /// Batch decisions to dequeue or enqueue based on the slack time and size
        /// </summary>
        /// <param name="searchObject">row to enqueue</param>
        private void BatchedUpload(Dictionary<string, object> searchObject)
        {
            AddActionandEnqueue(searchObject);
            if (batchedQueue.Count > AzureSyncStrategy.batchSize)
            {
                DequeueBatch();
            }
            
        }

        /// <summary>
        /// Enqueue the database row and add the search action to the row
        /// </summary>
        /// <param name="searchObject">the row</param>
        private void AddActionandEnqueue(Dictionary<string, object> searchObject)
        {
            string op = Enum.GetName(typeof(SyncOperation), SyncOperation.upload);
            searchObject.Add("@search.action", op);
            batchedQueue.Enqueue(searchObject);
        }

        /// <summary>
        /// Dequeue a batch rows and batch send the rows, will have a safety if the batch is less than batch size
        /// </summary>
        private void DequeueBatch()
        {
            List<Dictionary<string, object>> changes = new List<Dictionary<string, object>>(batchedQueue.Count);
            uint batchCount = 0;
            while (batchCount <= AzureSyncStrategy.batchSize)
            {
                try
                {
                    var row = batchedQueue.Dequeue();
                    changes.Add(row);
                    batchCount++;
                }
                catch (InvalidOperationException)
                {
                    break;
                }                
            }

            var batch = new
            {
                value = changes
            };
            string content = JsonConvert.SerializeObject(batch, serializationSettings);
            Dictionary<string, object> latest = changes.First();

            SendWithRetries(content, latest);
            changes.Clear();
        }

        /// <summary>
        /// Dequeue and Send all changes in the queue.
        /// </summary>
        private void DequeueAll()
        {
            List<Dictionary<string, object>> changes = new List<Dictionary<string, object>>(batchedQueue.Count);
            // check for remainder rows.
            while (batchedQueue.Count > 0)
            {
                var row = batchedQueue.Dequeue();
                changes.Add(row);
            }
            var batch = new
            {
                value = changes
            };
            Dictionary<string, object> latestRemainder = changes.First();
            string content = JsonConvert.SerializeObject(batch, serializationSettings);
            //arun
          
            //string actualcontent = content.Replace(@"\", "");
            //actualcontent = actualcontent.Replace("\"[","[");
            //actualcontent = actualcontent.Replace("]\"", "]");
            SendWithRetries(content, latestRemainder);
        }

        /// <summary>
        /// Send the serialized content with tries.
        /// </summary>
        /// <param name="content">serialized content</param>
        /// <param name="latest">latest row in the beginning of the batch or sequence</param>
        private void SendWithRetries(string content, Dictionary<string, object> latest)
        {
            try
            {
                string actualcontent ;
                actualcontent = content.Replace(@"\""", "\"");
                actualcontent = actualcontent.Replace("\"[", "[");
                actualcontent = actualcontent.Replace("]\"", "]");
                Send(content, latest);
                currentRetries = 0;
            }
            catch (GlobalSearchSyncException gx)
            {
                if (currentRetries == maxRetries)
                {
                    // need to log this...
                    currentRetries = 0;                  
                    this.onError(gx);
                }
                else
                {
                    currentRetries++;
                    SendWithRetries(content, latest);
                }
            }
        }

        /// <summary>
        /// Send the data upload to storage
        /// </summary>
        /// <param name="content">serialized content</param>
        /// <param name="latestRow">first element in the batch as a reference point</param>
        private void Send(string content, Dictionary<string, object> latestRow)
        {
            HttpContent httpContent = new StringContent(content, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            string requestUri = string.Format("indexes/{0}/docs/index?{1}", AzureSyncStrategy.index, apiVersion);
            var response = serviceClient.PostAsync(requestUri, httpContent).Result;
            EnsureSuccessfulSearchResponse(response, latestRow);

            lastUpdate = DateTime.Now;
            
        }
        
        /// <summary>
        /// validate the request and rethrow if failed
        /// </summary>
        /// <param name="response">the http response message</param>
        private static void EnsureSuccessfulSearchResponse(HttpResponseMessage response, Dictionary<string, object> failedRow = null)
        {
            if (!response.IsSuccessStatusCode)
            {
                string error = response.Content == null ? null : response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(error) && error.Contains("Cannot convert"))
                {
                    throw new GlobalSearchSyncException(failedRow, "Search request failed because of a schema-data type mismatch: \r\n" + error);
                }
                else
                {
                    throw new GlobalSearchSyncException(failedRow, "Search request failed: \r\n" + error);
                }                
            }
        }

        /// <summary>
        /// Get the index schema for the specified index name
        /// </summary>
        /// <typeparam name="T">type of the schema, in this case AzureSearchSchema</typeparam>
        /// <param name="indexName">name of the index</param>
        /// <returns>the schema</returns>
        public T GetIndex<T>(string indexName)
        {
            if (string.IsNullOrWhiteSpace(indexName))
            {
                throw new ArgumentNullException("indexName");
            }
            string requestUri = string.Format("indexes/{0}?{1}", indexName, apiVersion);
            var response = serviceClient.GetAsync(requestUri).Result;
            EnsureSuccessfulSearchResponse(response);
            string content = response.Content.ReadAsStringAsync().Result;
            foreach (string name in Enum.GetNames(typeof(DataType)))
            {
                DataType dt = (DataType)Enum.Parse(typeof(DataType), name);
                if (name == "CollectionOfString")
                {
                    content = content.Replace("Collection(String)", name);
                }
                else if(name != "CollectionOfString") { content = content.Replace(dt.GetDescription(), name); }
                
            }
            return JsonConvert.DeserializeObject<T>(content);            
        }

        /// <summary>
        /// Given a index schema create the index.
        /// </summary>
        /// <typeparam name="T">AzureSearchSchema, throw otherwise</typeparam>
        /// <param name="indexSchema">the schema</param>
        public void CreateIndex<T>(T indexSchema)
        {
            var schema = indexSchema as AzureSearchSchema;
            if (SchemaExists(schema.Name))
            {
                throw new InvalidOperationException(string.Format("{0} already exists as an index.", schema.Name));
            }

            string content = JsonConvert.SerializeObject(indexSchema, serializationSettings);
            foreach (string name in Enum.GetNames(typeof(DataType)))
            {
                DataType dt = (DataType)Enum.Parse(typeof(DataType), name);
                if (name == "CollectionOfString") 
                {
                    content = content.Replace("CollectionOfEdm.String", dt.GetDescription()); 
                }
                else if (name != "CollectionOfString")
                {
                    content = content.Replace(name, dt.GetDescription());
                }
            }
            
            HttpContent httpContent = new StringContent(content, Encoding.UTF8);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            string requestUri = string.Format("indexes?{0}", apiVersion);
            var response = serviceClient.PostAsync(requestUri, httpContent).Result;
            EnsureSuccessfulSearchResponse(response);
        }

        /// <summary>
        /// Method to check whether is a Schema index exists
        /// </summary>
        /// <param name="indexName">the schema name</param>
        /// <returns>true if yes</returns>
        public bool SchemaExists(string indexName)
        {
            if (string.IsNullOrWhiteSpace(indexName))
            {
                throw new ArgumentNullException("indexName");
            }
            string requestUri = string.Format("indexes/{0}?{1}", indexName, apiVersion);
            var response = serviceClient.GetAsync(requestUri).Result;
            try
            {
                EnsureSuccessfulSearchResponse(response);
                return true;
            }
            catch (GlobalSearchSyncException)
            {
                return false;
            }
        }

        /// <summary>
        /// On Error event
        /// </summary>
        /// <param name="ex">exception received from the sequence</param>
        public void OnError(Exception ex)
        {
            GlobalSearchSyncException gx = new GlobalSearchSyncException(ex.Message, ex);
            this.onError(gx);
        }

        /// <summary>
        /// on completed event, check for remainder items before we complete the sequence.
        /// </summary>
        public void OnCompleted()
        {
            // send any remaining data elements in the queue
            if (batchedQueue.Count > 0 && batchedQueue.Count < AzureSyncStrategy.batchSize)
            {
                DequeueAll();
            }
            this.onCompleted();
        }

        /// <summary>
        /// Delete an index and all documents in the index.
        /// </summary>
        /// <param name="indexName">the index name</param>
        public void DeleteIndex(string indexName)
        {
            if (string.IsNullOrWhiteSpace(indexName))
            {
                throw new ArgumentNullException(indexName);
            }
            string requestUri = string.Format("indexes/{0}?{1}", indexName, apiVersion);
            var response = serviceClient.DeleteAsync(requestUri).Result;
            EnsureSuccessfulSearchResponse(response);
        }
    }
}
