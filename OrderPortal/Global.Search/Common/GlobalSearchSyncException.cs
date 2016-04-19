using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global.Search.Common
{
    [Serializable]
    public class GlobalSearchSyncException : Exception
    {
        /// <summary>
        /// the failed row in the sequence
        /// </summary>
        public Dictionary<string, object> failedRow { get; private set; }
        public GlobalSearchSyncException(Dictionary<string, object> failedRow, string message)
            : base(message)
        {
            this.failedRow = failedRow;
        }

        public GlobalSearchSyncException(string message, Exception inner)
            : base(message, inner)
        {

        }
    }
}
