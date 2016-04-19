using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.UI.Repositories.DomainModels;
using Web.UI.Repositories.Models;

namespace ExcelParser
{
    public class PurchaseOrderWithLineItems
    {
        public OrderHeader OrderHeader { get; set; }
        public List<OrderLineItem> LineItems { get; set; }

        public Shipment Shipment { get; set; }

        public PurchaseOrderWithLineItems()
        {
            OrderHeader = new OrderHeader();
            Shipment = new Shipment();
            LineItems = new List<OrderLineItem>();
        }
    }
}
