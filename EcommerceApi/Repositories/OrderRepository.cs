using Dapper;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.Controllers;
using EcommerceApi.Context;
using EcommerceApi.Enums;
using EcommerceApi.Repositories.Interfaces;
using EcommerceApi.Entities;
using EcommerceApi.Dtos;
using Microsoft.Data.SqlClient;

namespace EcommerceApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;
        protected readonly AppDbContext _context;
        protected readonly ILogger<OrdersController> _logger;

        public OrderRepository(AppDbContext context, ILogger<OrdersController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("EcommerceApi");
            //_connectionString = configuration.GetConnectionString("EcommerceApiTest");

            _context.Database.EnsureCreated();
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            try
            {
                _logger.LogInformation("Creating connection...");
                using var connection = new SqlConnection(_connectionString);

                var query = "SELECT * FROM Orders AS A INNER JOIN CartItems AS B ON A.Id = B.OrderId";
                var orderDictionary = new Dictionary<Guid, Order>();

                _logger.LogInformation("Opening connection...");
                await connection.OpenAsync();

                _logger.LogInformation("Fetching orders...");
                var list = await connection.QueryAsync<Order, CartItem, Order>(
                    query,
                    (order, cartItem) =>
                    {
                        if (!orderDictionary.TryGetValue(order.Id, out Order? orderEntry))
                        {
                            orderEntry = order;

                            orderEntry.CartItems ??= new List<CartItem>();
                            orderDictionary.Add(orderEntry.Id, orderEntry);
                        }

                        orderEntry.CartItems!.Add(cartItem);
                        return orderEntry;
                    },
                    splitOn: "Id"
                    );

                return orderDictionary.Values;
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                throw;
            }
        }

        public async Task<Order?> GetOrderById(Guid id)
        {
            try
            {
                _logger.LogInformation("Creating connection...");
                using var connection = new SqlConnection(_connectionString);
                var query = "SELECT * FROM Orders WHERE Id = @Id;" + "SELECT * FROM CartItems WHERE OrderId = @Id";

                var multi = await connection.QueryMultipleAsync(query, new { id });

                _logger.LogInformation("Getting Order With Id: {id}...", id);
                var order = await multi.ReadSingleOrDefaultAsync<Order>();

                if (order == null) return default;
                var items = (await multi.ReadAsync<CartItem>()).ToList();
                order.CartItems = items;

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                throw;
            }
        }

        public async Task AddOrder(Order order)
        {
            try
            {
                _logger.LogInformation("Adding order...");
                await _context.Orders!.AddAsync(order);

                _logger.LogInformation("Saving Changes...");
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex.Message);
                throw;
            }
        }

        public async Task<CheckoutDtoOutput> Checkout(Guid id, Status status)
        {
            try
            {
                var order = _context.Orders.Find(id);

                _logger.LogInformation("Updating order...");
                order.Status = status;
                _context.Update(order);

                _logger.LogInformation("Saving Changes...");
                await _context.SaveChangesAsync();

                return new CheckoutDtoOutput
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    OrderPrice = order.OrderPrice,
                    Status = order.Status,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                throw;
            }
        }

        public async Task<Order> UpdateOrderPrice(Guid id, double orderPrice)
        {
            try
            {
                var order = _context.Orders.Find(id);

                _logger.LogInformation("Updating order...");
                order.OrderPrice = orderPrice;

                _context.Update(order);

                _logger.LogInformation("Saving Changes...");
                await _context.SaveChangesAsync();

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                throw;
            }
        }

        public async Task<Order> UpdateOrderStatus(Guid id, Status status)
        {
            try
            {
                var order = _context.Orders.Find(id);

                _logger.LogInformation("Updating order...");
                order.Status = status;
                _context.Update(order);

                _logger.LogInformation("Saving Changes...");
                await _context.SaveChangesAsync();

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                throw;
            }
        }

        public async Task<OrderDtoOutput> DeleteOrder(Guid id)
        {
            try
            {
                var order = _context.Orders.Find(id);

                _logger.LogInformation("Removing Order...");
                _context.Remove(order);

                _logger.LogInformation("Saving Changes...");
                await _context.SaveChangesAsync();

                return new OrderDtoOutput
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    OrderPrice = order.OrderPrice,
                    Status = order.Status,
                    CartItems = order.CartItems,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                throw;
            }
        }
    }
}
