using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.UI.Repositories.Models
{
    //TODO: remove this class
    public class AgreementOrderTypeRes
    {
        public string AgreementTypeCode { get; set; }
        public string AnniversaryOrdersSupportedFlag { get; set; }
        public string ElectronicPOHoldFlag { get; set; }
        public string ProposalIDSupported { get; set; }
        public string PurchaseOrderTypeCode { get; set; }
        public string PurchaseOrderTypeName { get; set; }
        public string UpgradeProtectionRenewalSupportedFlag { get; set; }
    }
}