using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Search
{
    public interface ISearchResult<T> 
    {
          IEnumerable<T> SearchResults { get; set; }
          int Count { get; set; }

          string Content { get; set; }

    }
}
