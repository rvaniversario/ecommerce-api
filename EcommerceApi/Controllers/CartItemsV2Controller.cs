using AutoMapper;
using EcommerceApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EcommerceApi.Commands;
using EcommerceApi.Data;
using EcommerceApi.Dto;
using EcommerceApi.Enums;
using EcommerceApi.Models;
using EcommerceApi.Queries;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EcommerceApi.Controllers
{
    [ApiVersion("2.0")]
    [Route("/api/v{version:apiVersion}/cart-items")]
    [ApiController]
    public class CartItemsV2Controller : EcommerceControllerBase
    {
        [HttpGet]
        public async Task<ActionResult> GetCartItems()
        {
            var result = await Mediator.Send(new GetCartItemsQuery());

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddCartItem([FromBody] CreateCartItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (dto.Price.EndsWith("0"))
            {
                var price = dto.Price.Append('0');
                dto.Price = (string)price;
            }
            try
            {
                string? userId = Request.Headers["x-user-id"].FirstOrDefault();
                double price = double.Parse(dto.Price);

                var result = await Mediator.Send(new AddCartItemCommand
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    ProductName = dto.ProductName,
                    Price = price,
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                Logger.LogError("{ex}", ex);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [HttpPut]
        [Route("{itemId:Guid}")]
        public async Task<ActionResult> UpdateCartItem([FromBody] UpdateCartItemDto dto, [FromRoute] Guid itemId)
        {
            var result = await Mediator.Send(new UpdateCartItemCommand
            {
                Id = itemId,
                ProductName = dto.ProductName,
            });

            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("{itemId:Guid}")]
        public async Task<ActionResult> DeleteCartItem([FromRoute] Guid itemId)
        {
            var result = await Mediator.Send(new DeleteCartItemCommand
            {
                ItemId = itemId
            });

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
