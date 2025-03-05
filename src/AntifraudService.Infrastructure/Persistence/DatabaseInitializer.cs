using Microsoft.EntityFrameworkCore;
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
                _logger.LogInformation("Creando la base de datos si esta no existe");
                _context.Database.EnsureCreated();
                _logger.LogInformation("Inicializaci�n de base de datos completada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurri� un error inicializando la base de datos");
                throw;
            }
        }
    }
}