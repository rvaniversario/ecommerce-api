using Autofac;
using EcommerceApi.Services.Interfaces;
using EcommerceApi.Services;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace EcommerceApi.AutofacModules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CartItemService>().As<ICartItemService>().SingleInstance();
            builder.RegisterType<OrderService>().As<IOrderService>().SingleInstance();
            builder.RegisterType<UserService>().As<IUserService>().SingleInstance();

            builder.RegisterMediatR(typeof(CartItemService).Assembly);
            builder.RegisterMediatR(typeof(OrderService).Assembly);
            builder.RegisterMediatR(typeof(UserService).Assembly);
        }
    }
}
