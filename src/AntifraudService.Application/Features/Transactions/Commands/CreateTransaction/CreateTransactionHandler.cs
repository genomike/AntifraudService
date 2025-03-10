using AntifraudService.Application.Common.Interfaces;
using AntifraudService.Application.Features.Antifraud.Services;
using AntifraudService.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AntifraudService.Application.Features.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Guid>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMessageProducer _messageProducer;
        private readonly TransactionValidationService _transactionValidationService;
        private readonly ILogger<CreateTransactionCommandHandler> _logger;

        public CreateTransactionCommandHandler(
            ITransactionRepository transactionRepository,
            IMessageProducer messageProducer,
            TransactionValidationService transactionValidationService,
            ILogger<CreateTransactionCommandHandler> logger)
        {
            _transactionRepository = transactionRepository;
            _messageProducer = messageProducer;
            _transactionValidationService = transactionValidationService;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction
            {
                SourceAccountId = request.SourceAccountId,
                TargetAccountId = request.TargetAccountId,
                TransferTypeId = request.TransferTypeId,
                Value = request.Value,
                CreatedAt = DateTime.UtcNow
            };

            await _transactionRepository.AddTransaction(transaction);

            try
            {
                await _messageProducer.Produce(new TransactionMessage
                {
                    TransactionExternalId = transaction.Id,
                    SourceAccountId = transaction.SourceAccountId,
                    Value = transaction.Value,
                    CreatedAt = transaction.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Hubieron errores produciendo el mensaje para Kafka, pero la transacción se guardo en la BD");
            }

            return transaction.Id;
        }
    }
}