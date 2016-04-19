using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Common
{
    public static class UIConstants
    {
        public static int RequestLogEventId =20001;
        public static int ResponseLogEventId = 20002;


        //STS namespace for first name.
        public static string ClaimFirstName = "http://sts.msft.net/user/FirstName";


        //Domian Constats
        public static string DomainCountryKey = "Countries";

        /* Session keys constant */
        public static string SessionUserContext = "UserContext";
        public static string SessionShowPartnerModal = "ShowPartnerModal";
        /* Session keys constant */

       /// <summary>
       /// Order Roles Type.
       /// </summary>
        public enum OrderRoles
        {
            SuperAdmin,
            OrderViewUser,
            OrderEditUser,
            OrderSubmitUser,
            OrderAdjustUser,
            ROEditUser,
            ROSubmitUser,
            User,
            SoliEditUser,
            SoliViewUser,
            EventProcessingUser
        }

    }
}