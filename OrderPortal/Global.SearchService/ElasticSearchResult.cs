using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Global.SearchService
{
    [DataContract]
    public sealed class ElasticSearchResult<T>
    {
        [DataMember(Name = "value")]
        public IEnumerable<T> SearchResults { get; set; }
    }
}
