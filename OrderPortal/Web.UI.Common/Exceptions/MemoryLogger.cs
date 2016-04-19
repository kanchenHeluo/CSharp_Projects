using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Common.Logging
{
    public class MemoryLogger : ILogger
    {
        private IList<KeyValuePair<LogMessageType, string>> _logs = new List<KeyValuePair<LogMessageType, string>>();
        public void Write(string message, params object[] args)
        {
            Write(message, LogMessageType.Info, args);
        }
        public void Write(string message, LogMessageType type, params object[] args)
        {
            _logs.Add(new KeyValuePair<LogMessageType, string>(type, String.Format(message, args)));
        }

        public IEnumerable<string> Get(bool format = false)
        {
            return _logs.Select(item => format ? String.Format("{0}: {1}", item.Key, item.Value) : item.Value);
        }
    }
}