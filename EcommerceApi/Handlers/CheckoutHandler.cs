using MediatR;
using EcommerceApi.Commands;

using EcommerceApi.Services.Interfaces;
using EcommerceApi.Entities;

namespace EcommerceApi.Handlers
{
    public class CheckoutHandler : IRequestHandler<CheckoutCommand, Order?>
    {
        private readonly IOrderService _orderService;

        public CheckoutHandler(IOrderService orderService) => _orderService = orderService;

        public async Task<Order?> Handle(CheckoutCommand request, CancellationToken cancellationToken)
        {
            var output = await _orderService.Checkout(request.Id);
            return output;
        }
    }
}
