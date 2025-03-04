using AntifraudService.Domain.Entities;
using AntifraudService.Domain.Exceptions;
using AntifraudService.Application.Common.Interfaces;
using System;

namespace AntifraudService.Application.Features.Antifraud.Services
{
    public class TransactionValidationService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionValidationService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public TransactionStatus ValidateTransaction(Transaction transaction)
        {
            if (transaction.Value > 2000)
            {
                return TransactionStatus.Rejected;
            }

            var dailyTotal = _transactionRepository.GetDailyTotal(transaction.SourceAccountId, transaction.CreatedAt.Date);
            if (dailyTotal + transaction.Value > 20000)
            {
                return TransactionStatus.Rejected;
            }

            return TransactionStatus.Approved;
        }
    }
}