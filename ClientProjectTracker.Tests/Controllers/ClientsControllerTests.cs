using Xunit;
using Moq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ClientProjectTracker.Controllers;
using ClientProjectTracker.Services;
using ClientProjectTracker.Models;
using ClientProjectTracker.DTOs;

namespace ClientProjectTracker.Tests.Controllers
{
    public class ClientsControllerTests
    {
        [Fact]
        public async Task Index_ShouldReturnViewWithClients()
        {
            // Arrange
            var mockService = new Mock<IClientService>();

            var clients = new List<Client>
            {
                new Client { Id = 1, Name = "Test Client", Email = "test@test.com" }
            };

            mockService.Setup(s => s.GetClientsAsync(
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((clients, 1));

            // Mock UserManager
            var userStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new Mock<UserManager<IdentityUser>>(
                userStore.Object,
                null, null, null, null, null, null, null, null
            );

            var controller = new ClientsController(
                mockService.Object,
                userManager.Object
            );

            // Fake logged in user
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user123")
            }, "mock"));

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                       .ReturnsAsync(new IdentityUser { Id = "user123" });

            // Act
            var result = await controller.Index(null, 1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Client>>(viewResult.Model);

            Assert.Single(model);
        }

        [Fact]
        public async Task Create_ShouldRedirectToIndex_WhenModelIsValid()
        {
            // Arrange
            var mockService = new Mock<IClientService>();

            var userStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new Mock<UserManager<IdentityUser>>(
                userStore.Object,
                null, null, null, null, null, null, null, null
            );

            var controller = new ClientsController(
                mockService.Object,
                userManager.Object
            );

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user123")
            }, "mock"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            userManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                    .ReturnsAsync(new IdentityUser { Id = "user123" });

            var dto = new ClientDto
            {
                Name = "Client1",
                Email = "test@test.com",
                Phone = "9999999999",
                CompanyName = "Test Company"
            };

            // Act
            var result = await controller.Create(dto);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            mockService.Verify(
                s => s.CreateAsync(It.IsAny<ClientDto>(), "user123"),
                Times.Once
            );
        }
        [Fact]
        public async Task Delete_ShouldRedirectToIndex()
        {
            // Arrange
            var mockService = new Mock<IClientService>();

            var userStore = new Mock<IUserStore<IdentityUser>>();
            var userManager = new Mock<UserManager<IdentityUser>>(
                userStore.Object,
                null, null, null, null, null, null, null, null
            );

            var controller = new ClientsController(
                mockService.Object,
                userManager.Object
            );

            // Act
            var result = await controller.Delete(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            mockService.Verify(s => s.DeleteAsync(1), Times.Once);
        }
    }
}