using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Commands;
using EcommerceApi.Dtos;
using EcommerceApi.Queries;
using MediatR;

namespace EcommerceApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/cart-items")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CartItemsController> _logger;

        public CartItemsController(IMediator mediator, ILogger<CartItemsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetCartItems()
        {
            var request = new GetCartItemsQuery();

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

        [HttpPost]
        public async Task<ActionResult> AddCartItem([FromBody] AddCartItemDto dto)
        {
            var userId = Request.Headers["x-user-id"].FirstOrDefault();
            var parsedUserId = Guid.Parse(userId!);

            var request = new AddCartItemCommand
            {
                UserId = parsedUserId,
                ProductName = dto.ProductName,
                ProductPrice = dto.ProductPrice,
                Quantity = dto.Quantity,
            };
            
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

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<ActionResult> UpdateCartItem([FromBody] UpdateCartItemDto dto, [FromRoute] Guid id)
        {
            var userId = Request.Headers["x-user-id"].FirstOrDefault();
            var parsedUserId = Guid.Parse(userId!);

            var request = new UpdateCartItemCommand
            {
                Id = id,
                UserId = parsedUserId,
                Quantity = dto.Quantity,
            };

            try
            {
                var response = await _mediator.Send(request);

                if (response == null)
                {
                    return NotFound($"Could not find item with ID: {id}");
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
        [Route("{id:Guid}")]
        public async Task<ActionResult> DeleteCartItem([FromRoute] Guid id)
        {
            var request = new DeleteCartItemCommand
            {
                Id = id
            };

            try
            {
                var response = await _mediator.Send(request);

                if (response == null)
                {
                    return NotFound($"Could not find item with ID: {id}");
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