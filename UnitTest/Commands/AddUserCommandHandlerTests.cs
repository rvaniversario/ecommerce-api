using Bogus;
using EcommerceApi.Commands;
using EcommerceApi.Dtos;
using EcommerceApi.Handlers;
using EcommerceApi.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace UnitTest.Commands
{
    public class AddUserCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WithValidUser_ShouldAddUser()
        {
            // Arrange
            var command = new Faker<AddUserCommand>()
                .RuleFor(a => a.Name, f => f.Person.FullName).Generate();

            var userDtoOutput = new Faker<UserDtoOutput>()
                .RuleFor(u => u.Id, f => f.Random.Guid())
                .RuleFor(u => u.Name, command.Name).Generate();

            var mockUserService = new Mock<IUserService>();
            mockUserService
                .Setup(s => s.Add(It.IsAny<string>()))
                .ReturnsAsync(userDtoOutput);

            var handler = new AddUserHandler(mockUserService.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(userDtoOutput.Id);
            result.Name.Should().Be(userDtoOutput.Name);

            mockUserService.Verify(s => s.Add(It.IsAny<string>()), Times.Once);
        }
    }
}
