
using EcommerceApi.Entities;
using EcommerceApi.Enums;

namespace EcommerceApi.Services.Interfaces
{
    public interface IOrderService
    {
        public Task AddOrder(Guid userId, Status status, double orderPrice);
        public Task<IEnumerable<Order>> GetOrders(Guid userId);
        public Task<Order?> GetOrderById(Guid id);
        public Task<Order?> Checkout(Guid id);
        public Task<Order?> UpdateOrderStatus(Status status, Guid id);
        public Task UpdateOrderPrice(double orderPrice, Guid id);
        public Task<Order?> DeleteOrder(Guid id);
    }
}
