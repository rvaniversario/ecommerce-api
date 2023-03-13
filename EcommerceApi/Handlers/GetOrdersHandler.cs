using MediatR;
using EcommerceApi.Queries;
using EcommerceApi.Services.Interfaces;
using EcommerceApi.Dtos;
using EcommerceApi.Entities;

namespace EcommerceApi.Handlers
{
    public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, IEnumerable<Order>>
    {
        private readonly IOrderService _orderService;

        public GetOrdersHandler(IOrderService orderService) => _orderService = orderService;

        public async Task<IEnumerable<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAll();
            return orders;
        }
    }
}
