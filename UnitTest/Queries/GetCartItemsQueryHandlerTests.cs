using Bogus;
using EcommerceApi.Entities;
using EcommerceApi.Handlers;
using EcommerceApi.Queries;
using EcommerceApi.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace UnitTest.Queries
{
    public class GetCartItemsQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnListOfCartItems()
        {
            // Arrange
            var cartItems = new Faker<CartItem>()
                .RuleFor(c => c.Id, f => f.Random.Guid())
                .RuleFor(c => c.OrderId, f => f.Random.Guid())
                .RuleFor(c => c.ProductName, f => f.Commerce.ProductName())
                .RuleFor(c => c.ProductPrice, f => f.Random.Double())
                .RuleFor(c => c.ItemPrice, f => f.Random.Double())
                .RuleFor(c => c.Quantity, f => f.Random.Int()).Generate(3);

            var query = new GetCartItemsQuery();

            var mockCartItemService = new Mock<ICartItemService>();
            mockCartItemService
                .Setup(s => s.GetCartItems(It.IsAny<Guid>()))
                .ReturnsAsync(cartItems);

            var handler = new GetCartItemsHandler(mockCartItemService.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(cartItems.Count());

            foreach (var cartItem in result)
            {
                var expectedCartItem = cartItems.FirstOrDefault(c => c.Id == cartItem.Id);
                expectedCartItem.Should().NotBeNull();
                expectedCartItem.Should().BeEquivalentTo(cartItem);
            }

            mockCartItemService.Verify(s => s.GetCartItems(It.IsAny<Guid>()), Times.Once);
        }
    }
}
