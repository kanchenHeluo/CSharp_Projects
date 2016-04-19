using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.UI.Common;

namespace Web.UI.Security
{
    public interface ICapabilities
    {
        string GetFeatures();
        bool IsValidFeature(UIConstants.OrderRoles role);
    }
}