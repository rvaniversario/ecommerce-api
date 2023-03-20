using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Commands;
using EcommerceApi.Dtos;
using EcommerceApi.Queries;
using MediatR;

namespace EcommerceApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetOrders()
        {
            var request = new GetOrdersQuery();

            try
            {
                var response = await _mediator.Send(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the {request}", nameof(request));
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
        [HttpGet]
        [Route("{orderId:Guid}")]
        public async Task<ActionResult> GetOrder([FromRoute] Guid orderId)
        {
            var request = new GetOrderByIdQuery
            {
                Id = orderId
            };

            try
            {
                var result = await _mediator.Send(request);

                if (result == null)
                {
                    return NotFound($"Could not find order with ID: {orderId}.");   
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the {request}", nameof(request));
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPut]
        [Route("{orderId:Guid}")]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderDto dto, [FromRoute]Guid orderId)
        {
            var request = new UpdateOrderCommand
            {
                Id = orderId,
                Status = dto.Status,
            };

            try
            {
                var response = await _mediator.Send(request);

                if (response == null)
                {
                    return NotFound($"Could not find order with ID: {orderId}.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the {request}", nameof(request));
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpDelete]
        [Route("{orderId:Guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid orderId)
        {
            var request = new DeleteOrderCommand
            {
                Id = orderId
            };

            try
            {
                var response = await _mediator.Send(request);

                if (response == null)
                {
                    return NotFound($"Could not find order with ID: {orderId}.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the {request}", nameof(request));
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }
}
