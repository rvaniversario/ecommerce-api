namespace EcommerceApi.Dtos
{
    public class CartItemDtoOutput : Base
    {
        public Guid OrderId { get; set; }
        public string? ProductName { get; set; }
        public double ProductPrice { get; set; }
        public double ItemPrice { get; set; }
        public int Quantity { get; set; }
    }
}
