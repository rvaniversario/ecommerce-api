using MediatR;

using EcommerceApi.Models;
using EcommerceApi.Entities;

namespace EcommerceApi.Commands
{
    public class CheckoutCommand : BaseModel, IRequest<Order> { }
}
