using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Search.Strategies
{
    public class RedisStrategy : ISearchStrategy
    {
        public T Search<T>(string query, string indexname)
        {
            throw new NotImplementedException();
        }

        public T Suggest<T>(string searchText, string indexName, string[] selectedColumns = null, string ODataFilter = null)
        {
            throw new NotImplementedException();
        }
    }
}
