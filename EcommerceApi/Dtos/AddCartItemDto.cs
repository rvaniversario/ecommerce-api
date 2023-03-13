namespace EcommerceApi.Dtos
{
    public class AddCartItemDto
    {
        public string? ProductName { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}
