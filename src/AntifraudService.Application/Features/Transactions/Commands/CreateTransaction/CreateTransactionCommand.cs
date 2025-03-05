using MediatR;
using System;

namespace AntifraudService.Application.Features.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommand : IRequest<Guid> // o IRequest<TipoDeRetorno>
    {
        public Guid SourceAccountId { get; }
        public Guid TargetAccountId { get; }
        public int TransferTypeId { get; }
        public decimal Value { get; }

        public CreateTransactionCommand(Guid sourceAccountId, Guid targetAccountId, int transferTypeId, decimal value)
        {
            SourceAccountId = sourceAccountId;
            TargetAccountId = targetAccountId;
            TransferTypeId = transferTypeId;
            Value = value;
        }
    }
}