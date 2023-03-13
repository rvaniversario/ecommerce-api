using MediatR;
using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Commands;
using EcommerceApi.Dto;
using EcommerceApi.Models;
using EcommerceApi.Queries;

namespace EcommerceApi.Controllers
{
    [ApiVersion("2.0")]
    [Route("/api/v{version:apiVersion}/users")]
    [ApiController]
    public class UsersV2Controller : ControllerBase
    {
        private IMediator _mediator;

        public UsersV2Controller(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [Route("{userId:Guid}")]
        public async Task<ActionResult> GetUser(Guid userId)
        {
            var result = await _mediator.Send(new GetUserByIdQuery { UserId = userId });

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(CreateUserDto createUserDto)
        {
            AddUserCommand command = new()
            {
                Name = createUserDto.Name,
            };

            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }   
}
