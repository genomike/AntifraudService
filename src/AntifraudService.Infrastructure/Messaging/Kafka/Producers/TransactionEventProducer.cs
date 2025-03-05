using AntifraudService.Application.Common.Interfaces;
using Confluent.Kafka;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace AntifraudService.Infrastructure.Messaging.Kafka.Producers
{
    public class TransactionEventProducer : IMessageProducer
    {
        private readonly IProducer<string, string> _producer;
        private readonly string _topic;

        public TransactionEventProducer(KafkaSettings kafkaSettings)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = kafkaSettings.BootstrapServers,
                ClientId = "antifraud-service",
                Acks = Acks.Leader,
                MessageSendMaxRetries = 0,
                EnableIdempotence = false
            };

            _topic = kafkaSettings.Topic;
            _producer = new ProducerBuilder<string, string>(config)
                .SetErrorHandler((_, e) => Console.WriteLine($"Kafka error: {e.Reason}"))
                .Build();
        }

        public Task Produce(TransactionMessage transactionMessage)
        {
            return ProduceTransactionEventAsync(transactionMessage);
        }

        public async Task ProduceTransactionEventAsync(TransactionMessage transactionMessage)
        {
            var message = new Message<string, string>
            {
                Key = transactionMessage.TransactionExternalId.ToString(),
                Value = JsonSerializer.Serialize(transactionMessage)
            };

            await _producer.ProduceAsync(_topic, message);
        }
    }
}