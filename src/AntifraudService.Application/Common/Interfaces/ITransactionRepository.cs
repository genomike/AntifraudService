using AntifraudService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace AntifraudService.Application.Common.Interfaces;

public interface ITransactionRepository
{
    Task AddTransaction(Transaction transaction);
    Task UpdateTransaction(Transaction transaction);
    Transaction GetTransactionById(Guid transactionId);
    Task<Transaction> GetTransactionByIdAsync(Guid transactionId);
    IEnumerable<Transaction> GetTransactionsByDate(DateTime date);
    decimal GetDailyTotal(Guid sourceAccountId, DateTime date);
}