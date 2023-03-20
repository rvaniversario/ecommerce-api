using MediatR;
using EcommerceApi.Commands;

using EcommerceApi.Services.Interfaces;
using EcommerceApi.Entities;

namespace EcommerceApi.Handlers
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, Order?>
    {
        private readonly IOrderService _orderService;

        public DeleteOrderHandler(IOrderService orderService) => _orderService = orderService;

        public async Task<Order?> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var output = await _orderService.DeleteOrder(request.Id);
            return output;
        }
    }
}
