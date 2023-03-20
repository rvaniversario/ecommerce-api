using Bogus;
using EcommerceApi.Entities;
using EcommerceApi.Enums;
using EcommerceApi.Handlers;
using EcommerceApi.Queries;
using EcommerceApi.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace UnitTest.Queries
{
    public class GetOrderByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_WithValidId_ShouldReturnOrder()
        {
            // Arrange

            var query = new Faker<GetOrderByIdQuery>()
                .RuleFor(g => g.Id, f => f.Random.Guid()).Generate();

            var order = new Faker<Order>()
                .RuleFor(o => o.Id, query.Id)
                .RuleFor(o => o.UserId, f => f.Random.Guid())
                .RuleFor(o => o.OrderPrice, f => f.Random.Double())
                .RuleFor(o => o.Status, f => f.Random.Enum<Status>()).Generate();

            var mockOrderService = new Mock<IOrderService>();
            mockOrderService
                .Setup(s => s.GetOrderById(It.IsAny<Guid>()))
                .ReturnsAsync(order);

            var handler = new GetOrderByIdHandler(mockOrderService.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(order);

            mockOrderService.Verify(s => s.GetOrderById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var query = new Faker<GetOrderByIdQuery>()
                .RuleFor(g => g.Id, f => f.Random.Guid()).Generate();

            var mockOrderService = new Mock<IOrderService>();
            mockOrderService
                .Setup(s => s.GetOrderById(It.IsAny<Guid>()))
                .ReturnsAsync(null as Order);

            var handler = new GetOrderByIdHandler(mockOrderService.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            mockOrderService.Verify(service => service.GetOrderById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
