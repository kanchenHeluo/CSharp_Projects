using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Web.UI.Repositories.Models
{
    public class LicensePool
    {
       
            public string ColId { get; set; }
            public string LicensePoolID { get; set; }
            public string IncludeFromFolderID { get; set; }
            public string LicensePoolCode { get; set; }
            public string LicensePoolName { get; set; }
            public string CDSetName { get; set; }
            public string RollUpParentPoolID { get; set; }
            public string CreatedDate { get; set; }
            public string CreatedByUser { get; set; }
            public string LastModifiedDate { get; set; }
            public string ModifiedByUser { get; set; }
            public string LocalizedPhraseGUID { get; set; }

     
    }
}
