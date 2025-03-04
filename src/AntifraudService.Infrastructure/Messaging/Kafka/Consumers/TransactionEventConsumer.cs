using AntifraudService.Application.Common.Interfaces;
using AntifraudService.Application.Features.Transactions.Commands.UpdateTransactionStatus;
using AntifraudService.Domain.Entities;
using Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AntifraudService.Infrastructure.Messaging.Kafka.Consumers
{
    public class TransactionEventConsumer
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMessageProducer _messageProducer;
        private readonly string _topic = "transaction-events";

        public TransactionEventConsumer(ITransactionRepository transactionRepository, IMessageProducer messageProducer)
        {
            _transactionRepository = transactionRepository;
            _messageProducer = messageProducer;
        }

        public void Start(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "transaction-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, TransactionMessage>(config).Build())
            {
                consumer.Subscribe(_topic);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var cr = consumer.Consume(cancellationToken);
                        ProcessMessage(cr.Message.Value);
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }
        }

        private async Task ProcessMessage(TransactionMessage message)
        {
            // Logic to validate the transaction and update its status
            var transaction = await _transactionRepository.GetTransactionByIdAsync(message.TransactionExternalId);
            if (transaction != null)
            {
                var status = ValidateTransaction(transaction);
                await _messageProducer.ProduceAsync(
                    new TransactionMessage
                    {
                        TransactionExternalId = transaction.Id,
                        Status = status.ToString()
                    });
            }
        }

        private TransactionStatus ValidateTransaction(Transaction transaction)
        {
            // Implement validation logic based on criteria
            if (transaction.Value > 2000)
            {
                return TransactionStatus.Rejected;
            }

            // Additional logic for daily accumulated value can be added here

            return TransactionStatus.Approved;
        }
    }
}