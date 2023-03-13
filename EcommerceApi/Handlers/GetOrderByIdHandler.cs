using MediatR;
using EcommerceApi.Queries;
using EcommerceApi.Services.Interfaces;
using EcommerceApi.Entities;

namespace EcommerceApi.Handlers
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, Order?>
    {
        private readonly IOrderService _orderService;

        public GetOrderByIdHandler(IOrderService orderService) => _orderService = orderService;

        public async Task<Order?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetById(request.Id);
            return order;
        }
    }
}
