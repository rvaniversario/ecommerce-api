using EcommerceApi.Entities;
using EcommerceApi.Repositories.Interfaces;
using EcommerceApi.Services.Interfaces;

namespace EcommerceApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AddUser(string name)
        {
            var UserEntity = new User
            {
                Name = name,
            };
            return await _userRepository.AddUser(UserEntity);
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _userRepository.GetUserById(id);
        }
    }
}
