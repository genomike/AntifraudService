using AntifraudService.Application.Common.Interfaces;
using AntifraudService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

        public async Task AddTransaction(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetDailyTotal(Guid sourceAccountId, DateTime date)
        {
            var transactions = await _context.Transactions
                .Where(transaction => transaction.SourceAccountId == sourceAccountId && transaction.CreatedAt.Date == date.Date)
                .ToListAsync();

            return transactions.Sum(transaction => transaction.Value);
        }

        public async Task<Transaction> GetTransactionById(Guid id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task UpdateTransaction(Transaction transaction)
        {
            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }
    }
}