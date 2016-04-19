using MemoContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoApiClient
{
    public class MemoClient : ClientBase
    {
        public async Task<IEnumerable<MemoItem>> GetAsync()
        {
            return await GetJsonAsync<IEnumerable<MemoItem>>(Config.MemoApiUri);
        }

        public IEnumerable<MemoItem> Get()
        {
            return GetJson<IEnumerable<MemoItem>>(Config.MemoApiUri);
        }
    }
}
