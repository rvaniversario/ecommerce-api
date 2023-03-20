using Bogus;
using EcommerceApi.Commands;
using EcommerceApi.Dtos;
using EcommerceApi.Entities;
using EcommerceApi.Handlers;
using EcommerceApi.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace UnitTest.Commands
{
    public class DeleteCartItemCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WithValidCartItem_ShouldDeleteCartItem()
        {
            // Arrange
            var command = new Faker<DeleteCartItemCommand>()
                .RuleFor(d => d.Id, f => f.Random.Guid()).Generate();

            var cartItem = new Faker<CartItem>()
                .RuleFor(c => c.Id, command.Id)
                .RuleFor(c => c.OrderId, f => f.Random.Guid())
                .RuleFor(c => c.ProductName, f => f.Commerce.ProductName())
                .RuleFor(c => c.ProductPrice, f => f.Random.Double())
                .RuleFor(c => c.ItemPrice, f => f.Random.Double())
                .RuleFor(c => c.Quantity, f => f.Random.Int()).Generate();

            var mockCartItemService = new Mock<ICartItemService>();
            mockCartItemService
                .Setup(s => s.DeleteCartItem(It.IsAny<Guid>()))
                .ReturnsAsync(cartItem);

            var handler = new DeleteCartItemHandler(mockCartItemService.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(cartItem.Id);
            result.OrderId.Should().Be(cartItem.OrderId);
            result.ProductName.Should().Be(cartItem.ProductName);
            result.ProductPrice.Should().Be(cartItem.ProductPrice);
            result.ItemPrice.Should().Be(cartItem.ItemPrice);
            result.Quantity.Should().Be(cartItem.Quantity);

            mockCartItemService.Verify(s => s.DeleteCartItem(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidCartItem_ShouldNotDeleteCartItem()
        {
            // Arrange
            var command = new Faker<DeleteCartItemCommand>()
                .RuleFor(d => d.Id, f => f.Random.Guid()).Generate();

            var mockCartItemService = new Mock<ICartItemService>();
            mockCartItemService
                .Setup(s => s.DeleteCartItem(It.IsAny<Guid>()))
                .ReturnsAsync(null as CartItem);

            var handler = new DeleteCartItemHandler(mockCartItemService.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            mockCartItemService.Verify(s => s.DeleteCartItem(It.IsAny<Guid>()), Times.Once);
        }
    }
}
