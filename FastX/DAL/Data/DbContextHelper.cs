using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace FastX.DAL.Helpers
{
    public static class DbContextHelper
    {
        public static ApplicationDbContext GetDbContext()
        {
            // Build configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Important for CLI
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            // Get connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Build DbContextOptions
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Return usable context
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
