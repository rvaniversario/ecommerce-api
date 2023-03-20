using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Commands;

using EcommerceApi.Queries;
using MediatR;
using Bogus.DataSets;
using Microsoft.AspNetCore.WebUtilities;
using EcommerceApi.Dtos;

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
            var userId = Request.Headers["x-user-id"][0];

            var request = new GetCartItemsQuery
            {
                UserId = Guid.Parse(userId),
            };

            try
            {
                var response = await _mediator.Send(request);

                if (response == null)
                {
                    return Ok("No cart-items found.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the {request}", nameof(request));
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpGet]
        [Route("{cartItemId:Guid}")]
        public async Task<ActionResult> GetCartItemById([FromRoute] Guid cartItemId)
        {
            var request = new GetCartItemByIdQuery
            {
                Id = cartItemId
            };

            try
            {
                var result = await _mediator.Send(request);

                if (result == null)
                {
                    return NotFound($"Could not find cart-item with ID: {cartItemId}.");
                }

                return Ok(result);
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
            var userId = Request.Headers["x-user-id"][0];

            var request = new AddCartItemCommand
            {
                UserId = Guid.Parse(userId),
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
        [Route("{cartItemId:Guid}")]
        public async Task<ActionResult> UpdateCartItem([FromBody] UpdateCartItemDto dto, [FromRoute] Guid cartItemId)
        {
            var request = new UpdateCartItemCommand
            {
                Id = cartItemId,
                Quantity = dto.Quantity,
            };

            try
            {
                var response = await _mediator.Send(request);

                if (response == null)
                {
                    return NotFound($"Could not find item with ID: {cartItemId}");
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
        [Route("{cartItemId:Guid}")]
        public async Task<ActionResult> DeleteCartItem([FromRoute] Guid cartItemId)
        {
            var userId = Request.Headers["x-user-id"][0];

            var request = new DeleteCartItemCommand
            {
                Id = cartItemId,
                UserId = Guid.Parse(userId),
            };

            try
            {
                var response = await _mediator.Send(request);

                if (response == null)
                {
                    return NotFound($"Could not find item with ID: {cartItemId}");
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