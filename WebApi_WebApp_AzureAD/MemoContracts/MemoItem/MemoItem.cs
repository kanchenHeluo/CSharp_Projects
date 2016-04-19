using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoContracts
{
    public class MemoItem
    {
        public MemoItem()
        {
            Key = Guid.NewGuid().ToString();
            Value = Key.Substring(0,2);
        }
        public MemoItem(string val)
        {
            Key = Guid.NewGuid().ToString();
            Value = val;
        }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
