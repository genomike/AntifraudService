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
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddSingleton<IMessageProducer, TransactionEventProducer>();
            services.AddSingleton<KafkaSettings>();
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            
        }
    }
}