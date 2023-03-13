using EcommerceApi.Enums;

namespace EcommerceApi.Entities
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; set; }
        public double OrderPrice { get; set; }
        public Status Status { get; set; } = Status.Pending;
        public List<CartItem>? CartItems { get; set; }
    }
}
