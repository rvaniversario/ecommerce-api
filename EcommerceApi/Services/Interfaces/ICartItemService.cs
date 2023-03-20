
using EcommerceApi.Entities;

namespace EcommerceApi.Services.Interfaces
{
    public interface ICartItemService
    {
        public Task<IEnumerable<CartItem>> GetCartItems(Guid userId);
        public Task<CartItem> GetCartItemById(Guid cartItemId);
        public Task<CartItem> AddCartItem(Guid userId, string productName, double productPrice, int quantity);
        public Task<CartItem?> UpdateCartItem(int quantity, Guid cartItemId);
        public Task<CartItem?> DeleteCartItem(Guid cartItemId);
    }
}
