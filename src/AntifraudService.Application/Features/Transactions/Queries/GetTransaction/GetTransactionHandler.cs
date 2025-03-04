using System.Threading;
using System.Threading.Tasks;
using AntifraudService.Application.Common.Interfaces;
using AntifraudService.Application.DTOs;
using MediatR;

namespace AntifraudService.Application.Features.Transactions.Queries.GetTransaction
{
    public class GetTransactionHandler : IRequestHandler<GetTransactionQuery, TransactionDto>
    {
        private readonly ITransactionRepository _transactionRepository;

        public GetTransactionHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionDto> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(request.TransactionExternalId);
            if (transaction == null)
            {
                return null;
            }

            return new TransactionDto
            {
                Id = transaction.Id,
                SourceAccountId = transaction.SourceAccountId,
                TargetAccountId = transaction.TargetAccountId,
                TransferTypeId = transaction.TransferTypeId,
                Value = transaction.Value,
                Status = transaction.Status.ToString(),
                CreatedAt = transaction.CreatedAt
            };
        }
    }
}