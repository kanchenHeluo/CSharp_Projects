using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;//Included for "DataContract" attribute

namespace Web.UI.Repositories.DomainModels
{
    
    public class PriceAtNewLevel
    {
        /// <summary>
        /// OfferingLevel after taking into account the quote
        /// </summary>

        public string OfferingLevel { get; set; }

        /// <summary>
        /// Price of the item before tax per customer commitments
        /// </summary>

        public decimal? ListPrice { get; set; }
        
        /// <summary>
        /// Approved system price of the item before tax
        /// </summary>

        public decimal? SystemPrice { get; set; }

        /// <summary>
        /// Price for the given quantity
        /// </summary>

        public decimal? NetPrice { get; set; }

        public int Points { get; set; }

        public int? PurchaseUnitQuantity { get; set; }

    }
}
