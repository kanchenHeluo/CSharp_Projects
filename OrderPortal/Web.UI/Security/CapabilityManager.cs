using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.UI.Common;
using System.Configuration;

namespace Web.UI.Security
{
    public class CapabilityManager : ICapabilities
    {
        public string GetFeatures()
        {
            return MyPrincipal.Current.GetRoles;          
        }

       

        public bool IsValidFeature(Common.UIConstants.OrderRoles role)
        {
            string featuresSet  = GetFeatures();
            if (string.IsNullOrEmpty(featuresSet))
           {
               return false;
           }
            return featuresSet.Contains(role.ToString());
        }

    }
}