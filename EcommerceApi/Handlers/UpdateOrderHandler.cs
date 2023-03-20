using MediatR;
using EcommerceApi.Commands;
using EcommerceApi.Dtos;
using EcommerceApi.Services.Interfaces;

namespace EcommerceApi.Handlers
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, OrderDtoOutput?>
    {
        private readonly IOrderService _orderService;

        public UpdateOrderHandler(IOrderService orderService) => _orderService = orderService;

        public async Task<OrderDtoOutput?> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var output = await _orderService.UpdateOrderStatus(request.Status, request.Id);
            return output;
        }
    }
}
