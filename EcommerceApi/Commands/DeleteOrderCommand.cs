using MediatR;
using EcommerceApi.Dtos;
using EcommerceApi.Models;

namespace EcommerceApi.Commands
{
    public class DeleteOrderCommand : OrderModel, IRequest<OrderDtoOutput> { }
}
