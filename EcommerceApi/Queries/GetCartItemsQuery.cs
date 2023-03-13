using MediatR;
using EcommerceApi.Models;
using EcommerceApi.Entities;

namespace EcommerceApi.Queries
{
    public class GetCartItemsQuery : CartItemModel, IRequest<IEnumerable<CartItem>> { }
}