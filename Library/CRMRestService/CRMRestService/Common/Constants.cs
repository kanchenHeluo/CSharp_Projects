using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRMRestService.Common
{
    public class Constants
    {
        public const int CRMCreate = 0;
        public const int CRMUpdate = 1;

        public const int CRMFailed = 0;
        public const int CRMSucceed = 1;

        public const string PropertyName_Action = "Action";
        public const string PropertyName_StateCode = "StateCode";
        public const string PropertyName_StatusCode = "StatusCode";
        public const string PropertyName_OptionSetValue = "Value";
        public const string PropertyName_Company = "ParentCustomerId";

        public const string StateCode_Inactive = "1";
    }
}