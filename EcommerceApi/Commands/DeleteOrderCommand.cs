using MediatR;

using EcommerceApi.Models;
using EcommerceApi.Entities;

namespace EcommerceApi.Commands
{
    public class DeleteOrderCommand : OrderModel, IRequest<Order> { }
}
