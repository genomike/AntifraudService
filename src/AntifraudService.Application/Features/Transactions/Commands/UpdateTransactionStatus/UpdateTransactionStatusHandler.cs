using AntifraudService.Application.Common.Interfaces;
using AntifraudService.Application.Features.Transactions.Commands.UpdateTransactionStatus;
using AntifraudService.Domain.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

public class UpdateTransactionStatusCommandHandler : IRequestHandler<UpdateTransactionStatusCommand, bool>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMessageProducer _messageProducer;

    public UpdateTransactionStatusCommandHandler(ITransactionRepository transactionRepository, IMessageProducer messageProducer)
    {
        _transactionRepository = transactionRepository;
        _messageProducer = messageProducer;
    }

    public async Task<bool> Handle(UpdateTransactionStatusCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.GetTransactionById(request.TransactionExternalId);

        if (transaction == null)
        {
            throw new TransactionValidationException("Transacción no encontrada.");
        }

        if (!Enum.TryParse(request.Status, out TransactionStatus status))
        {
            throw new TransactionValidationException("Estado de transacción inválido.");
        }

        transaction.Status = status;

        await _transactionRepository.UpdateTransaction(transaction);

        return true;
    }
}
