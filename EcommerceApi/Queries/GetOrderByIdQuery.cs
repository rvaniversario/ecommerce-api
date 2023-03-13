using EcommerceApi.Entities;
using EcommerceApi.Models;
using MediatR;


namespace EcommerceApi.Queries
{
    public class GetOrderByIdQuery : OrderModel, IRequest<Order> { }
}
