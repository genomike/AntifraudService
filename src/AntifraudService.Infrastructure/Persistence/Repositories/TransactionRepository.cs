using AntifraudService.Application.Common.Interfaces;
using AntifraudService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AntifraudService.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task AddTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public async Task<Transaction> AddTransactionAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public decimal GetDailyTotal(Guid sourceAccountId, DateTime date)
        {
            throw new NotImplementedException();
        }

        public Transaction GetTransactionById(Guid transactionId)
        {
            throw new NotImplementedException();
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public IEnumerable<Transaction> GetTransactionsByDate(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task UpdateTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }
    }
}