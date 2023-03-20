using MediatR;
using EcommerceApi.Commands;

using EcommerceApi.Services.Interfaces;
using EcommerceApi.Entities;

namespace EcommerceApi.Handlers
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, Order?>
    {
        private readonly IOrderService _orderService;

        public UpdateOrderHandler(IOrderService orderService) => _orderService = orderService;

        public async Task<Order?> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var output = await _orderService.UpdateOrderStatus(request.Status, request.Id);
            return output;
        }
    }
}
