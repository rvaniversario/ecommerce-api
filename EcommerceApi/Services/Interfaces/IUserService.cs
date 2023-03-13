using EcommerceApi.Dtos;
using EcommerceApi.Entities;

namespace EcommerceApi.Services.Interfaces
{
    public interface IUserService
    {
        public Task<User?> GetById(Guid id);
        public Task<UserDtoOutput> Add(string name);
    }
}
