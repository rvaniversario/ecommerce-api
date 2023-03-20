using Dapper;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.Controllers;
using EcommerceApi.Context;
using EcommerceApi.Repositories.Interfaces;
using EcommerceApi.Entities;
using EcommerceApi.Dtos;
using Microsoft.Data.SqlClient;

namespace EcommerceApi.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly string _connectionString;
        protected readonly AppDbContext _context;
        protected readonly ILogger<CartItemsController> _logger;

        public CartItemRepository(AppDbContext context, ILogger<CartItemsController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("EcommerceApi");
            //_connectionString = configuration.GetConnectionString("EcommerceApiTest");

            _context.Database.EnsureCreated();
        }

        public async Task<IEnumerable<CartItem>> GetCartItems()
        {
            try
            {
                _logger.LogInformation("Creating connection...");
                using var connection = new SqlConnection(_connectionString);
                var query = "SELECT * FROM CartItems";

                _logger.LogInformation("Opening connection...");
                await connection.OpenAsync();

                _logger.LogInformation("Fetching Cart Items...");
                var items = await connection.QueryAsync<CartItem>(query);

                return items;
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                throw;
            }
        }

        public async Task<CartItemDtoOutput> AddCartItem(CartItem item)
        {
            try
            {
                _logger.LogInformation("Adding Cart Item...");
                await _context.CartItems!.AddAsync(item);

                _logger.LogInformation("Saving Changes...");
                await _context.SaveChangesAsync();

                return new CartItemDtoOutput
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    ProductName = item.ProductName,
                    ProductPrice = item.ProductPrice,
                    ItemPrice = item.ItemPrice,
                    Quantity = item.Quantity,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                throw;
            }
        }

        public async Task<CartItemDtoOutput> UpdateCartItem(Guid id, double itemPrice, int quantity)
        {
            try
            {
                var item = _context.CartItems.Find(id);

                _logger.LogInformation("Updating item...");
                item.ItemPrice = itemPrice;
                item.Quantity = quantity;

                _context.Update(item);

                _logger.LogInformation("Saving Changes...");
                await _context.SaveChangesAsync();

                return new CartItemDtoOutput
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    ProductName = item.ProductName,
                    ProductPrice = item.ProductPrice,
                    ItemPrice = item.ItemPrice,
                    Quantity = item.Quantity,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                throw;
            }
        }

        public async Task<CartItemDtoOutput> DeleteCartItem(Guid id)
        {
            try
            {
                var item = _context.CartItems.Find(id);

                _logger.LogInformation("Removing CartItem...");
                _context.CartItems.Remove(item);

                _logger.LogInformation("Saving Changes...");
                await _context.SaveChangesAsync();

                return new CartItemDtoOutput
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    ProductName = item.ProductName,
                    ProductPrice = item.ProductPrice,
                    ItemPrice = item.ItemPrice,
                    Quantity = item.Quantity,
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
