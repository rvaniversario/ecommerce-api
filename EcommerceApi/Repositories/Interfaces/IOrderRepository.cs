using EcommerceApi.Dtos;
using EcommerceApi.Entities;
using EcommerceApi.Enums;

namespace EcommerceApi.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetOrders();
        public Task<Order?> GetOrderById(Guid id);
        public Task AddOrder(Order order);
        public Task<CheckoutDtoOutput> Checkout(Guid id, Status status);
        public Task<Order> UpdateOrderStatus(Guid id,Status status);
        public Task<Order> UpdateOrderPrice(Guid id, double orderPrice);
        public Task<OrderDtoOutput> DeleteOrder(Guid id);
    }
}
