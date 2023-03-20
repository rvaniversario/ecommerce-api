using MediatR;
using EcommerceApi.Commands;

using EcommerceApi.Services.Interfaces;
using EcommerceApi.Entities;

namespace EcommerceApi.Handlers
{
    public class UpdateCartItemHandler : IRequestHandler<UpdateCartItemCommand, CartItem?>
    {
        private readonly ICartItemService _cartItemService;

        public UpdateCartItemHandler(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public async Task<CartItem?> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            var output = await _cartItemService.UpdateCartItem(request.Quantity, request.Id);
            return output;
        }
    }
}