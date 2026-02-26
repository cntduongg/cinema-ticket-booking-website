using BookingService.Models;
using BookingService.Repositories;
using Microsoft.EntityFrameworkCore;
using VNPAY.NET.Models;

namespace BookingService.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repository;
        private readonly IOrderRepository _OrderRepo;
        public TransactionService(ITransactionRepository transactionRepository, IOrderRepository OrderRepo)
        {
            _repository = transactionRepository;
            _OrderRepo = OrderRepo;
        }
        public async Task<Transaction> AddAsync(Transaction transaction)
        {
            return await _repository.AddAsync(transaction);
        }
        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<Transaction?> UpdateAsync(Transaction transaction)
        {
            return await _repository.UpdateAsync(transaction);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<List<Transaction>> GetByDateAsync(DateTime date)
        {
            var targetDate = DateOnly.FromDateTime(date); // chuyển DateTime sang DateOnly

            var transactions = await _repository.GetAllAsync();
            return transactions
                .Where(t => t != null && t.TransactionDate == targetDate)
                .ToList();
        }
        public async Task<bool> SaveTransactionAsync(PaymentResult result)
        {
            if (!result.IsSuccess) return false;

            // Check xem transaction đã tồn tại chưa
            var existing = await _repository.GetByOrderIdAsync((int)result.PaymentId);
            if (existing != null) return true;

            // Lấy order
            var order = await _OrderRepo.GetByIdAsync((int)result.PaymentId);
            if (order == null) return false;

            var transaction = new Transaction
            {
                OrderId = order.Id,
                PaymentId = 1,
                TransactionDate = DateOnly.FromDateTime(DateTime.Now),
                Price = order.TotalPrice,
                Status = true,
                TransactionCode = result.VnpayTransactionId > 0
                    ? result.VnpayTransactionId.ToString()
                    : Guid.NewGuid().ToString()
            };

            await _repository.AddAsync(transaction);

            // Cập nhật trạng thái đơn hàng
            order.Status = true;
            await _repository.SaveChangesAsync();

            return true;
        }


    }
}
