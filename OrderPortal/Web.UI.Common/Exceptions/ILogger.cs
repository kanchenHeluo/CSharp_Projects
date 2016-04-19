using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Common.Logging
{
    public interface ILogger
    {
        void Write(string message, params object[] args);
        void Write(string message, LogMessageType type, params object[] args);
        IEnumerable<string> Get(bool format = false);
    }
}