using BookingService.Models;   

namespace BookingService.Repositories
{
    public interface ITransactionRepository
    {
        Task<Transaction> AddAsync(Transaction transaction);
        Task<Transaction?> GetByIdAsync(int id);
        Task<List<Transaction>> GetAllAsync();
        Task<Transaction?> UpdateAsync(Transaction transaction);
        Task<bool> DeleteAsync(int id);
        Task SaveChangesAsync();

        Task<Transaction?> GetByOrderIdAsync(int orderId);
    }
}
