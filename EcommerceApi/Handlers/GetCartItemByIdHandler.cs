using EcommerceApi.Entities;
using EcommerceApi.Queries;
using EcommerceApi.Services.Interfaces;
using MediatR;

namespace EcommerceApi.Handlers;

public class GetCartItemByIdHandler : IRequestHandler<GetCartItemByIdQuery, CartItem?>
{
    private readonly ICartItemService _cartItemService;

    public GetCartItemByIdHandler(ICartItemService cartItemService) => _cartItemService = cartItemService;

    public async Task<CartItem?> Handle(GetCartItemByIdQuery request, CancellationToken cancellationToken)
    {
        var cartItem = await _cartItemService.GetCartItemById(request.Id);
        return cartItem;
    }
}
