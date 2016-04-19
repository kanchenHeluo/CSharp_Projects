using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Global.Search
{
    [DataContract]
    public sealed class AzureSearchResult<T> : ISearchResult<T>
    {
        [DataMember(Name = "@odata.context")]
        public string ODataContext { get; set; }

        [DataMember(Name = "value")]
        public IEnumerable<T> SearchResults { get; set; }

        [DataMember(Name = "@odata.nextLink")]
        public string ODataNext { get; set; }


        [DataMember(Name = "@odata.count")]
        public int Count { get; set; }
        [DataMember(Name="@odata.content")]

        public string Content { get; set; }


    }
}
