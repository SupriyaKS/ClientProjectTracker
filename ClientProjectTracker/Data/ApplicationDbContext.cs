using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ClientProjectTracker.Models;

namespace ClientProjectTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}