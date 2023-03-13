using EcommerceApi.Dtos;
using EcommerceApi.Entities;

namespace EcommerceApi.Services.Interfaces
{
    public interface ICartItemService
    {
        public Task<IEnumerable<CartItem>> GetAll();
        public Task<CartItemDtoOutput> Add(Guid userId,string productName,double productPrice,int quantity);
        public Task<CartItemDtoOutput?> Update(int quantity, Guid id);
        public Task<CartItemDtoOutput?> Delete(Guid id);
    }
}
