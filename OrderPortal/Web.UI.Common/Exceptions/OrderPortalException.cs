using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Common.Exceptions
{
    public class OrderPortalException : Exception
    {
        public bool Recoverable { get; set; }

        public OrderPortalException(string message, bool recoverable = true) : base(message)
        {
            Recoverable = recoverable;
        }
    }
}