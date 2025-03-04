using Microsoft.Extensions.Logging;
using System;

namespace AntifraudService.Infrastructure.Persistence
{
    public class DatabaseInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(ApplicationDbContext context, ILogger<DatabaseInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Initialize()
        {
            try
            {
                _logger.LogInformation("Creating database if it doesn't exist");

                // This creates the database with the schema defined in your DbContext
                _context.Database.EnsureCreated();

                // You can add seed data here if needed
                // SeedData();

                _logger.LogInformation("Database initialization completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the database");
                throw;
            }
        }

        // private void SeedData()
        // {
        //     // Add initial data if needed
        // }
    }
}