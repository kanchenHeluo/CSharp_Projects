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
    /// Represents the Price Header attributes and LineItem with/without price
    /// </summary>
   
    public class OrderHeader 
    {
        #region Private Declaration
        private Guid headerGuid;
        private Collection<ExtendedProperty> extendedProperties;
      

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor, initializes extendedProperties Collection Property
        /// </summary>
        public OrderHeader()
        {
            extendedProperties = new Collection<ExtendedProperty>();
          
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Unique Identifier of the Header
        /// </summary>
        public Guid HeaderGuid
        {
            get
            {
                return headerGuid;
            }
            set
            {
                //Check and raise an ArgumentNullException if the given input is null
                if (value == null)
                {
                    throw new ArgumentNullException("value", "HeaderGuid cannot be Null or Empty");
                }
                headerGuid = value;
            }
        }

        /// <summary>
        /// Date for which the request is made
        /// </summary>
        public DateTime? PurchaseOrderDate { get; set; }
        public DateTime? UsageDate { get; set; }
        public long? Id { get; set; }
        /// <summary>
        /// Identifies the Requested PriceType
        /// </summary>
    
        public HeaderType HeaderType { get; set; }

        /// <summary>
        /// AgreementId that is used in request. Ex: In case of Laminar, it is the AGM Id
        /// </summary>
        /// <value>AgreementId(int) get/sets the value in POAgreementId private variable</value>
        public int POAgreementId { get; set; }
        /// <summary>
        /// AgreementNumber that is used in request. Ex: In case of Laminar, it is the AGM number
        /// </summary>
        /// <value>AgreementNumber(string) get/sets the value in POAgreementNumber private variable</value>
        public string POAgreementNumber { get; set; }

        /// <summary>
        /// AgreementId that is used for Billing partner. Ex: In case of Laminar, it is the ARF Id
        /// </summary>
        /// <value>AgreementId(int) get/sets the value in AgreementNumber private variable</value>
        public int AgreementId { get; set; }
        /// <summary>
        /// AgreementNumber that is used  for Billing partner. Ex: In case of Laminar, it is the ARF number
        /// </summary>
        /// <value>AgreementNumber(string) get/sets the value in AgreementNumber private variable</value>
        public string AgreementNumber { get; set; }
        /// <summary>
        /// Unique alphanumeric number of Customer associated with the Order
        /// </summary>
        public string EndCustomerNumber { get; set; }

        /// <summary>
        /// Unique alphanumeric number of Customer associated with the Order
        /// </summary>
        public string DirectCustomerNumber { get; set; }

        public string IndirectCustomerNumber { get; set; }
        /// <summary>
        /// Unique alphanumeric id for the PO
        /// </summary>
        public long? DraftOrderId { get; set; }
        /// <summary>
        /// Unique alphanumeric number for the PO
        /// </summary>
        public string PurchaseOrderNumber { get; set; }
        public string SeconderyPurchaseOrderNumber { get; set; }
        /// <summary>
        /// Type of PurchaseOrder
        /// </summary>
        public string PurchaseOrderTypeCode { get; set; }

        public string PurchaseOrderStatusCode { get; set; }

        public string PricingCountryCode { get; set; }

        public string PricingCurrencyCode { get; set; }

        public decimal TotalExtendedAmount { get; set; }

        public string SalesLocationCode { get; set; }

        public string EndCustomerName { get; set; }

        public string ProgramName { get; set; }
        public string ProgramCode { get; set; }
        public List<DraftOrderComment> Comments { get; set; }

        public string OrderName { get; set; }

        public string SourceCode { get; set; }

        public string CreatedUser { get; set; }

        public string  ModifiedUser { get; set; }
      
        public bool ValidateFlag { get; set; }

        public string SourceSystem { get; set; }

        public string AssignedTo { get; set; }

        public bool LockedFlag { get; set; }

        public string LockedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public string CarrierAccountNumber { get; set; } 
        public string CarrierCode { get; set; } 
        public string Reference { get; set; } 
        public long? PurchaseOrderShipToId { get; set; }

        public Guid? CorrelationId { get; set; }
        #endregion
    }
    #endregion
}

