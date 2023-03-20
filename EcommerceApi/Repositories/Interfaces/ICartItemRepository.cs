using EcommerceApi.Dtos;
using EcommerceApi.Entities;

namespace EcommerceApi.Repositories.Interfaces
{
    public interface ICartItemRepository
    {
        public Task<IEnumerable<CartItem>> GetCartItems();
        public Task<CartItemDtoOutput> AddCartItem(CartItem item);
        public Task<CartItemDtoOutput> UpdateCartItem(Guid id, double itemPrice, int quantity);
        public Task<CartItemDtoOutput> DeleteCartItem(Guid id);
    }
}
