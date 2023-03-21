using EcommerceApi.Entities;

namespace EcommerceApi.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetUserById(Guid id);
        public Task<User> AddUser(string name);
    }
}
