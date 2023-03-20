using MediatR;
using EcommerceApi.Dtos;
using EcommerceApi.Models;

namespace EcommerceApi.Commands
{
    public class DeleteCartItemCommand : CartItemModel, IRequest<CartItemDtoOutput> { }
}
