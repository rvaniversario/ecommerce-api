using MediatR;
using EcommerceApi.Commands;
using EcommerceApi.Services.Interfaces;
using EcommerceApi.Entities;

namespace EcommerceApi.Handlers
{
    public class AddCartItemHandler : IRequestHandler<AddCartItemCommand, CartItem>
    {
        private readonly ICartItemService _cartItemService;

        public AddCartItemHandler(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public async Task<CartItem> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            var output = await _cartItemService.AddCartItem(
                request.UserId,
                request.ProductName!,
                request.ProductPrice,
                request.Quantity);

            return output;
        }
    }
}
