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
    public class CheckoutCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WithPendingOrder_ShouldCheckoutOrder()
        {
            // Arrange
            var command = new Faker<CheckoutCommand>()
                .RuleFor(c => c.Id, f => f.Random.Guid()).Generate();

            var order = new Faker<Order>()
                .RuleFor(c => c.Id, f => f.Random.Guid())
                .RuleFor(c => c.UserId, command.Id)
                .RuleFor(c => c.OrderPrice, f => f.Random.Double())
                .RuleFor(c => c.Status, f => f.Random.Enum(Status.Pending, Status.Cancelled)).Generate();

            var mockOrderService = new Mock<IOrderService>();
            mockOrderService
                .Setup(s => s.Checkout(It.IsAny<Guid>()))
                .ReturnsAsync(order);

            var handler = new CheckoutHandler(mockOrderService.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(order);

            mockOrderService.Verify(s => s.Checkout(It.IsAny<Guid>()), Times.Once);
        }
    }
}
