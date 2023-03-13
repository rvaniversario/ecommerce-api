using Bogus;
using EcommerceApi.Entities;
using EcommerceApi.Handlers;
using EcommerceApi.Queries;
using EcommerceApi.Services.Interfaces;
using FluentAssertions;
using Moq;

namespace UnitTest.Queries
{
    public class GetUserByIdQueryHandlerTests
    {
        [Fact]
        public async Task Handle_WithValidId_ShouldReturnUser()
        {
            // Arrange
            var query = new Faker<GetUserByIdQuery>()
                .RuleFor(g => g.Id, f => f.Random.Guid()).Generate();

            var user = new Faker<User>()
                .RuleFor(u => u.Id, query.Id)
                .RuleFor(u => u.Name, f => f.Person.FullName).Generate();


            var mockUserService = new Mock<IUserService>();
            mockUserService
                .Setup(s => s.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(user);

            var handler = new GetUserByIdHandler(mockUserService.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(user);

            mockUserService.Verify(s => s.GetById(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var query = new Faker<GetUserByIdQuery>()
                .RuleFor(g => g.Id, f => f.Random.Guid()).Generate();

            var mockUserService = new Mock<IUserService>();
            mockUserService
                .Setup(s => s.GetById(It.IsAny<Guid>()))
                .ReturnsAsync(null as User);

            var handler = new GetUserByIdHandler(mockUserService.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeNull();

            mockUserService.Verify(s => s.GetById(It.IsAny<Guid>()), Times.Once);
        }
    }
}
