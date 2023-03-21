using EcommerceApi.Entities;
using EcommerceApi.Enums;

namespace EcommerceApi.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Task<IEnumerable<Order>> GetOrders(Guid userId);
        public Task<Order?> GetOrderById(Guid orderId);
        public Task AddOrder(Order order);
        public Task<Order> Checkout(Guid orderId, Status status);
        public Task<Order> UpdateOrderStatus(Guid orderId, Status status);
        public Task UpdateOrderPrice(Guid orderId, double orderPrice);
        public Task<Order> DeleteOrder(Guid orderId);
    }
}
