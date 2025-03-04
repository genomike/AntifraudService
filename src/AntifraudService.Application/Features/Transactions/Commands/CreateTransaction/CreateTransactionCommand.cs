using System;

namespace AntifraudService.Application.Features.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommand
    {
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TransferTypeId { get; set; }
        public decimal Value { get; set; }

        public CreateTransactionCommand(Guid sourceAccountId, Guid targetAccountId, int transferTypeId, decimal value)
        {
            SourceAccountId = sourceAccountId;
            TargetAccountId = targetAccountId;
            TransferTypeId = transferTypeId;
            Value = value;
        }
    }
}