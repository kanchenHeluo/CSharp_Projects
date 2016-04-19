
namespace Demo.Contracts
{
    using System.ComponentModel.DataAnnotations;
    public class PurchaseOrderLineItem
    {
        [Required]
        public long PoId { get; set; }

        public long ItemId { get; set; }

        public string ItemName { get; set; }
    }
}