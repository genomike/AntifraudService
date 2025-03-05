using AntifraudService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace AntifraudService.Application.Common.Interfaces;

public interface ITransactionRepository
{
    Task AddTransaction(Transaction transaction);
    Task UpdateTransaction(Transaction transaction);
    Task<Transaction> GetTransactionById(Guid transactionId);
    Task<decimal> GetDailyTotal(Guid sourceAccountId, DateTime date);
}