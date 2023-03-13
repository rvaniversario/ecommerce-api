using MediatR;
using EcommerceApi.Commands;
using EcommerceApi.Dtos;
using EcommerceApi.Services.Interfaces;

namespace EcommerceApi.Handlers
{
    public class AddUserHandler : IRequestHandler<AddUserCommand, UserDtoOutput>
    {
        private readonly IUserService _userService;

        public AddUserHandler(IUserService userService) => _userService = userService;

        public async Task<UserDtoOutput> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var output = await _userService.Add(request.Name!);
            return output;
        }
    }
}
