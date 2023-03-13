using Bogus;
using EcommerceApi.Commands;
using EcommerceApi.Dtos;
using EcommerceApi.Handlers;
using EcommerceApi.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace UnitTest.Commands
{
    public class AddCartItemCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WithValidCartItem_ShouldAddCartItem()
        {
            // Arrange

            var command = new Faker<AddCartItemCommand>()
                .RuleFor(a => a.UserId, f => f.Random.Guid())
                .RuleFor(a => a.ProductName, f => f.Commerce.ProductName())
                .RuleFor(a => a.ProductPrice, f => f.Random.Double(1,100))
                .RuleFor(a => a.Quantity, f => f.Random.Int(1,20)).Generate();

            var cartItemDtoOutput = new Faker<CartItemDtoOutput>()
                .RuleFor(c => c.Id, f => f.Random.Guid())
                .RuleFor(c => c.OrderId, f => f.Random.Guid())
                .RuleFor(c => c.ProductName, command.ProductName)
                .RuleFor(c => c.ProductPrice, command.ProductPrice)
                .RuleFor(c => c.ItemPrice, command.ProductPrice * command.Quantity)
                .RuleFor(c => c.Quantity, command.Quantity).Generate();

            var mockCartItemService = new Mock<ICartItemService>();

            mockCartItemService
                .Setup(s => s.Add(
                    It.IsAny<Guid>(),
                    It.IsAny<string>(),
                    It.IsAny<double>(),
                    It.IsAny<int>()))
                .ReturnsAsync(cartItemDtoOutput);

            var handler = new AddCartItemHandler(mockCartItemService.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(cartItemDtoOutput.Id);
            result.OrderId.Should().Be(cartItemDtoOutput.OrderId);
            result.ProductName.Should().Be(cartItemDtoOutput.ProductName);
            result.ProductPrice.Should().Be(cartItemDtoOutput.ProductPrice);
            result.ItemPrice.Should().Be(cartItemDtoOutput.ItemPrice);
            result.Quantity.Should().Be(cartItemDtoOutput.Quantity);

            mockCartItemService.Verify(s => s.Add(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<double>(), It.IsAny<int>()), Times.Once);
        }
    }
}
