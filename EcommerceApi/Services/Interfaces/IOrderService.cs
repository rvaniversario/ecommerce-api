using EcommerceApi.Dtos;
using EcommerceApi.Entities;
using EcommerceApi.Enums;

namespace EcommerceApi.Services.Interfaces
{
    public interface IOrderService
    {
        public Task Add(Guid userId, Status status, double orderPrice);
        public Task<IEnumerable<Order>> GetAll();
        public Task<Order?> GetById(Guid id);
        public Task<CheckoutDtoOutput?> Checkout(Guid id);
        public Task<OrderDtoOutput?> UpdateOrderStatus(Status status, Guid id);
        public Task<Order?> UpdateOrderPrice(double orderPrice, Guid id);
        public Task<OrderDtoOutput?> Delete(Guid id);
    }
}
