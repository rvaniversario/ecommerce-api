using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceApi.Entities
{
    public class CartItem : BaseEntity
    {
        [ForeignKey("Order")]
        public Guid OrderId { get; set; }
        public string? ProductName { get; set; }
        public double ProductPrice { get; set; }
        public double ItemPrice { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
