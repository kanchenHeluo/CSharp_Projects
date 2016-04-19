using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.UI.Repositories.Models
{
    public class OderableItem
    {

        public string ColId{ get; set; }
        public string ItemID{ get; set; }
        public string PartNumber{ get; set; }
        public string ItemName{ get; set; }
        public string ItemLegalName{ get; set; }
        public string ItemDescription{ get; set; }
        public string ProductTypeCode{ get; set; }
        public string ProductTypeName{ get; set; }
        public string ProductFamilyCode{ get; set; }
        public string ProductFamilyName{ get; set; }
        public string PoolCode{ get; set; }
        public string PoolName{ get; set; }
        public string LineitemType{ get; set; }
        public string PurchaseUnitType{ get; set; }
        public string PurchaseUnitQuantity{ get; set; }
        public string startdate{ get; set; }
        public string EndDate{ get; set; }
        public string ParentReference{ get; set; }
        public string ParentPartNumber{ get; set; }
        public string OriginalQuantity{ get; set; }
        public string Remainingquantity{ get; set; }
        public string OriginalStartDate{ get; set; }
        public string OriginalEndDate{ get; set; }
        public string AssociatedPurchaseOrder{ get; set; }
        public string AssociatedPurchaseaccount{ get; set; }
        public string AgreementId{ get; set; }
        public string AgreementNumber{ get; set; }
        public string EndCustomerNumber{ get; set; }
        public string PurchasingAccountNumber{ get; set; }
        public string AdvisorNumber{ get; set; }
        public string DerivedItemType{ get; set; }
        public string UnitPrice{ get; set; }
        public string IsAutoReNew { get; set; }
    }
}
