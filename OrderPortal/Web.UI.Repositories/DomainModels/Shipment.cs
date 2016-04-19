using System;

namespace Web.UI.Repositories.DomainModels
{
    public class Shipment
    {
        public long? PurchaseOrderShipToId { get; set; } 
        public string ContactFirstName { get; set; } 
        public string ContactLastName { get; set; } 
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; } 
        public string OrganizationName { get; set; } 
        public string PostalCode { get; set; } 
        public string City { get; set; } 
        public string StateProvince { get; set; } 
        public string CountryCode { get; set; }
        public string ContactPhoneNumber { get; set; } 
        public string ContactEmailAddress { get; set; } 
        public string ContactFaxNumber { get; set; }
        public string AddressLine3 { get; set; } 
        public string AddressLine4 { get; set; } 
        public string CorrespondenceLanguageCode { get; set; } 

        public string CreatedByUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LastModifiedUser { get; set; }
        public string LicenseProgramCode { get; set; }
        public int AgreementID { get; set; }
        public string AgreementNumber { get; set; }
        public string ShipToPartnerNumber { get; set; }
        public string EndCustomName { get; set; }
        public DateTime? LastValidatedDate { get; set; }
        public string ShipToStatus { get; set; }
    }
}