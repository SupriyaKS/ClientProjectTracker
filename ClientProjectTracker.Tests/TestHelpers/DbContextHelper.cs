using Microsoft.EntityFrameworkCore;
using ClientProjectTracker.Data;

namespace ClientProjectTracker.Tests.TestHelpers
{
    public static class DbContextHelper
    {
        public static ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}