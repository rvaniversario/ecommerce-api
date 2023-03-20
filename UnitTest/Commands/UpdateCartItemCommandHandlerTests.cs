using Bogus;
using EcommerceApi.Commands;
using EcommerceApi.Dtos;
using EcommerceApi.Entities;
using EcommerceApi.Handlers;
using EcommerceApi.Services.Interfaces;
using FluentAssertions;
using Moq;
using Shouldly;

namespace UnitTest.Commands
{
    public class UpdateCartItemCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WithValidCartItem_ShouldUpdateCartItem()
        {
            // Arrange
            var command = new Faker<UpdateCartItemCommand>()
                .RuleFor(u => u.Id, f => f.Random.Guid())
                .RuleFor(u => u.Quantity, f => f.Random.Int(1,10)).Generate();

            var cartItem = new Faker<CartItem>()
                .RuleFor(c => c.Id, command.Id)
                .RuleFor(c => c.OrderId, f => f.Random.Guid())
                .RuleFor(c => c.ProductName, f => f.Commerce.ProductName())
                .RuleFor(c => c.ProductPrice, f => f.Random.Double())
                .RuleFor(c => c.ItemPrice, f => f.Random.Double())
                .RuleFor(c => c.Quantity, command.Quantity).Generate();

            var mockCartItemService = new Mock<ICartItemService>();
            mockCartItemService
                .Setup(s => s.UpdateCartItem(It.IsAny<int>(), It.IsAny<Guid>()))
                .ReturnsAsync(cartItem);

            var handler = new UpdateCartItemHandler(mockCartItemService.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(cartItem);

            mockCartItemService.Verify(s => s.UpdateCartItem(It.IsAny<int>(), It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidCartItem_ShouldNotUpdateCartItem()
        {
            // Arrange
            var command = new Faker<UpdateCartItemCommand>()
                .RuleFor(u => u.Id, f => f.Random.Guid())
                .RuleFor(u => u.Quantity, f => f.Random.Int(1, 10)).Generate();

            var mockCartItemService = new Mock<ICartItemService>();
            mockCartItemService
                .Setup(s => s.UpdateCartItem(It.IsAny<int>(), It.IsAny<Guid>()))
                .ReturnsAsync(null as CartItem);

            var handler = new UpdateCartItemHandler(mockCartItemService.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            mockCartItemService.Verify(s => s.UpdateCartItem(It.IsAny<int>(), It.IsAny<Guid>()), Times.Once);
        }
    }
}
