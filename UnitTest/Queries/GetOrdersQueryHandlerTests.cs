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
    public class GetOrdersQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnListOfOrders()
        {
            // Arrange
            var query = new GetOrdersQuery();
            var orders = new Faker<Order>()
                .RuleFor(o => o.Id, f => f.Random.Guid())
                .RuleFor(o => o.UserId, f => f.Random.Guid())
                .RuleFor(o => o.OrderPrice, f => f.Random.Double())
                .RuleFor(o => o.Status, f => f.Random.Enum(Status.Pending)).Generate(3);

            var mockOrderService = new Mock<IOrderService>();
            mockOrderService
                .Setup(s => s.GetOrders(It.IsAny<Guid>()))
                .ReturnsAsync(orders);

            var handler = new GetOrdersHandler(mockOrderService.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(orders.Count());

            foreach (var order in result)
            {
                var expectedOrder = orders.FirstOrDefault(o => o.Id == order.Id);
                expectedOrder.Should().BeEquivalentTo(order);
            }

            mockOrderService.Verify(s => s.GetOrders(It.IsAny<Guid>()), Times.Once);
        }
    }
}
