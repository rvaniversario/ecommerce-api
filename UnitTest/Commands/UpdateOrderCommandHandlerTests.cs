using Bogus;
using EcommerceApi.Commands;
using EcommerceApi.Dtos;
using EcommerceApi.Enums;
using EcommerceApi.Handlers;
using EcommerceApi.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace UnitTest.Commands
{
    public class UpdateOrderCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WithValidOrder_ShouldUpdateOrder()
        {
            // Arrange
            var command = new Faker<UpdateOrderCommand>()
                .RuleFor(u => u.Id, f => f.Random.Guid())
                .RuleFor(u => u.Status, f => f.Random.Enum(Status.Pending,Status.Processed)).Generate();

            var orderDtoOutput = new Faker<OrderDtoOutput>()
                .RuleFor(o => o.Id, command.Id)
                .RuleFor(o => o.UserId, f => f.Random.Guid())
                .RuleFor(o => o.OrderPrice, f => f.Random.Double())
                .RuleFor(o => o.Status, command.Status).Generate();

            var mockOrderService = new Mock<IOrderService>();
            mockOrderService
                .Setup(s => s.UpdateOrderStatus(It.IsAny<Status>(), It.IsAny<Guid>()))
                .ReturnsAsync(orderDtoOutput);

            var handler = new UpdateOrderHandler(mockOrderService.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(orderDtoOutput);

            mockOrderService.Verify(s => s.UpdateOrderStatus(It.IsAny<Status>(), It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidOrder_ShouldNotUpdateOrder()
        {
            // Arrange
            var command = new Faker<UpdateOrderCommand>()
                .RuleFor(u => u.Id, f => f.Random.Guid())
                .RuleFor(u => u.Status, f => f.Random.Enum(Status.Pending, Status.Processed));

            var mockOrderService = new Mock<IOrderService>();
            mockOrderService
                .Setup(s => s.UpdateOrderStatus(It.IsAny<Status>(), It.IsAny<Guid>()))
                .ReturnsAsync(null as OrderDtoOutput);

            var handler = new UpdateOrderHandler(mockOrderService.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            mockOrderService.Verify(s => s.UpdateOrderStatus(It.IsAny<Status>(), It.IsAny<Guid>()), Times.Once);
        }
    }
}
