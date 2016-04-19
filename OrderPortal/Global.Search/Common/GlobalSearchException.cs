using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Search.Common
{
    [Serializable]
    public class GlobalSearchException : Exception
    {
        public GlobalSearchException(string message)
            : base(message)
        {

        }
        public GlobalSearchException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
