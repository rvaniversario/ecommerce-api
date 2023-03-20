using MediatR;
using EcommerceApi.Queries;
using EcommerceApi.Services.Interfaces;
using EcommerceApi.Entities;

namespace EcommerceApi.Handlers
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User?>
    {
        private readonly IUserService _userService;

        public GetUserByIdHandler(IUserService userService) => _userService = userService;

        public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserById(request.Id);
            return user;
        }
    }
}