using EcommerceApi.Entities;
using EcommerceApi.Enums;

namespace EcommerceApi.Models
{
    public class OrderModel : BaseModel
    {
        public Guid UserId { get; set; }
        public double OrderPrice { get; set; }
        public Status Status { get; set; }
        public List<CartItem>? CartItems { get; set; }
    }
}
