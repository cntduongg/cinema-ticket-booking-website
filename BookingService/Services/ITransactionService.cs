using BookingService.Models;
using VNPAY.NET.Models;

namespace BookingService.Services
{
    public interface ITransactionService
    {
        Task<Transaction> AddAsync(Transaction transaction);
        Task<Transaction?> GetByIdAsync(int id);
        Task<List<Transaction?>> GetAllAsync();
        Task<Transaction?> UpdateAsync(Transaction transaction);
        Task<bool> DeleteAsync(int id);

        Task<List<Transaction?>> GetByDateAsync(DateTime date);
        Task<bool> SaveTransactionAsync(PaymentResult result);

    }
}
