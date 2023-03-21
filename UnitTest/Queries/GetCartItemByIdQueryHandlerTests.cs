using Bogus;
using EcommerceApi.Entities;
using EcommerceApi.Handlers;
using EcommerceApi.Queries;
using EcommerceApi.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace UnitTest.Queries
{
    public class GetCartItemByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_WithValidId_ShouldReturnCartItem()
        {
            // Arrange

            var query = new Faker<GetCartItemByIdQuery>()
                .RuleFor(g => g.Id, f => f.Random.Guid()).Generate();

            var cartItem = new Faker<CartItem>()
                .RuleFor(o => o.Id, query.Id)
                .RuleFor(o => o.OrderId, f => f.Random.Guid())
                .RuleFor(o => o.ProductName, f => f.Commerce.ProductName())
                .RuleFor(o => o.ProductPrice, f => f.Random.Double())
                .RuleFor(o => o.ItemPrice, f => f.Random.Double())
                .RuleFor(o => o.Quantity, f => f.Random.Int()).Generate();

            var mockCartItemService = new Mock<ICartItemService>();
            mockCartItemService
                .Setup(s => s.GetCartItemById(It.IsAny<Guid>()))
                .ReturnsAsync(cartItem);

            var handler = new GetCartItemByIdHandler(mockCartItemService.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(cartItem);

            mockCartItemService.Verify(s => s.GetCartItemById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var query = new Faker<GetCartItemByIdQuery>()
                .RuleFor(g => g.Id, f => f.Random.Guid()).Generate();

            var mockCartItemService = new Mock<ICartItemService>();
            mockCartItemService
                .Setup(s => s.GetCartItemById(It.IsAny<Guid>()))
                .ReturnsAsync(null as CartItem);

            var handler = new GetCartItemByIdHandler(mockCartItemService.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            mockCartItemService.Verify(service => service.GetCartItemById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
