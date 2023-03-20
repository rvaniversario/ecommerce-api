using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Commands;
using MediatR;
using Bogus.DataSets;
using Microsoft.AspNetCore.WebUtilities;

namespace EcommerceApi.Controllers;

[ApiVersion("1.0")]
[Route("/api/v{version:apiVersion}/checkout")]
[ApiController]
public class CheckoutController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CheckoutController> _logger;

    public CheckoutController(IMediator mediator, ILogger<CheckoutController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> Checkout()
    {
        var userId = Request.Headers["x-user-id"][0];

        var request = new CheckoutCommand()
        {
            Id = Guid.Parse(userId),
        };

        try
        {
            var result = await _mediator.Send(request);

            if (result == null)
            {
                return BadRequest($"No pending order for User ID: {userId}.");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the {request}", nameof(request));
            return StatusCode(500, "An unexpected error occurred. Please try again later.");
        }
    }
}