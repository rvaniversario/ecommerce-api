using EcommerceApi.Dtos;
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

        public async Task<CartItemDtoOutput> Add(Guid userId,string productName,double productPrice,int quantity)
        {
            var orders = await _orderRepository.GetOrders();

            var pendingOrder = orders.FirstOrDefault(x => x.UserId == userId && x.Status == Status.Pending);

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

                Math.Round(newOrder.OrderPrice, 2);

                await _orderRepository.AddOrder(newOrder);

                var newItem = new CartItem
                {
                    OrderId = newOrder.Id,
                    ProductName =  productName,
                    ProductPrice = productPrice,
                    ItemPrice = cartItemPrice,
                    Quantity = quantity
                };

                return await _cartItemRepository.AddCartItem(newItem);
            }
            else
            {
                var cartItemPrice = productPrice * quantity;
                var orderPrice = pendingOrder.OrderPrice + cartItemPrice;

                Math.Round(cartItemPrice, 2);
                Math.Round(orderPrice, 2);

                var newItem = new CartItem 
                {
                    OrderId = pendingOrder.Id,
                    ProductName = productName,
                    ProductPrice = productPrice,
                    ItemPrice = cartItemPrice,
                    Quantity = quantity 
                };

                var item = await _cartItemRepository.AddCartItem(newItem);

                await _orderRepository.UpdateOrderPrice(pendingOrder.Id, orderPrice);

                return item;
            }
        }

        public async Task<CartItemDtoOutput?> Delete(Guid id)
        {
            var items = await _cartItemRepository.GetCartItems();
            var item = items.FirstOrDefault(x => x.Id == id);

            if (item == null) return default;

            var order = await _orderRepository.GetOrderById(item.OrderId);

            var orderItems = items.Where(x => x.OrderId == item.OrderId);

            if (orderItems.Count() < 2)
            {
                var deletedItem = await _cartItemRepository.DeleteCartItem(item.Id);

                await _orderRepository.DeleteOrder(order.Id);

                return deletedItem;
            }
            else
            {
                var orderPrice = order.OrderPrice - item.ItemPrice;
                Math.Round(orderPrice, 2);

                var deletedItem = await _cartItemRepository.DeleteCartItem(item.Id);

                await _orderRepository.UpdateOrderPrice(order.Id, orderPrice);
                
                return deletedItem;
            }
        }

        public async Task<IEnumerable<CartItem>> GetAll()
        {
            var cartItems = await _cartItemRepository.GetCartItems();
            return cartItems;
        }

        public async Task<CartItemDtoOutput?> Update(int quantity, Guid id)
        {
            var items = await _cartItemRepository.GetCartItems();

            var itemToUpdate = items.FirstOrDefault(x => x.Id == id);

            if (itemToUpdate == null) return null;

            var newQuantity = quantity;
            var newItemPrice = itemToUpdate.ProductPrice * quantity;

            var orderToUpdate = await _orderRepository.GetOrderById(itemToUpdate!.OrderId);

            var newOrderPrice = orderToUpdate!.OrderPrice - itemToUpdate.ItemPrice;
            newOrderPrice += newItemPrice;

            Math.Round(newOrderPrice, 2);

            var item = await _cartItemRepository.UpdateCartItem(itemToUpdate.Id, newItemPrice, newQuantity);
            await _orderRepository.UpdateOrderPrice(orderToUpdate.Id, newOrderPrice);

            return item;
        }
    }
}
