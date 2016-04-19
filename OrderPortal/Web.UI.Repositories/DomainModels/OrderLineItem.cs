using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;//Included for "DataContract" attribute
using System.Collections.ObjectModel; //Included for Collection<T>
using Web.UI.ServiceGateway.DraftOrderServiceProxy;

namespace Web.UI.Repositories.DomainModels
{
    #region Class
    /// <summary>
    /// Represents a LineItem that is either initiated from a quote or order
    /// </summary>
   
    public class OrderLineItem 
    {
        #region Private Declaration
        private Guid? lineItemGuid;
        private Collection<ExtendedProperty> extendedProperties;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor, initializes extendedProperties Collection Property
        /// </summary>
        public OrderLineItem()
        {
            extendedProperties = new Collection<ExtendedProperty>();
        }
        #endregion

        
        #region Public Properties
        /// <summary>
        /// Unique Identifier of the LineItem
        /// </summary>
        
        public Guid? LineItemGuid
        {
            get
            {
                return lineItemGuid;
            }
            set
            {

                lineItemGuid = value;
            }
        }

        /// <summary>
        /// Unique alphanumerical value of a SKU (item)
        /// </summary>
        
        public string PartNumber { get; set; }

        
        public string ItemName { get; set; }


        public int ItemId { get; set; }
        /// <summary>
        /// Billing Option selected for the current item purchase
        /// </summary>
        
        public string BillingOption { get; set; }

        /// <summary>
        /// Order Quantity
        /// </summary>
        
        public int QuantityOrdered { get; set; }
        public decimal ExtendedAmount { get; set; } 

        /// <summary>
        /// 
        /// </summary>
       
        public Boolean IsPriced { get; set; }

        
        public PricingDrivers PricingDrivers { get; set; }

        /// <summary>
        /// Pool at the new level
        /// </summary>
        
        public string PoolCode { get; set; }

        /* removed as already in PricingDrivers
         /// <summary>
        /// Number of points accumulated at the new level
        /// </summary>
        
        public int Points { get; set; }
        
        /// <summary>
        /// OfferingLevel before taking into account the quote
        /// </summary>
        
        public string OfferingLevel { get; set; }

        /// <summary>
        /// Price of the item before tax per customer commitments
        /// </summary>
        
        public float? ListPrice { get; set; }

        /// <summary>
        /// Approved system price of the item before tax
        /// </summary>
        
        public float? SystemPrice { get; set; }

        /// <summary>
        /// Price for the given quantity
        /// </summary>
        
        public float? NetPrice { get; set; }
         */
        public decimal UnitPrice { get; set; }
        public string ProgramOfferingCode { get; set; }
        public string PurchaseUnitTypeCode { get; set; }
        public string PurchaseUnitQuantity { get; set; }

        public PriceAtNewLevel PriceAtNewLevel { get; set; }

       
        public int? CoverageTerm { get; set; }

        public DateTime CoverageStartDate { get; set; }
        public DateTime CoverageEndDate { get; set; }

        
        public string UsageCountryCode { get; set; }

        public string CountryName { get; set; }
        /// <summary>
        /// Additional Transactional attributes that can impact the price can be added in this collection
        /// </summary>
        
        
        public Collection<ExtendedProperty> ExtendedProperties
        {
            get
            {
                return extendedProperties;
            }
            set
            {
                extendedProperties = value;
            }
        }
        #endregion

        public int OrderQuantity { get; set; } 

        public DateTime? POLIUsageDateTime { get; set; }

        public DateTime? POLineItemStatusDate { get; set; }

        public int POLineItemId { get; set; }

        public List<int> ReferencePOLIds { get; set; }

        public string LineItemType { get; set; }

        public string ProductFamilyCode { get; set; }

        public string ProductTypeCode { get; set; }

        public string ProductFamilyName { get; set; }

        public string PoolName { get; set; }

        public string CustomerReferenceNumber { get; set; }
        public string SpecialDealNumber { get; set; }

        //Add
        public bool ValidateFlag { get; set; }

        public string CreatedBy { get; set; }
        public string FlagReason { get; set; }
        public string UserComment { get; set; }

        public List<DraftOrderComment> Comments { get; set; }

        public bool IsOlsItem { get; set; }

        public bool IsStepUp { get { return (ProductTypeCode=="STP" || ProductTypeCode=="STA");} }
    }

    #endregion

}

