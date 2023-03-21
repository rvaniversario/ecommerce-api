using Dapper;
using EcommerceApi.Controllers;
using EcommerceApi.Context;
using EcommerceApi.Repositories.Interfaces;
using EcommerceApi.Entities;
using Microsoft.Data.SqlClient;

namespace EcommerceApi.Repositories;

public class CartItemRepository : ICartItemRepository
{
    private readonly string _conString;
    protected readonly AppDbContext _context;
    protected readonly ILogger<CartItemsController> _logger;

    public CartItemRepository(AppDbContext context, ILogger<CartItemsController> logger, string conString)
    {
        _context = context;
        _logger = logger;
        _conString = conString;

        _context.Database.EnsureCreated();
    }

    public async Task<IEnumerable<CartItem>> GetCartItems(Guid OrderId)
    {
        try
        {
            _logger.LogInformation("Creating connection...");
            using var connection = new SqlConnection(_conString);
            var query = "SELECT * FROM CartItems WHERE OrderId = @Id";

            _logger.LogInformation("Opening connection...");
            await connection.OpenAsync();

            var items = connection.Query<CartItem>(query, new { Id = OrderId });

            _logger.LogInformation("Fetching Cart Items...");
            return items;
        }
        catch (Exception ex)
        {
            _logger.LogError("{ex}", ex);
            throw;
        }
    }

    public async Task<CartItem> AddCartItem(CartItem item)
    {
        try
        {
            _logger.LogInformation("Adding Cart Item...");
            await _context.CartItems!.AddAsync(item);

            _logger.LogInformation("Saving Changes...");
            await _context.SaveChangesAsync();

            return item;
        }
        catch (Exception ex)
        {
            _logger.LogError("{ex}", ex);
            throw;
        }
    }

    public async Task<CartItem> UpdateCartItem(Guid id, double itemPrice, int quantity)
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

            return item;
        }
        catch (Exception ex)
        {
            _logger.LogError("{ex}", ex);
            throw;
        }
    }

    public async Task<CartItem> DeleteCartItem(Guid id)
    {
        try
        {
            var item = _context.CartItems.Find(id);

            _logger.LogInformation("Removing CartItem...");
            _context.CartItems.Remove(item);

            _logger.LogInformation("Saving Changes...");
            await _context.SaveChangesAsync();

            return item;
        }
        catch (Exception ex)
        {
            _logger.LogError("{ex}", ex);
            throw;
        }
    }

    public async Task<CartItem> GetCartItemById(Guid id)
    {
        try
        {
            _logger.LogInformation("Creating connection...");
            using var connection = new SqlConnection(_conString);
            var query = "SELECT * FROM CartItems WHERE Id = @Id";

            _logger.LogInformation("Fetching Cart-item With ID: {id}...", id);
            var cartItem = await connection.QuerySingleOrDefaultAsync<CartItem>(query, new { Id = id });

            if (cartItem == null) return null;

            return cartItem;
        }
        catch (Exception ex)
        {
            _logger.LogError("{ex}", ex);
            throw;
        }
    }
}
