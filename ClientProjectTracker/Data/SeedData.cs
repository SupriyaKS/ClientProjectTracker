using ClientProjectTracker.Models;
using Microsoft.AspNetCore.Identity;

namespace ClientProjectTracker.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            if (context.Clients.Any())
                return;

            var user = await userManager.FindByEmailAsync("admin@test.com");

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = "admin@test.com",
                    Email = "admin@test.com"
                };

                await userManager.CreateAsync(user, "Admin@123");
            }

            var clients = new List<Client>
            {
                new Client
                {
                    Name = "John Smith",
                    Email = "john@company.com",
                    Phone = "9991112222",
                    CompanyName = "TechSoft",
                    OwnerId = user.Id
                },
                new Client
                {
                    Name = "Sarah Lee",
                    Email = "sarah@company.com",
                    Phone = "8883334444",
                    CompanyName = "Bright Solutions",
                    OwnerId = user.Id
                },
                new Client
                {
                    Name = "David Brown",
                    Email = "david@company.com",
                    Phone = "7775556666",
                    CompanyName = "CloudWorks",
                    OwnerId = user.Id
                },
                new Client
                {
                    Name = "Emily Clark",
                    Email = "emily@company.com",
                    Phone = "6661113333",
                    CompanyName = "NextGen Apps",
                    OwnerId = user.Id
                },
                new Client
                {
                    Name = "Michael Scott",
                    Email = "michael@dundermifflin.com",
                    Phone = "5558887777",
                    CompanyName = "Dunder Mifflin",
                    OwnerId = user.Id
                }
            };

            context.Clients.AddRange(clients);
            await context.SaveChangesAsync();
        }
    }
}