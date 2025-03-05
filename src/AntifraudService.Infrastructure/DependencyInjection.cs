using AntifraudService.Application.Common.Interfaces;
using AntifraudService.Infrastructure.Messaging.Kafka;
using AntifraudService.Infrastructure.Messaging.Kafka.Producers;
using AntifraudService.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace AntifraudService.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            // Database context is configured in Startup.cs's ConfigureDatabase method
            // which handles multiple database providers

            // Register repositories and services
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddSingleton<IMessageProducer, TransactionEventProducer>();
            services.AddSingleton<KafkaSettings>();
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            // Register application services here
        }
    }
}