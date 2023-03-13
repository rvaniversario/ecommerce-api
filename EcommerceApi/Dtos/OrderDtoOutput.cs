using EcommerceApi.Entities;
using EcommerceApi.Enums;

namespace EcommerceApi.Dtos
{
    public class OrderDtoOutput : Base
    {
        public Guid UserId { get; set; }
        public double OrderPrice { get; set; }
        public Status Status { get; set; }
        public List<CartItem>? CartItems { get; set; }
    }
}
