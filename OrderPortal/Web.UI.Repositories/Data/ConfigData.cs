using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Common.Extensions;

namespace Web.UI.Repositories.Data
{
    public static class ConfigData
    {

      
        public static string RenewalIndex { get; set; }
        public static string GetRenewals { get; set; }
        public static string StepUpIndex { get; set; }
        public static string AgreementIndex { get; set; }
        public static string AgreementDetails { get; set; }
        public static string SetupSearch { get; set; }
        public static string SpecialPricingIndex { get; set; }
        public static string ItemIndex { get; set; }
        public static string AvailableAppPoolIndex { get; set; }
        public static string LicensePoolIndex { get; set; }
        public static string OfferingIndex { get; set; }
        public static string  OrgGuid { get; set; }
        public static string ApplicationId { get; set; }
        public static string AgreApplicationId { get; set; }
        public static int PageIndex{get;set;}
        public static bool IsDebug { get; set; }
        public static string AgreRequestId { get; set; }
        public static string OrderRequestId { get; set; }
        public static string AgreementFilter{ get; set; }
        public static string AgreementDetailsFilter { get; set; }
        public static string AgreementInDetailFilter { get; set; }
        public static string OrderFilter { get; set; }
        /// <summary>
        /// If NoSQLBridgeSwitch is true Then Data will be from Azure else Elastic Engine
        /// </summary>
        public static bool NoSQLBridgeSwitch { get; set; }
        /// <summary>
        /// Products Index Name Under Product Catalog Collection(Index name is Item)
        /// </summary>
         public static string ProductsIndex{get;set;}
        /// <summary>
        /// 
        /// </summary>
         public static string ProductsFilter { get; set; }

         public static string SearchFields { get; set; }
         public static string AgreementSearchConfig { get; set; }

        

        static ConfigData()
        {
            #region AzureIndexs

            IsDebug = ConfigurationManager.AppSettings.GetValue<bool>("IsDebug", false);
            RenewalIndex = ConfigurationManager.AppSettings.GetValue<string>("RenewalIndex", "renewals");
            StepUpIndex = ConfigurationManager.AppSettings.GetValue<string>("StepUpIndex", "stepupcatalog");
            AgreementIndex = ConfigurationManager.AppSettings.GetValue<string>("AgreementIndex", "agreementreference");
           // AgreementDetails = ConfigurationManager.AppSettings.GetValue<string>("AgreementDetails", "agreementdetails");
            AgreementDetails = ConfigurationManager.AppSettings.GetValue<string>("AgreementDetails", "agreementdetails");
            SpecialPricingIndex = ConfigurationManager.AppSettings.GetValue<string>("SpecialPricingIndex", "specialpricing");
            ItemIndex = ConfigurationManager.AppSettings.GetValue<string>("ItemIndex", "item");
            AvailableAppPoolIndex = ConfigurationManager.AppSettings.GetValue<string>("AvailableAppPoolIndex", "availableinpool");
            LicensePoolIndex = ConfigurationManager.AppSettings.GetValue<string>("LicensePoolIndex", "licensepool");
            OfferingIndex = ConfigurationManager.AppSettings.GetValue<string>("OfferingIndex", "vreference");
            SetupSearch = ConfigurationManager.AppSettings.GetValue<string>("SetupSearch", "stepupopportunities");
            GetRenewals = ConfigurationManager.AppSettings.GetValue<string>("GetRenewals", "renewals");
            ProductsIndex = ConfigurationManager.AppSettings.GetValue<string>("ProductsIndex", "products");
            AgreementSearchConfig = ConfigurationManager.AppSettings.GetValue<string>("AgreementSearchConfig", "EDB114F3C804F6D788F7B077B3435A81;agreements;newagreements");
            #endregion

            #region Filters
            AgreementFilter = ConfigurationManager.AppSettings.GetValue<string>("AgreementFilter", "$filter=AgreementNumber eq '{0}' and BillToPCN eq '{1}' and CustomerName eq '{2}'");
            AgreementDetailsFilter = ConfigurationManager.AppSettings.GetValue<string>("AgreementDetailsFilter", "search='{0}'&searchFields=AgreementNumber&$top=15");
            AgreementInDetailFilter = ConfigurationManager.AppSettings.GetValue<string>("AgreementInDetailFilter", "$filter= ( AgreementNumber eq '{0}')");
            OrderFilter=ConfigurationManager.AppSettings.GetValue<string>("OrderFilter","$filter=AgreementNumber eq '{0}' and EndCustomerNumber eq '{1}' and POAgreementNumber eq '{2}'");
            ProductsFilter = ConfigurationManager.AppSettings.GetValue<string>("ProductsFilter", "$filter=");
            SearchFields = ConfigurationManager.AppSettings.GetValue<string>("SearchFields", "search=");
            #endregion

            #region Parameters
            OrgGuid = ConfigurationManager.AppSettings.GetValue<string>("OrgGuid", "E0A818EE-4BC7-4CE7-96EF-3197CC0D56D7");
            ApplicationId = ConfigurationManager.AppSettings.GetValue<string>("ApplicationId", "49ffbd0e-707a-4b3c-a22b-6dfe9190986a");
            AgreApplicationId = ConfigurationManager.AppSettings.GetValue<string>("AgreApplicationId", "ValidApplicationId");
            AgreRequestId = ConfigurationManager.AppSettings.GetValue<string>("AgreRequestId", "ValidRequestId");
            OrderRequestId = ConfigurationManager.AppSettings.GetValue<string>("OrderRequestId", "ValidRequestId");
            NoSQLBridgeSwitch = ConfigurationManager.AppSettings.GetValue<bool>("NoSQLBridgeSwitch", true);
            #endregion
        }

    }
}
