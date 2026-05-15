using Xunit;
using Microsoft.EntityFrameworkCore;
using ClientProjectTracker.Services;
using ClientProjectTracker.Data;
using ClientProjectTracker.DTOs;
using ClientProjectTracker.Models;

namespace ClientProjectTracker.Tests.Services
{
    public class ClientServiceTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_ShouldAddClient()
        {
            // Arrange
            var context = GetDbContext();
            var service = new ClientService(context);

            var dto = new ClientDto
            {
                Name = "Test Client",
                Email = "test@test.com",
                Phone = "9999999999",
                CompanyName = "Test Company"
            };

            var userId = "user123";

            // Act
            await service.CreateAsync(dto, userId);

            // Assert
            var clients = await context.Clients.ToListAsync();

            Assert.Single(clients);
            Assert.Equal("Test Client", clients[0].Name);
        }
        [Fact]
        public async Task UpdateAsync_ShouldModifyClient()
        {
            // Arrange
            var context = GetDbContext();

            var client = new Client
            {
                Name = "Old Name",
                Email = "old@test.com",
                Phone = "1111111111",
                CompanyName = "Old Company",
                OwnerId = "user1"
            };

            context.Clients.Add(client);
            await context.SaveChangesAsync();

            var service = new ClientService(context);

            var dto = new ClientDto
            {
                Name = "New Name",
                Email = "new@test.com",
                Phone = "2222222222",
                CompanyName = "New Company"
            };

            // Act
            await service.UpdateAsync(client.Id, dto);

            // Assert
            var updatedClient = await context.Clients.FindAsync(client.Id);

            Assert.NotNull(updatedClient);
            Assert.Equal("New Name", updatedClient!.Name);
            Assert.Equal("new@test.com", updatedClient.Email);
        }
        [Fact]
        public async Task DeleteAsync_ShouldRemoveClient()
        {
            // Arrange
            var context = GetDbContext();

            var client = new Client
            {
                Name = "Delete Test",
                Email = "delete@test.com",
                Phone = "3333333333",
                CompanyName = "Delete Company",
                OwnerId = "user1"
            };

            context.Clients.Add(client);
            await context.SaveChangesAsync();

            var service = new ClientService(context);

            // Act
            await service.DeleteAsync(client.Id);

            // Assert
            var clients = await context.Clients.ToListAsync();

            Assert.Empty(clients);
        }
        [Fact]
        public async Task GetClientsAsync_ShouldReturnUserClients()
        {
            // Arrange
            var context = GetDbContext();

            context.Clients.AddRange(
                new Client
                {
                    Name = "Client 1",
                    Email = "c1@test.com",
                    CompanyName = "Company1",
                    OwnerId = "user1"
                },
                new Client
                {
                    Name = "Client 2",
                    Email = "c2@test.com",
                    CompanyName = "Company2",
                    OwnerId = "user2"
                }
            );

            await context.SaveChangesAsync();

            var service = new ClientService(context);

            // Act
            var (clients, total) = await service.GetClientsAsync(
                "user1",
                false,
                null,
                1,
                10
            );

            // Assert
            Assert.Single(clients);
            Assert.Equal("Client 1", clients[0].Name);
            Assert.Equal(1, total);
        }

        [Fact]
        public async Task GetClientsAsync_ShouldReturnPagedResults()
        {
            // Arrange
            var context = GetDbContext();

            context.Clients.AddRange(
                new Client { Name = "Client1", Email = "c1@test.com", CompanyName = "C1", OwnerId = "user1" },
                new Client { Name = "Client2", Email = "c2@test.com", CompanyName = "C2", OwnerId = "user1" },
                new Client { Name = "Client3", Email = "c3@test.com", CompanyName = "C3", OwnerId = "user1" }
            );

            await context.SaveChangesAsync();

            var service = new ClientService(context);

            // Act
            var (clients, total) = await service.GetClientsAsync("user1", false, null, 1, 2);

            // Assert
            Assert.Equal(2, clients.Count);
            Assert.Equal(3, total);
        }
    }
}