using MediatR;
using EcommerceApi.Commands;
using EcommerceApi.Dtos;
using EcommerceApi.Services.Interfaces;

namespace EcommerceApi.Handlers
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, OrderDtoOutput?>
    {
        private readonly IOrderService _orderService;

        public DeleteOrderHandler(IOrderService orderService) => _orderService = orderService;

        public async Task<OrderDtoOutput?> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var output = await _orderService.Delete(request.Id);
            return output;
        }
    }
}
