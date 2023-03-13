using EcommerceApi.Dtos;
using EcommerceApi.Entities;


namespace EcommerceApi.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<User?> GetUserById(Guid id);
        public Task<UserDtoOutput> AddUser(User user);
    }
}
