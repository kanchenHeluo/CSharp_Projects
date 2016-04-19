using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Web.UI.Repositories.DomainModels
{
   
    public class LevelPosition 
    {
        public string Pool { get; set; }

        public string PoolName { get; set; }

        public int CurrentPoints { get; set; }

        public int AccruedPoints { get; set; }

        public string CurrentOfferingLevel { get; set; }

        public string NewOfferingLevel { get; set; }

    }
}
