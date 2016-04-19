using Global.Search;
using Global.SearchSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.SearchService
{
    public interface ISearchSyncProvider : ISearchProvider, ISyncPublisher
    {
    }
}
