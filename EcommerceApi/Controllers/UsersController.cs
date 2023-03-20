using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Commands;
using EcommerceApi.Dtos;
using EcommerceApi.Queries;
using MediatR;

namespace EcommerceApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("/api/v{version:apiVersion}/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IMediator mediator, ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("{userId:Guid}")]
        public async Task<ActionResult> GetUser([FromRoute] Guid userId)
        {
            var request = new GetUserByIdQuery { Id = userId };

            try
            {
                var response = await _mediator.Send(request);

                if (response == null)
                {
                    return BadRequest($"Could not find User with ID: {userId}.");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the {request}", nameof(request));
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(AddUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var command = new AddUserCommand
            {
                Name = dto.Name,
            };

            try
            {
                var response = await _mediator.Send(command);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the {request}", nameof(command));
                return StatusCode(500, "An unexpected error occurred. Please try again later.");
            }
        }
    }   
}
