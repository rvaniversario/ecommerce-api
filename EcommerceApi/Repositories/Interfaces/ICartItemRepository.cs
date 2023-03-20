
using EcommerceApi.Entities;

namespace EcommerceApi.Repositories.Interfaces
{
    public interface ICartItemRepository
    {
        public Task<IEnumerable<CartItem>> GetCartItems(Guid orderId);
        public Task<CartItem> GetCartItemById(Guid cartItemId);
        public Task<CartItem> AddCartItem(CartItem cartItem);
        public Task<CartItem> UpdateCartItem(Guid cartItemId, double itemPrice, int quantity);
        public Task<CartItem> DeleteCartItem(Guid cartItemId);
    }
}
