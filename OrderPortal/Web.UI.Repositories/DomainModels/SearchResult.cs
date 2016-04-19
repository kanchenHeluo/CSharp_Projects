using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.DomainModels
{
    public class SearchResult<T>
    {
        public List<T> Results { get; set; }
        public int TotalCount { get; set; }
    }
}
