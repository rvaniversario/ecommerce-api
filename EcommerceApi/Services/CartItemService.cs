using EcommerceApi.Entities;
using EcommerceApi.Enums;
using EcommerceApi.Repositories.Interfaces;
using EcommerceApi.Services.Interfaces;

namespace EcommerceApi.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IOrderRepository _orderRepository;

        public CartItemService(ICartItemRepository cartItemRepository, IOrderRepository orderRepository)
        {
            _cartItemRepository = cartItemRepository;
            _orderRepository = orderRepository;
        }

        public async Task<CartItem> AddCartItem(Guid userId, string productName, double productPrice, int quantity)
        {
            var orders = await _orderRepository.GetOrders(userId);

            var pendingOrder = orders.FirstOrDefault(x => x.Status == Status.Pending);

            if (pendingOrder == null)
            {
                var cartItemPrice = productPrice * quantity;

                var orderPrice = cartItemPrice;

                var newOrder = new Order
                {
                    UserId = userId,
                    OrderPrice = orderPrice,
                    Status = Status.Pending,
                };

                newOrder.OrderPrice = Math.Round(newOrder.OrderPrice, 2);

                await _orderRepository.AddOrder(newOrder);

                cartItemPrice = Math.Round(cartItemPrice, 2);

                var newItem = new CartItem
                {
                    OrderId = newOrder.Id,
                    ProductName = productName,
                    ProductPrice = productPrice,
                    ItemPrice = cartItemPrice,
                    Quantity = quantity
                };

                return await _cartItemRepository.AddCartItem(newItem);
            }
            else
            {
                var cartItemPrice = productPrice * quantity;
                cartItemPrice = Math.Round(cartItemPrice, 2);

                var newItem = new CartItem
                {
                    OrderId = pendingOrder.Id,
                    ProductName = productName,
                    ProductPrice = productPrice,
                    ItemPrice = cartItemPrice,
                    Quantity = quantity
                };

                var item = await _cartItemRepository.AddCartItem(newItem);

                var orderPrice = pendingOrder.OrderPrice + cartItemPrice;
                orderPrice = Math.Round(orderPrice, 2);

                await _orderRepository.UpdateOrderPrice(pendingOrder.Id, orderPrice);

                return item;
            }
        }

        public async Task<CartItem?> DeleteCartItem(Guid id)
        {
            var item = await _cartItemRepository.GetCartItemById(id);

            if (item == null) return default;

            var items = await _cartItemRepository.GetCartItems(item.OrderId);

            if (items.Count() < 2)
            {
                var deletedItem = await _cartItemRepository.DeleteCartItem(item.Id);

                await _orderRepository.DeleteOrder(item.OrderId);

                return deletedItem;
            }
            else
            {
                var order = await _orderRepository.GetOrderById(item.OrderId);

                var orderPrice = order.OrderPrice - item.ItemPrice;
                orderPrice = Math.Round(orderPrice, 2);

                var deletedItem = await _cartItemRepository.DeleteCartItem(item.Id);

                await _orderRepository.UpdateOrderPrice(order.Id, orderPrice);

                return deletedItem;
            }
        }

        public async Task<CartItem> GetCartItemById(Guid id)
        {
            var cartItem = await _cartItemRepository.GetCartItemById(id);

            if (cartItem == null) return default;

            return cartItem;
        }

        public async Task<IEnumerable<CartItem>> GetCartItems(Guid userId)
        {
            var orders = await _orderRepository.GetOrders(userId);

            if (!orders.Any()) return null;

            var order = orders.FirstOrDefault(x => x.Status == Status.Pending);

            var cartItems = await _cartItemRepository.GetCartItems(order.Id);
            return cartItems;
        }

        public async Task<CartItem?> UpdateCartItem(int quantity, Guid id)
        {
            var itemToUpdate = await _cartItemRepository.GetCartItemById(id);

            if (itemToUpdate == null) return null;

            var newItemPrice = itemToUpdate.ProductPrice * quantity;

            var orderToUpdate = await _orderRepository.GetOrderById(itemToUpdate!.OrderId);

            var newOrderPrice = orderToUpdate!.OrderPrice - itemToUpdate.ItemPrice;
            newOrderPrice += newItemPrice;

            newItemPrice = Math.Round(newItemPrice, 2);

            newOrderPrice = Math.Round(newOrderPrice, 2);

            var item = await _cartItemRepository.UpdateCartItem(itemToUpdate.Id, newItemPrice, quantity);
            await _orderRepository.UpdateOrderPrice(orderToUpdate.Id, newOrderPrice);

            return item;
        }
    }
}
