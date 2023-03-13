using MediatR;
using EcommerceApi.Dtos;
using EcommerceApi.Models;

namespace EcommerceApi.Commands
{
    public class UpdateCartItemCommand : CartItemModel, IRequest<CartItemDtoOutput> { }
}