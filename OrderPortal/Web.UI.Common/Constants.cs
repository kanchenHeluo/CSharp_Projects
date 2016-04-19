using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Common
{
    public static class Constants
    {
        public static int RequestLogEventId = 20001;
        public static int ResponseLogEventId = 20002;
        public static string ClaimFirstName = "http://sts.msft.net/user/FirstName";
        public static string ClaimWindows = "http://sts.msft.net/user/WindowsAccountName";
        public static string ClaimPartnerImmutableID = "http://sts.msft.net/user/PartnerImmutableID";
        public static string ClaimEmail = "http://sts.msft.net/user/EmailAddress";
        public static string ClaimAlias = "http://sts.msft.net/user/Alias";
        public static string AccessorDB = "Accessor";
        public static string OrganizationDB = "Organization";
        public static string ContactPCN = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/ContactPCN";
        public static string WindowsLiveName = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/Name";
        public static string WindowsLiveAuthentication = "PUIDHex";
        public static string NTAuthentication = "NTAuth";
        public static string ClaimNamespace ="http://schemas.xmlsoap.org/ws/2005/05/identity/claims/";
        public static string CredentialType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/CredentialType";
        public static string EBACConstant = "EBAC";
        public static string EBACOrderRoles = "OrderRoles";
    }
}
