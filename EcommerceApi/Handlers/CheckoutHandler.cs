using MediatR;
using EcommerceApi.Commands;
using EcommerceApi.Dtos;
using EcommerceApi.Services.Interfaces;

namespace EcommerceApi.Handlers
{
    public class CheckoutHandler : IRequestHandler<CheckoutCommand, CheckoutDtoOutput?>
    {
        private readonly IOrderService _orderService;

        public CheckoutHandler(IOrderService orderService) => _orderService = orderService;

        public async Task<CheckoutDtoOutput?> Handle(CheckoutCommand request, CancellationToken cancellationToken)
        {
            var output = await _orderService.Checkout(request.Id);
            return output;
        }
    }
}
