using MediatR;
using EcommerceApi.Commands;
using EcommerceApi.Dtos;
using EcommerceApi.Services.Interfaces;

namespace EcommerceApi.Handlers
{
    public class DeleteCartItemHandler : IRequestHandler<DeleteCartItemCommand, CartItemDtoOutput?>
    {
        private readonly ICartItemService _cartItemService;

        public DeleteCartItemHandler(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        public async Task<CartItemDtoOutput?> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
        {
            var output = await _cartItemService.Delete(request.Id);
            return output;
        }
    }
}
