using EcommerceApi.Entities;
using EcommerceApi.Enums;

namespace EcommerceApi.Dtos
{
    public class CheckoutDtoOutput : Base
    {
        public Guid UserId { get; set; }
        public double OrderPrice { get; set; }
        public Status Status { get; set; }
    }
}
