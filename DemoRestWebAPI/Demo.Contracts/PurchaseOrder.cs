
namespace Demo.Contracts
{
    using System.ComponentModel.DataAnnotations;

    public class PurchaseOrder
    {
        [Required]
        public long PoId { get; set; }

        public string OrderStatus { get; set; }

    }
}