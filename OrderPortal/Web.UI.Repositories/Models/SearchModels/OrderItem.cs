using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.Models
{
    public class OrderItem
    {
        public string ItemID { get; set; }
        public string PartNumber { get; set; }
        public string ItemName { get; set; }
        public string ItemLegalName { get; set; }
        public string MaxcimPartDescription { get; set; }
        public string MacPacPartDescription { get; set; }
        public string ItemTypeCode { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemStatusCode { get; set; }
        public string ItemStatusName { get; set; }
        public string PoolCode { get; set; }
        public string PoolName { get; set; }
        public string ProductFamilyCode { get; set; }
        public string ProductFamilyName { get; set; }
        public string OperatingSystemCode { get; set; }
        public string OperatingSystemName { get; set; }
        public string HardwareInterfaceCode { get; set; }
        public string HardwareInterfaceName { get; set; }
        public string Version { get; set; }
        public string VersionSequence { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }
        public string MarketPlaceCode { get; set; }
        public string MarketPlaceName { get; set; }
        public string ProductTypeCode { get; set; }
        public string ProductTypeName { get; set; }
        public string PackageTypeCode { get; set; }
        public string PackageTypeName { get; set; }
        public string LicenseTypeCode { get; set; }
        public string LicenseTypeName { get; set; }
        public string LicenseCount { get; set; }
        public string ClientLimitCount { get; set; }
        public string UpgradeFromCode { get; set; }
        public string UpgradeFromName { get; set; }
        public string SellingConstraintCode { get; set; }
        public string SellingConstraintName { get; set; }
        public string ReplacesPartNumber { get; set; }
        public string ReplacedByPartNumber { get; set; }
        public string RecurringMaintPartNumber { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public string LeadPricingCountryCode { get; set; }
        public string PriceDifferentiatorCode { get; set; }
        public string PriceDifferentiatorName { get; set; }
        public string SecondaryLicenseTypeCode { get; set; }
        public string SecondaryLicenseTypeName { get; set; }
        public string NameDifferentiatorCode { get; set; }
        public string NameDifferentiatorName { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string PricingLevelCode { get; set; }
        public string PricingLevelName { get; set; }
        public string ProgramOfferingCode { get; set; }
        public string AvailableInPoolID { get; set; }
    }

    //    public class Itemoffering
    //    {
    //        public string ItemOfferingID { get; set; }
    //        public string ProgramCode { get; set; }
    //        public string ProgramOfferingCode { get; set; }
    //        public string ItemId { get; set; }
    //        public string PurchasePeriodCode { get; set; }
    //        public string UnitCount { get; set; }
    //        public string ChangeControl { get; set; }
    //        public string CreatedByUser { get; set; }
    //        public string CreatedDate { get; set; }
    //        public string LastModifiedDate { get; set; }
    //        public string LastModifiedBy { get; set; }
    //    }

    //    public class Availablepool
    //    {
    //        public string AvailableInPoolID { get; set; }
    //        public string LicensePoolID { get; set; }
    //        public string ItemID { get; set; }
    //        public string StartEffectiveDate { get; set; }
    //        public string EndEffectiveDate { get; set; }
    //    }
    //}
}