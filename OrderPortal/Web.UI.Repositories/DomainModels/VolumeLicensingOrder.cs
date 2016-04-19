using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.UI.Repositories.Models;

namespace Web.UI.Repositories.DomainModels
{
    public class VolumeLicensingOrder
    {

        public VolumeLicensingOrder()
        {
            Header = new OrderHeader();
        }
        public Shipment Shipment { get; set; }

        private Collection<IssueInfo> issues;
        private Collection<LevelPosition> levelPosition;

        public OrderHeader Header { get; set; }
        public List<OrderLineItem> LineItems { get; set; }

        /// <summary>
        /// Agreement level position
        /// </summary>
        public Collection<LevelPosition> LevelPositions
        {
            get
            {
                return levelPosition;
            }
            set
            {
                levelPosition = value;
            }
        }

        /// <summary>
        /// Additional Transactional attributes that can impact the price/Price process can be added in this collection
        /// </summary>
      
        public Collection<ExtendedProperty> ExtendedProperties { get; set; }
       
     
        /// <summary>
        /// Collection of Items that need to be priced
        /// </summary>
        public Collection<IssueInfo> Issues
        {
            get
            {
                return issues;
            }
            set
            {
                issues = value;
            }

        }
    }
}
