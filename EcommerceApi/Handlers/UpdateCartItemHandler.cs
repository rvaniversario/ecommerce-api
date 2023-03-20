using MediatR;
using EcommerceApi.Commands;
using EcommerceApi.Dtos;
using EcommerceApi.Services.Interfaces;

namespace EcommerceApi.Handlers
{
    public class UpdateCartItemHandler : IRequestHandler<UpdateCartItemCommand, CartItemDtoOutput?>
    {
        private readonly ICartItemService _cartItemService;

        public UpdateCartItemHandler(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public async Task<CartItemDtoOutput?> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
        {
            var output = await _cartItemService.Update(request.Quantity, request.Id);
            return output;
        }
    }
}