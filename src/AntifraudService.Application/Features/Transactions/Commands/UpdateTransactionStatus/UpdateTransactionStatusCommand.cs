using MediatR;
using System;

namespace AntifraudService.Application.Features.Transactions.Commands.UpdateTransactionStatus
{
    public class UpdateTransactionStatusCommand : IRequest<bool>
    {
        public Guid TransactionExternalId { get; }
        public string Status { get; }

        public UpdateTransactionStatusCommand(Guid transactionExternalId, string status)
        {
            TransactionExternalId = transactionExternalId;
            Status = status;
        }
    }
}