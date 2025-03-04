using AntifraudService.Application.Common.Interfaces;
using AntifraudService.Application.Features.Antifraud.Services;
using AntifraudService.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AntifraudService.Application.Features.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionHandler
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMessageProducer _messageProducer;
        private readonly TransactionValidationService _transactionValidationService;

        public CreateTransactionHandler(ITransactionRepository transactionRepository, IMessageProducer messageProducer, TransactionValidationService transactionValidationService)
        {
            _transactionRepository = transactionRepository;
            _messageProducer = messageProducer;
            _transactionValidationService = transactionValidationService;
        }

        public async Task<Guid> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                SourceAccountId = request.SourceAccountId,
                TargetAccountId = request.TargetAccountId,
                TransferTypeId = request.TransferTypeId,
                Value = request.Value,
                Status = TransactionStatus.Pending
            };

            _transactionRepository.AddTransaction(transaction);

            var isApproved = _transactionValidationService.ValidateTransaction(transaction);
            if (isApproved == TransactionStatus.Approved)
            {
                transaction.Status = TransactionStatus.Approved;
            }
            else
            {
                transaction.Status = TransactionStatus.Rejected;
            }

            await _transactionRepository.UpdateTransaction(transaction);
            await _messageProducer.ProduceAsync(new TransactionMessage
            {
                TransactionExternalId = transaction.Id,
                Status = transaction.Status.ToString()
            });

            return transaction.Id;
        }
    }
}