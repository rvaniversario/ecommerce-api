using Dapper;
using Microsoft.EntityFrameworkCore;
using EcommerceApi.Controllers;
using EcommerceApi.Context;
using EcommerceApi.Repositories.Interfaces;

using EcommerceApi.Entities;
using Microsoft.Data.SqlClient;

namespace EcommerceApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _conString;
    protected readonly AppDbContext _context;
    protected readonly ILogger<UsersController> _logger;

    public UserRepository(AppDbContext context, ILogger<UsersController> logger, string conString)
    {
        _context = context;
        _logger = logger;
        _conString = conString;

        _context.Database.EnsureCreated();
    }

    public async Task<User?> GetUserById(Guid id)
    {
        try
        {
            _logger.LogInformation("Creating connection...");
            using var connection = new SqlConnection(_conString);
            var query = "SELECT * FROM Users WHERE Id = @Id";

            _logger.LogInformation("Fetching User With ID: {id}...", id);
            var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { id });

            if (user == null) return null;

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError("{ex}", ex);
            throw;
        }
    }

    public async Task<User> AddUser(User user)
    {
        try
        {
            _logger.LogInformation("Adding User...");
            await _context.Users!.AddAsync(user);

            _logger.LogInformation("Saving Changes...");
            await _context.SaveChangesAsync();

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError("{ex}", ex);
            throw;
        }
    }
}
