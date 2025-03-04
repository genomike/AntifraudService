using System;
using System.Threading;
using System.Threading.Tasks;
using AntifraudService.Application.Common.Interfaces;
using AntifraudService.Application.Features.Transactions.Commands.UpdateTransactionStatus;
using AntifraudService.Domain.Entities;
using AntifraudService.Domain.Exceptions;

public class UpdateTransactionStatusHandler
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMessageProducer _messageProducer;

    public UpdateTransactionStatusHandler(ITransactionRepository transactionRepository, IMessageProducer messageProducer)
    {
        _transactionRepository = transactionRepository;
        _messageProducer = messageProducer;
    }

    public async Task Handle(UpdateTransactionStatusCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetTransactionByIdAsync(request.TransactionExternalId);

        if (transaction == null)
        {
            throw new TransactionValidationException("Transaction not found."); // Update the exception type
        }

        if (!Enum.TryParse(request.Status, out TransactionStatus status))
        {
            throw new TransactionValidationException("Invalid transaction status.");
        }

        transaction.Status = status;

        await _transactionRepository.UpdateTransaction(transaction);
        await _messageProducer.ProduceAsync(new TransactionMessage
        {
            TransactionExternalId = transaction.Id,
            Status = transaction.Status.ToString()
        });
    }
}
