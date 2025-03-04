using System;

namespace AntifraudService.Application.Features.Transactions.Commands.UpdateTransactionStatus
{
    public class UpdateTransactionStatusCommand
    {
        public Guid TransactionExternalId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }

        public UpdateTransactionStatusCommand(Guid transactionExternalId, DateTime createdAt, string status)
        {
            TransactionExternalId = transactionExternalId;
            CreatedAt = createdAt;
            Status = status;
        }
    }
}