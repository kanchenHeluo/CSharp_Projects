using System;

namespace Web.UI.Repositories.Models
{
    //TODO: remove this class
    public class OrderSearchResult
    {
        public Guid DummyGuid { get; set; }
        public string EndCustomerPublicNumber { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string PurchaseOrderType { get; set; }
        public string OrderType { get; set; }
        public int OrderId { get; set; }
        public string AgreementNumber { get; set; }
    }
}