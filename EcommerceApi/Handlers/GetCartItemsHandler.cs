using MediatR;
using EcommerceApi.Queries;
using EcommerceApi.Services.Interfaces;
using EcommerceApi.Entities;

namespace EcommerceApi.Handlers
{
    public class GetCartItemsHandler : IRequestHandler<GetCartItemsQuery, IEnumerable<CartItem>>
    {
        private readonly ICartItemService _cartItemService;

        public GetCartItemsHandler(ICartItemService cartItemService) => _cartItemService = cartItemService;

        public async Task<IEnumerable<CartItem>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
        {
            var cartItems = await _cartItemService.GetAll();
            return cartItems;
        }
    }
}
