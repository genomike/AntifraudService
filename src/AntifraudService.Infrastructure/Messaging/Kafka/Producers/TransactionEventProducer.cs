using AntifraudService.Application.Common.Interfaces;
using Confluent.Kafka;
using System.Text.Json;
using System.Threading.Tasks;

namespace AntifraudService.Infrastructure.Messaging.Kafka.Producers
{
    public class TransactionEventProducer : IMessageProducer
    {
        private readonly IProducer<string, string> _producer;

        public TransactionEventProducer(KafkaSettings kafkaSettings)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = kafkaSettings.BootstrapServers,
                Acks = Acks.All
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public void Produce(string topic, string message)
        {
            throw new System.NotImplementedException();
        }

        public Task ProduceAsync(TransactionMessage transactionMessage)
        {
            throw new System.NotImplementedException();
        }

        public async Task ProduceTransactionEventAsync(TransactionMessage transactionMessage)
        {
            var message = new Message<string, string>
            {
                Key = transactionMessage.TransactionExternalId.ToString(),
                Value = JsonSerializer.Serialize(transactionMessage)
            };

            await _producer.ProduceAsync("transaction-events", message);
        }
    }
}