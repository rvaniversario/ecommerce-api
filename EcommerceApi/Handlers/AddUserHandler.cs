using MediatR;
using EcommerceApi.Commands;
using EcommerceApi.Services.Interfaces;
using EcommerceApi.Entities;

namespace EcommerceApi.Handlers
{
    public class AddUserHandler : IRequestHandler<AddUserCommand, User>
    {
        private readonly IUserService _userService;

        public AddUserHandler(IUserService userService) => _userService = userService;

        public async Task<User> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var output = await _userService.AddUser(request.Name!);
            return output;
        }
    }
}
