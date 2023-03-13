namespace EcommerceApi.Models
{
    public class CartItemModel : BaseModel
    {
        public Guid UserId { get; set; }
        public Guid OrderId { get; set; }
        public string? ProductName { get; set; }
        public double ProductPrice { get; set; }
        public double ItemPrice { get; set; }
        public int Quantity { get; set; }
    }
}
