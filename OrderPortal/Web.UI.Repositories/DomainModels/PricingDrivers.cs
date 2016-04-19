using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;//Included for "DataContract" attribute

namespace Web.UI.Repositories.DomainModels
{
    
    public class PricingDrivers
    {
        /// <summary>
        /// unit price
        /// </summary>
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// pricing country
        /// </summary>
        public string PricingCountryCode { get; set; }

        /// <summary>
        /// pricing currency
        /// </summary>
        public string PricingCurrencyCode { get; set; }

        /// <summary>
        /// program - no need any more
        /// </summary>
        //public string ProgramCode { get; set; }
        /// <summary>
        /// program offering
        /// </summary>
        public string ProgramOfferingCode { get; set; }

        /// <summary>
        /// customer type
        /// </summary>
        public string CustomerTypeCode { get; set; }

        /// <summary>
        /// license agreement type
        /// </summary>
        public string LicenseAgreementTypeCode { get; set; }

        /// <summary>
        /// coverage date
        /// </summary>
        public DateTime PricingDate { get; set; }

        /// <summary>
        /// Identifies the duration that goes with PurchaseUnitQuanitty for pricing (applies when IsCoterminus = F)
        /// </summary>
        public string PurchaseUnitTypeCode { get; set; }

        /// <summary>
        /// Unit quantity of the current item purchase
        /// </summary>
        public int? PurchaseUnitQuantity { get; set; }

        /// <summary>
        /// PurchaseUnit
        /// </summary>
        public string PurchaseUnitCode { get; set; }

        /// <summary>
        /// PurchasePeriod
        /// </summary>
        public string PurchasePeriodCode { get; set; }


     
        
    }
}
