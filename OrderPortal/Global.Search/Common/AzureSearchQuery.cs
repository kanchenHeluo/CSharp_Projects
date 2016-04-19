using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Global.Search.Common
{
    public class AzureSearchQuery
    {

        private static readonly Lazy<AzureSearchQuery> lazy =
        new Lazy<AzureSearchQuery>(() => new AzureSearchQuery());
    
      public static AzureSearchQuery Instance { get { return new AzureSearchQuery(); } }

  
        List<Tuple<string, string>> QueryParameters;
        public AzureSearchQuery()
        {
            QueryParameters = new List<Tuple<string, string>>();
        }
        public AzureSearchQuery WithQuery(string query)
        {
            this.Query = query;
            return this;
        
        }

        public AzureSearchQuery WithFilter(string filter)
        {
            this.Filter = filter;
            return this;

        }
        public AzureSearchQuery WithSearchMode(SearchMode searchMode)
        {
            this.Mode = searchMode;
            return this;

        }

        public AzureSearchQuery WithSearchFields(string searchFields)
        {
            this.SearchFields = searchFields;
            return this;

        }

        public AzureSearchQuery Skip(int skip)
        {
            this.SkipCount = skip;
            return this;

        }
        public AzureSearchQuery Take(int take)
        {
            this.Top = take;
            return this;

        }
        public AzureSearchQuery WithCount(bool count)
        {
            this.Count = count;
            return this;

        }
        public AzureSearchQuery(string query)
        {
            Query = query;
            QueryParameters = new List<Tuple<string, string>>();
        }      

      
        [JsonProperty("query")]
        string Query
        {
            get;
            set;
        }

        [JsonProperty("mode")]
         SearchMode? Mode
        {
            get;
            set;
        }

        [JsonProperty("searchFields")]
         string SearchFields
        {
            get;
            set;
        }

        [JsonProperty("skip")]
         int SkipCount
        {
            get;
            set;
        }

        [JsonProperty("top")]
         int Top
        {
            get;
            set;
        }

        [JsonProperty("count")]
         bool Count
        {
            get;
            set;
        }

        [JsonProperty("orderBy")]
         string OrderBy
        {
            get;
            set;
        }

        [JsonProperty("select")]
         string Select
        {
            get;
            set;
        }

        [JsonProperty("facet")]
         IEnumerable<string> Facets
        {
            get;
            set;
        }

        [JsonProperty("filter")]
         string Filter
        {
            get;
            set;
        }

        [JsonProperty("highlight")]
         string Highlight
        {
            get;
            set;
        }

        [JsonProperty("scoringProfile")]
         string ScoringProfile
        {
            get;
            set;
        }

        [JsonProperty("scoringParameter")]
         IEnumerable<string> ScoringParameters
        {
            get;
            set;
        }

       
        public void AddQueryParameter(string key, string value)
        {
            QueryParameters.Add(new Tuple<string, string>(key, value));
        }
        
        public string BuildQuery()
        {
            string query = string.Empty;
            if (!String.IsNullOrEmpty(Query))
                AddQueryParameter("search", Query);
            if (Mode.HasValue)
                AddQueryParameter("searchMode", Mode.ToString().ToLower());
            if (!String.IsNullOrEmpty(SearchFields))
                AddQueryParameter("searchFields", SearchFields);
            if (SkipCount > 0)
                AddQueryParameter("$skip", SkipCount.ToString(CultureInfo.InvariantCulture));
            if (Top > 0)
                AddQueryParameter("$top", Top.ToString(CultureInfo.InvariantCulture));
            if (Count)
                AddQueryParameter("$count", Count.ToString().ToLower());
            if (!String.IsNullOrEmpty(OrderBy))
                AddQueryParameter("$orderby", OrderBy);
            if (!String.IsNullOrEmpty(Select))
                AddQueryParameter("$select", Select);            
            if (!String.IsNullOrEmpty(Filter))
                AddQueryParameter("$filter", Filter);
            
            if(QueryParameters.Any())
            {
               var parameters= QueryParameters.Select(p =>
                String.Format("{0}={1}", p.Item1, p.Item2));
                query = String.Join("&", parameters);
            }
            return query;
            
        }
    }
    public enum SearchMode
    {
        Any,
        All
    }
}
