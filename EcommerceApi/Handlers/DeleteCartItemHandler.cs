using MediatR;
using EcommerceApi.Commands;
using EcommerceApi.Services.Interfaces;
using EcommerceApi.Entities;

namespace EcommerceApi.Handlers
{
    public class DeleteCartItemHandler : IRequestHandler<DeleteCartItemCommand, CartItem?>
    {
        private readonly ICartItemService _cartItemService;

        public DeleteCartItemHandler(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public async Task<CartItem?> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
        {
            var output = await _cartItemService.DeleteCartItem(request.Id);
            return output;
        }
    }
}
