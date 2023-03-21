using Bogus;
using EcommerceApi.Commands;
using EcommerceApi.Entities;
using EcommerceApi.Enums;
using EcommerceApi.Handlers;
using EcommerceApi.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace UnitTest.Commands
{
    public class DeleteOrderCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WithValidOrder_ShouldDeleteOrder()
        {
            // Arrange
            var command = new Faker<DeleteOrderCommand>()
                .RuleFor(d => d.Id, f => f.Random.Guid()).Generate();

            var order = new Faker<Order>()
                .RuleFor(o => o.Id, command.Id)
                .RuleFor(o => o.UserId, f => f.Random.Guid())
                .RuleFor(o => o.OrderPrice, f => f.Random.Double())
                .RuleFor(o => o.Status, f => f.Random.Enum<Status>()).Generate();

            var mockOrderService = new Mock<IOrderService>();
            mockOrderService
                .Setup(s => s.DeleteOrder(It.IsAny<Guid>()))
                .ReturnsAsync(order);

            var handler = new DeleteOrderHandler(mockOrderService.Object);

            // Act
            var response = await handler.Handle(command, CancellationToken.None);

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(order.Id);
            response.UserId.Should().Be(order.UserId);
            response.OrderPrice.Should().Be(order.OrderPrice);
            response.Status.Should().Be(order.Status);

            mockOrderService.Verify(s => s.DeleteOrder(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidOrder_ShouldNotDeleteOrder()
        {
            // Arrange
            var command = new Faker<DeleteOrderCommand>()
                .RuleFor(d => d.Id, f => f.Random.Guid()).Generate();

            var mockOrderService = new Mock<IOrderService>();

            mockOrderService
                .Setup(s => s.DeleteOrder(It.IsAny<Guid>()))
                .ReturnsAsync(null as Order);

            var handler = new DeleteOrderHandler(mockOrderService.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            mockOrderService.Verify(s => s.DeleteOrder(It.IsAny<Guid>()), Times.Once);
        }
    }
}
