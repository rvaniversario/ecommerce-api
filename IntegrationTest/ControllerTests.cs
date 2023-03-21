using Bogus;
using EcommerceApi.Commands;
using EcommerceApi.Context;
using EcommerceApi.Entities;
using EcommerceApi.Enums;
using FluentAssertions;
using IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;

namespace IntegrationTest
{
    public class ControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly AppDbContext _context;
        private readonly HttpClient _client;

        public ControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _context = _factory.Services.GetService<AppDbContext>();
            _client = _factory.CreateClient();

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task Test_GetUser_ShouldReturnOk_And_ExpectedUser()
        {
            // Arrange
            var expectedUser = SeedUsers().First();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/users/{expectedUser.Id}");
            request.Headers.Add("x-user-id", expectedUser.Id.ToString());

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var parsedContent = JsonConvert.DeserializeObject<User>(content);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
            parsedContent.Should().BeEquivalentTo(expectedUser);
        }

        [Fact]
        public async Task Test_AddUser_ShouldReturnOk_And_AddedUser()
        {
            // Arrange
            var requestBody = new Faker<AddUserCommand>()
                .RuleFor(x => x.Name, f => f.Person.FullName).Generate();

            var addedUser = new User
            {
                Name = requestBody.Name
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/users");
            request.Content = ContentHelper.GetStringContent(requestBody);

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var parsedContent = JsonConvert.DeserializeObject<User>(content);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
            parsedContent.Id.Should().NotBeEmpty();
            parsedContent.Should().BeEquivalentTo(addedUser, o => o.Excluding(x => x.Id));
        }

        [Fact]
        public async Task Test_GetOrders_ShouldReturnOk()
        {
            // Arrange
            var user = SeedUsers().First();
            var order = SeedOrders(user.Id);
            SeedCartItems(order.Id);

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/orders");
            request.Headers.Add("x-user-id", user.Id.ToString());

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Test_GetOrderById_ShouldReturnOk_And_ExpectedOrder()
        {
            // Arrange
            var user = SeedUsers().First();
            var expectedOrder = SeedOrders(user.Id);
            SeedCartItems(expectedOrder.Id);

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/orders/{expectedOrder.Id}");
            request.Headers.Add("x-user-id", user.Id.ToString());

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var parsedContent = JsonConvert.DeserializeObject<Order>(content);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
            parsedContent.Should().BeEquivalentTo(expectedOrder, o => o.Excluding(x => x.CartItems));
        }

        [Fact]
        public async Task Test_GetOrderById_ShouldReturnNotFound()
        {
            // Arrange
            var user = SeedUsers().First();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/orders/{Guid.NewGuid()}");
            request.Headers.Add("x-user-id", user.Id.ToString());

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Test_UpdateOrder_ShouldReturnOk_And_UpdatedOrder()
        {
            // Arrange
            var user = SeedUsers().First();
            var order = SeedOrders(user.Id);

            var requestBody = new Faker<UpdateOrderCommand>()
                .RuleFor(x => x.Id, order.Id)
                .RuleFor(u => u.Status, f => f.Random.Enum(Status.Pending, Status.Processed)).Generate();

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/orders/{order.Id}");
            request.Headers.Add("x-user-id", user.Id.ToString());
            request.Content = ContentHelper.GetStringContent(requestBody);

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var parsedContent = JsonConvert.DeserializeObject<Order>(content);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
            parsedContent.Should().BeEquivalentTo(order);
        }

        [Fact]
        public async Task Test_DeleteOrder_ShouldReturnOk_And_DeletedOrder()
        {
            // Arrange
            var user = SeedUsers().First();
            var order = SeedOrders(user.Id);

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/orders/{order.Id}");
            request.Headers.Add("x-user-id", user.Id.ToString());

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var parsedContent = JsonConvert.DeserializeObject<Order>(content);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
            parsedContent.Should().BeEquivalentTo(order);
        }

        [Fact]
        public async Task Test_GetCartItems_ShouldReturnOk_And_ListOfCartItems()
        {
            // Arrange
            var user = SeedUsers().First();
            var order = SeedOrders(user.Id);
            var cartItems = SeedCartItems(order.Id);

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/cart-items");
            request.Headers.Add("x-user-id", order.UserId.ToString());

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var parsedContent = JsonConvert.DeserializeObject<List<CartItem>>(content);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
            parsedContent.Should().BeEquivalentTo(cartItems);
        }

        [Fact]
        public async Task Test_GetCartItemById_ShouldReturnOk_And_ExpectedCartItem()
        {
            // Arrange
            var user = SeedUsers().First();
            var order = SeedOrders(user.Id);
            var expectedCartItem = SeedCartItems(order.Id).First();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/cart-items/{expectedCartItem.Id}");
            request.Headers.Add("x-user-id", user.Id.ToString());

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var parsedContent = JsonConvert.DeserializeObject<CartItem>(content);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
            parsedContent.Should().BeEquivalentTo(expectedCartItem);
        }

        [Fact]
        public async Task Test_GetCartItemById_ShouldReturnNotFound()
        {
            // Arrange
            var user = SeedUsers().First();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/api/v1/cart-items/{Guid.NewGuid()}");
            request.Headers.Add("x-user-id", user.Id.ToString());

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Test_AddCartItem_ShouldReturnOk_And_AddedCartItem()
        {
            // Arrange
            var user = SeedUsers().First();

            var requestBody = new Faker<AddCartItemCommand>()
                .RuleFor(x => x.UserId, user.Id)
                .RuleFor(x => x.ProductName, f => f.Commerce.ProductName())
                .RuleFor(x => x.ProductPrice, f => f.Random.Double(1.99, 99.99))
                .RuleFor(x => x.Quantity, f => f.Random.Int(1, 5)).Generate();

            requestBody.ProductPrice = Math.Round(requestBody.ProductPrice, 2);

            var expectedCartItem = new CartItem
            {
                ProductName = requestBody.ProductName,
                ProductPrice = requestBody.ProductPrice,
                ItemPrice = requestBody.ProductPrice * requestBody.Quantity,
                Quantity = requestBody.Quantity,
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/cart-items");
            request.Headers.Add("x-user-id", user.Id.ToString());
            request.Content = ContentHelper.GetStringContent(requestBody);

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var parsedContent = JsonConvert.DeserializeObject<CartItem>(content);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
            parsedContent.Id.Should().NotBeEmpty();
            parsedContent.ProductName.Should().Be(expectedCartItem.ProductName);
            parsedContent.ProductPrice.Should().Be(expectedCartItem.ProductPrice);
            parsedContent.ItemPrice.Should().Be(expectedCartItem.ItemPrice);
            parsedContent.Quantity.Should().Be(expectedCartItem.Quantity);
        }

        [Fact]
        public async Task Test_UpdateCartItem_ShouldReturnOk_And_UpdatedCartItem()
        {
            // Arrange
            var user = SeedUsers().First();
            var order = SeedOrders(user.Id);
            var cartItem = SeedCartItems(order.Id).First();

            var requestBody = new Faker<UpdateCartItemCommand>()
                .RuleFor(x => x.Id, cartItem.Id)
                .RuleFor(x => x.Quantity, f => f.Random.Int(1, 5)).Generate();

            var updatedCartItem = new CartItem
            {
                Id = cartItem.Id,
                OrderId = cartItem.OrderId,
                ProductName = cartItem.ProductName,
                ProductPrice = cartItem.ProductPrice,
                ItemPrice = cartItem.ProductPrice * requestBody.Quantity,
                Quantity = requestBody.Quantity,
            };

            updatedCartItem.ItemPrice = Math.Round(updatedCartItem.ItemPrice, 2);

            var request = new HttpRequestMessage(HttpMethod.Put, $"/api/v1/cart-items/{cartItem.Id}");
            request.Headers.Add("x-user-id", user.Id.ToString());
            request.Content = ContentHelper.GetStringContent(requestBody);

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var parsedContent = JsonConvert.DeserializeObject<CartItem>(content);

            parsedContent.ItemPrice = Math.Round(parsedContent.ItemPrice, 2);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
            parsedContent.Id.Should().NotBeEmpty();
            parsedContent.OrderId.Should().NotBeEmpty();
            parsedContent.ProductName.Should().Be(updatedCartItem.ProductName);
            parsedContent.ProductPrice.Should().Be(updatedCartItem.ProductPrice);
            parsedContent.ItemPrice.Should().Be(updatedCartItem.ItemPrice);
            parsedContent.Quantity.Should().Be(updatedCartItem.Quantity);
        }

        [Fact]
        public async Task Test_DeleteCartItem_ShouldReturnOk_And_DeletedCartItem()
        {
            // Arrange
            var user = SeedUsers().First();
            var order = SeedOrders(user.Id);
            var cartItem = SeedCartItems(order.Id).First();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"/api/v1/cart-items/{cartItem.Id}");
            request.Headers.Add("x-user-id", user.Id.ToString());

            // Act
            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var parsedContent = JsonConvert.DeserializeObject<CartItem>(content);

            // Assert
            response.Should().HaveStatusCode(HttpStatusCode.OK);
            parsedContent.Should().BeEquivalentTo(cartItem);
        }

        private List<User> SeedUsers()
        {
            var users = new Faker<User>()
                .RuleFor(x => x.Name, f => f.Person.FullName).Generate(3);

            _context.Users.AddRange(users);
            _context.SaveChanges();

            return users;
        }

        private Order SeedOrders(Guid userId)
        {
            var order = new Faker<Order>()
                .RuleFor(x => x.UserId, f => userId)
                .RuleFor(x => x.OrderPrice, f => f.Random.Double())
                .RuleFor(x => x.Status, f => f.Random.Enum(Status.Processed, Status.Cancelled)).Generate();

            _context.Orders.AddRange(order);
            _context.SaveChanges();

            return order;
        }

        private List<CartItem> SeedCartItems(Guid orderId)
        {
            var cartItems = new Faker<CartItem>()
                .RuleFor(x => x.OrderId, orderId)
                .RuleFor(x => x.ProductName, f => f.Commerce.ProductName())
                .RuleFor(x => x.ProductPrice, f => f.Random.Double())
                .RuleFor(x => x.ItemPrice, f => f.Random.Double())
                .RuleFor(x => x.Quantity, f => f.Random.Int()).Generate(3);

            _context.CartItems.AddRange(cartItems);
            _context.SaveChanges();

            return cartItems;
        }
    }
}
