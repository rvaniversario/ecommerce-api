using MediatR;
using EcommerceApi.Models;
using EcommerceApi.Entities;

namespace EcommerceApi.Queries
{
    public class GetUserByIdQuery: UserModel, IRequest<User> { }
}
