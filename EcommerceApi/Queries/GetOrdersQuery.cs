using MediatR;
using EcommerceApi.Models;
using EcommerceApi.Entities;

namespace EcommerceApi.Queries
{
    public class GetOrdersQuery : OrderModel, IRequest<IEnumerable<Order>> { }
}
