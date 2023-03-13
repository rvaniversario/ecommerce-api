using MediatR;
using EcommerceApi.Commands;
using EcommerceApi.Dtos;
using EcommerceApi.Services.Interfaces;

namespace EcommerceApi.Handlers
{
    public class AddCartItemHandler : IRequestHandler<AddCartItemCommand, CartItemDtoOutput>
    {
        private readonly ICartItemService _cartItemService;

        public AddCartItemHandler(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public async Task<CartItemDtoOutput> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            var output = await _cartItemService.Add(
                request.UserId,
                request.ProductName!,
                request.ProductPrice,
                request.Quantity);

            return output;
        }
    }
}
