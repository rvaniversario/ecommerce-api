using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using EcommerceApi.Commands;
using EcommerceApi.Data;
using EcommerceApi.Dto;
using EcommerceApi.Enums;
using EcommerceApi.Models;
using EcommerceApi.Repositories;

namespace EcommerceApi.Controllers
{
    [ApiVersion("2.0")]
    [Route("/api/v{version:apiVersion}/checkout")]
    [ApiController]
    public class CheckoutV2Controller : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _context;

        public CheckoutV2Controller(IMediator mediator, AppDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Checkout([FromBody] CheckoutDto checkoutDto)
        {
            CheckoutCommand command = new CheckoutCommand
            {
                UserId = checkoutDto.UserId,
            };

            var result = await _mediator.Send(command);

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
