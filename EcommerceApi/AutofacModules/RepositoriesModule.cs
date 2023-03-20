using Autofac;
using EcommerceApi.Handlers;
using EcommerceApi.Repositories;
using EcommerceApi.Repositories.Interfaces;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace EcommerceApi.AutofacModules
{
    public class RepositoriesModule : Module
    {
        private string _conString;
        public RepositoriesModule(string conString)
        {
            _conString = conString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => _conString).As<string>();
            builder.RegisterType<CartItemRepository>().As<ICartItemRepository>().WithParameter("constring", _conString).SingleInstance();
            builder.RegisterType<OrderRepository>().As<IOrderRepository>().WithParameter("constring", _conString).SingleInstance();
            builder.RegisterType<UserRepository>().As<IUserRepository>().WithParameter("constring", _conString).SingleInstance();

            builder.RegisterMediatR(typeof(CartItemRepository).Assembly);
            builder.RegisterMediatR(typeof(OrderRepository).Assembly);
            builder.RegisterMediatR(typeof(UserRepository).Assembly);
        }
    }
}
