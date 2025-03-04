using System;
using AntifraudService.Application.DTOs;
using MediatR;

namespace AntifraudService.Application.Features.Transactions.Queries.GetTransaction
{
    public class GetTransactionQuery : IRequest<TransactionDto>
    {
        public Guid TransactionExternalId { get; set; }
        public DateTime CreatedAt { get; set; }

        public GetTransactionQuery(Guid transactionExternalId)
        {
            TransactionExternalId = transactionExternalId;
        }

        public GetTransactionQuery(Guid transactionExternalId, DateTime createdAt)
        {
            TransactionExternalId = transactionExternalId;
            CreatedAt = createdAt;
        }
    }
}