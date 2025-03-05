using AntifraudService.Application.Common.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AntifraudService.Infrastructure.Messaging.Kafka.Consumers
{
    public class TransactionEventConsumer : IMessageConsumer
    {
        private readonly KafkaSettings _kafkaSettings;
        private readonly ILogger<TransactionEventConsumer> _logger;

        public TransactionEventConsumer(KafkaSettings kafkaSettings, ILogger<TransactionEventConsumer> logger)
        {
            _kafkaSettings = kafkaSettings;
            _logger = logger;
        }

        public async Task ConsumeAsync(Func<string, Task> messageHandler, CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaSettings.BootstrapServers,
                GroupId = "transaction-validation-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_kafkaSettings.Topic);

            _logger.LogInformation("Started consuming from topic: {topic}", _kafkaSettings.Topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken);
                        if (consumeResult != null)
                        {
                            _logger.LogInformation("Received message: {message}", consumeResult.Message.Value);
                            await messageHandler(consumeResult.Message.Value);
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError(ex, "Error consuming Kafka message");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Consumer stopping due to cancellation");
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}