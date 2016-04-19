using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Web.UI.Repositories.Models
{
    public class OrderLineItemModel
    {

        public OrderLineItemModel()
        {
          
        }

        public bool AgreementHasComplexDealFlag { get; set; }       
        //public Collection<Microsoft.IT.OrderCenter.UI.ServiceGateway.OrderProxy.VLItemInfo> AssociatedItemDetails { get; set; }       
        public Collection<string> AssocicatedProgramOfferingCodes { get; set; }       
        //public Microsoft.IT.OrderCenter.UI.ServiceGateway.OrderProxy.Audit Audit { get; set; }       
        public bool IsSelected { get; set; }
        public int AvailableQuantity { get; set; }       
   
        public int BaseMappingID { get; set; }       

        public int BillingMultipler { get; set; }       

        public DomainItem BillingOption { get; set; }       
        public decimal? BillingPrice { get; set; }    
        public decimal SubmittedPrice { get; set; }
        //public Microsoft.IT.OrderCenter.UI.ServiceGateway.OrderProxy.CALTypeId CALTypeId { get; set; }       

        
        public byte[] CheckSum { get; set; }       
        //public Collection<Microsoft.IT.OrderCenter.UI.ServiceGateway.OrderProxy.Comment> Comments { get; set; }       

        public DateTime? CoverageEndDate { get; set; }       
 
        public int CoverageLength { get; set; }       

        public DateTime? CoverageStartDate { get; set; }       

        public int? CoverageTerm { get; set; }       
        public decimal? ExtendedPrice { get; set; }       

        public bool HasBaseQuickStartReservation { get; set; }       

        public int HasMissingPriceIssue { get; set; }       

        public bool HasMultipleOfferings { get; set; }       

        public int? Id { get; set; }       

        public bool IsBasePOLIReconciledFlag { get; set; }       

        public bool IsComplexDeal { get; set; }       

        public bool IsDisplaySuite { get; set; }       

        public bool IsExceptionTransition { get; set; }       
 
        public bool IsFirstAnnualOrder { get; set; }       

        public bool IsOlsItem { get; set; }       

        public bool IsPriced { get; set; }       

        public bool? IsSourcedByQuickStart { get; set; }       
 
        public bool? IsSpecialPriced { get; set; }       

        public bool IsUsageDateBackDateable { get; set; }       

        public bool IsUsageDateEditable { get; set; }

        
        public string IsFulfillmentProductFlag { get; set; }    // eMSL SKUEntity

        
        public string ImmediateShipFlag { get; set; }   // eMSL SKUEntity

        
        public bool IsAZS { get; set; } // eMSL

        
        public bool IsCoterminus { get; set; }

        //public Collection<Microsoft.IT.OrderCenter.UI.ServiceGateway.OrderProxy.Issue> Issues { get; set; }       
        //public Microsoft.IT.OrderCenter.UI.ServiceGateway.OrderProxy.VLItemInfo ItemDetails { get; set; }       

        public int ItemUnitCount { get; set; }       

        public Guid? LineItemGuid { get; set; }       

        public int LineItemHistorySeqNumber { get; set; }       

        public string LineItemNumber { get; set; }       

        public int LineItemId { get; set; }

        public string LineItemType { get; set; }       

        public decimal? ListPrice { get; set; }       

        public string MappedIsSASKU { get; set; }       

        public bool? MultipleTransitionAssociatedFlag { get; set; }       

        public int? OpportunityId { get; set; }       

        public string OpportunityTypeCode { get; set; }       

        public int OrderQuantity { get; set; }       
        //public Microsoft.IT.OrderCenter.UI.ServiceGateway.OrderProxy.PFAMIdentity PFAMIdentity { get; set; }       

        public DateTime? POLIUsageDateTime { get; set; }       

        public int? POLineItemId { get; set; }       

        public DateTime POLineItemStatusDate { get; set; }       

        public decimal PoliInitialQuantityOrdered { get; set; }       

        public int PricePathID { get; set; }       
            

        public DateTime PricingPeriodDate { get; set; }       
        public string ProgramOfferingCode { get; set; }       
        public int PurchaseOrderId { get; set; }       

        public string PurchaseUnitQuantity { get; set; }
        public string PurchaseOrderNumber { get; set; }       
        public DomainItem PurchaseOrderType { get; set; }       
        public string PurchasePeriodCode { get; set; }       
        public DomainItem PurchaseUnit { get; set; }       
        public DomainItem PurchaseUnitType { get; set; }       
        public int QuantityFrom { get; set; }      


        public int QuantityTo { get; set; }       

        public DateTime? QuickStartComplianceEndDate { get; set; }       

        public Collection<int> ReferencePOLineItemIds { get; set; }       

        public DateTime? ReservationDate { get; set; }       

        public int? ReservationPOLineItemId { get; set; }       
         

        public int RoliID { get; set; }

        public string SpecialDealNumber { get; set; }       

        public decimal? SystemPrice { get; set; }       

        public string TargetType { get; set; }       

        public string UnitOfMeasure { get; set; }       

        public decimal? UnitPrice { get; set; }       
        public DomainItem UsageCountry { get; set; }       
        public Dictionary<string, bool> AssociatedProgramOfferingFlags { get; set; }
        
        public string ItemNumber { get; set; }

        
        public string Description { get; set; }

        
        public string ItemName { get; set; }
        public DomainItem ProductType { get; set; }
        public string PartNumber { get; set; }

        
        public string ProductFamilyName { get; set; }

        
        public string ProductFamilyCode { get; set; }

        
        public string TransactionType { get; set; }

        
        public string RmaNumber { get; set; }

        
        public string EndCustomPoNumber { get; set; }  

        
        public string EstimatedPrice { get; set; }  

        
        public string ReBillPurchaseOrderNumber { get; set; }  

        
        public string PurchaseOption { get; set; }

        
        public string ExtendAmount { get; set; }

        
        public string CorrelationId { get; set; }  

        
        public string Status { get; set; }
        
        public string PurposeCode { get; set; }
        
    }
}