using EcommerceApi.Dtos;
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

        public async Task<CheckoutDtoOutput?> Checkout(Guid id)
        {
            var orders = await _orderRepository.GetOrders();

            var orderToCheckout = orders.FirstOrDefault(x => x.Status == Status.Pending);

            if (orderToCheckout == null) return default;

            var status = Status.Processed;

            return await _orderRepository.Checkout(orderToCheckout.Id, status);
        }

        public async Task<OrderDtoOutput?> Delete(Guid id)
        {
            var orderToDelete = await _orderRepository.GetOrderById(id);

            if (orderToDelete == null) return default;

            return await _orderRepository.DeleteOrder(orderToDelete.Id);
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            var orders = await _orderRepository.GetOrders();
            return orders;
        }

        public async Task<Order?> GetById(Guid id)
        {
            var order = await _orderRepository.GetOrderById(id);

            if (order == null) return default;

            return order;
        }

        public async Task<OrderDtoOutput?> UpdateOrderStatus(Status status, Guid id)
        {
            var orderToUpdate = await _orderRepository.GetOrderById(id);

            if (orderToUpdate == null) return default;

            var order = await _orderRepository.UpdateOrderStatus(orderToUpdate.Id, status);

            return new OrderDtoOutput
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderPrice = order.OrderPrice,
                Status = order.Status,
                CartItems = order.CartItems
            };
        }

        public async Task<Order?> UpdateOrderPrice(double orderPrice, Guid id)
        {
            var orderToUpdate = await _orderRepository.GetOrderById(id);

            if (orderToUpdate == null) return default;

            var order = await _orderRepository.UpdateOrderPrice(orderToUpdate.Id, orderPrice);

            return order;
        }

        public async Task Add(Guid userId, Status status, double orderPrice)
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
