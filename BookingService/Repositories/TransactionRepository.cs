using Microsoft.EntityFrameworkCore;
using BookingService.Models;

namespace BookingService.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BookingDbContext _context;
        public TransactionRepository(BookingDbContext context)
        {
            _context = context;
        }
        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction == null) return false;
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _context.Transactions
                .Include(t => t.Order)
                .ToListAsync();
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _context.Transactions
                .Include(t => t.Order)
                .FirstOrDefaultAsync();
        }

        public async Task<Transaction?> UpdateAsync(Transaction transaction)
        {
            var existing = _context.Transactions.Find(transaction.Id);
            if (existing == null)
                return null;
            existing.OrderId = transaction.OrderId;
            existing.PaymentId = transaction.PaymentId;
            existing.TransactionDate = transaction.TransactionDate;
            existing.Price = transaction.Price;
            existing.Status = transaction.Status;
            await _context.SaveChangesAsync();
            return existing;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<Transaction?> GetByOrderIdAsync(int orderId)
        {
            return await _context.Transactions
                .FirstOrDefaultAsync(t => t.OrderId == orderId);
        }
    }
}
