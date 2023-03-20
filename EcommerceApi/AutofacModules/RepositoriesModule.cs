using Autofac;
using EcommerceApi.Repositories;
using EcommerceApi.Repositories.Interfaces;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace EcommerceApi.AutofacModules
{
    public class RepositoriesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CartItemRepository>().As<ICartItemRepository>().SingleInstance();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().SingleInstance();
            builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();

            builder.RegisterMediatR(typeof(CartItemRepository).Assembly);
            builder.RegisterMediatR(typeof(OrderRepository).Assembly);
            builder.RegisterMediatR(typeof(UserRepository).Assembly);
        }
    }
}
