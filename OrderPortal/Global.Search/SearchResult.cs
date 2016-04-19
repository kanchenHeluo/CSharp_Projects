using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Global.Search
{
    [DataContract]
    public  class SearchResult<T> : ISearchResult<T>
    {
      
        public IEnumerable<T> SearchResults { get; set; }
       
        public int Count { get; set; }

        public string Content { get; set; }

    }
}
