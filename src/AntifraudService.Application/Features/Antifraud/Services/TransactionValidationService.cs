using AntifraudService.Application.Common.Interfaces;
using AntifraudService.Domain.Entities;
using System.Threading.Tasks;

namespace AntifraudService.Application.Features.Antifraud.Services
{
    public class TransactionValidationService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionValidationService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionStatus> ValidateTransaction(Transaction transaction)
        {
            if (transaction.Value > 2000)
            {
                return TransactionStatus.Rejected;
            }

            var dailyTotal = await _transactionRepository.GetDailyTotal(transaction.SourceAccountId, transaction.CreatedAt.Date);
            if (dailyTotal + transaction.Value > 20000)
            {
                return TransactionStatus.Rejected;
            }

            return TransactionStatus.Approved;
        }
    }
}