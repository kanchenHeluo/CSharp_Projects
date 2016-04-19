using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Web.UI.Repositories.DomainModels
{
    [DataContract(Namespace = "http://MS.IT.Ops.MSLicense.OrderService.DataContract.Entity.Response/", Name = "LevelSetting")]
    public class LevelSetting 
    {
        public string Pool { get; set; }

        public string PoolName { get; set; }


        public string OfferingLevel { get; set; }

        public int Min { get; set; }

        public int? Max { get; set; }
    }
}
