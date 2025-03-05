using AntifraudService.Application.Common.Interfaces;
using AntifraudService.Application.Features.Antifraud.Services;
using AntifraudService.Infrastructure.Messaging.Kafka;
using AntifraudService.Infrastructure.Messaging.Kafka.Consumers;
using AntifraudService.Infrastructure.Persistence;
using AntifraudService.Infrastructure.Persistence.Repositories;
using AntifraudService.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // Configuración de base de datos
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });

        // Configuración de Kafka
        services.Configure<KafkaSettings>(hostContext.Configuration.GetSection("Kafka"));
        services.AddSingleton(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            return new KafkaSettings
            {
                BootstrapServers = configuration["Kafka:BootstrapServers"],
                Topic = configuration["Kafka:Topic"]
            };
        });

        // Registrar repositorios y servicios
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddTransient<TransactionValidationService>();
        
        // Registrar HttpClient para llamadas a la API
        services.AddHttpClient();
        
        // Registrar el consumer de Kafka
        services.AddSingleton<IMessageConsumer, TransactionEventConsumer>();
        
        // Registrar el worker
        services.AddHostedService<TransactionValidationWorker>();
    })
    .Build();

await host.RunAsync();
