using MediatR;

using EcommerceApi.Models;
using EcommerceApi.Entities;

namespace EcommerceApi.Commands
{
    public class AddUserCommand : UserModel, IRequest<User> { }
}
