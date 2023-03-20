
using EcommerceApi.Entities;
using EcommerceApi.Enums;
using EcommerceApi.Repositories.Interfaces;
using EcommerceApi.Services.Interfaces;

namespace EcommerceApi.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order?> Checkout(Guid userId)
        {
            var orders = await _orderRepository.GetOrders(userId);

            var orderToCheckout = orders.FirstOrDefault(x => x.Status == Status.Pending);

            if (orderToCheckout == null) return default;

            return await _orderRepository.Checkout(orderToCheckout.Id, Status.Processed);
        }

        public async Task<Order?> DeleteOrder(Guid id)
        {
            var orderToDelete = await _orderRepository.GetOrderById(id);

            if (orderToDelete == null) return default;

            return await _orderRepository.DeleteOrder(orderToDelete.Id);
        }

        public async Task<IEnumerable<Order>> GetOrders(Guid userId)
        {
            var orders = await _orderRepository.GetOrders(userId);

            if (!orders.Any()) return null;

            return orders;
        }

        public async Task<Order?> GetOrderById(Guid id)
        {
            var order = await _orderRepository.GetOrderById(id);

            if (order == null) return default;

            return order;
        }

        public async Task<Order?> UpdateOrderStatus(Status status, Guid id)
        {
            var orderToUpdate = await _orderRepository.GetOrderById(id);

            if (orderToUpdate == null) return default;

            var order = await _orderRepository.UpdateOrderStatus(orderToUpdate.Id, status);

            return order;
        }

        public async Task UpdateOrderPrice(double orderPrice, Guid id)
        {
            var orderToUpdate = await _orderRepository.GetOrderById(id);

            await _orderRepository.UpdateOrderPrice(orderToUpdate.Id, orderPrice);
        }

        public async Task AddOrder(Guid userId, Status status, double orderPrice)
        {
            var order = new Order
            {
                UserId = userId,
                Status = status,
                OrderPrice = orderPrice
            };

            await _orderRepository.AddOrder(order);
        }
    }
}
