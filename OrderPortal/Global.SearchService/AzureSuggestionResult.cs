using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Global.SearchService
{
    [DataContract]
    public sealed class AzureSuggestionResult<T>
    {
        [DataMember(Name = "@odata.context")]
        public string ODataContext { get; set; }

        [DataMember(Name="value")]
        public IEnumerable<T> Suggestions { get; set; }
    }
}
