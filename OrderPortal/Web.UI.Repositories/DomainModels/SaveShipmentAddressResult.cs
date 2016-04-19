using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.DomainModels
{
    public class SaveShipmentAddressResult
    {
        public long ShipToId { get; set; }

        public List<string> Errors { get; set; }
    }
}
