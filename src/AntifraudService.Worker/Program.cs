using AntifraudService.Application.Common.Interfaces;
using AntifraudService.Application.Features.Antifraud.Services;
using AntifraudService.Infrastructure.Messaging.Kafka;
using AntifraudService.Infrastructure.Messaging.Kafka.Consumers;
using AntifraudService.Infrastructure.Persistence;
using AntifraudService.Infrastructure.Persistence.Repositories;
using AntifraudService.Worker;
using Microsoft.EntityFrameworkCore;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });

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

        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddTransient<TransactionValidationService>();
        services.AddHttpClient();

        services.AddSingleton<IMessageConsumer, TransactionEventConsumer>();
        services.AddHostedService<TransactionValidationWorker>();
    })
    .Build();

await host.RunAsync();
