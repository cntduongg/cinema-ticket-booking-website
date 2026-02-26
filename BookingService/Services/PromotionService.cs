using BookingService.Models;
using BookingService.Models.DTOs;
using BookingService.Repositories;

namespace BookingService.Services
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _repository;

        public PromotionService(IPromotionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Promotion>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Promotion> GetByIdAsync(int id)
        {
            var promotion = await _repository.GetByIdAsync(id);
            if (promotion == null)
                throw new KeyNotFoundException("Promotion not found or inactive.");
            return promotion;
        }

        public async Task CreateAsync(Promotion promotion)
        {
            if (promotion == null)
                throw new ArgumentNullException(nameof(promotion));

            // Kiểm tra thời gian hợp lệ (StartTime < EndTime)
            if (promotion.StartTime >= promotion.EndTime)
                throw new ArgumentException("StartTime must be before EndTime.");

            await _repository.AddAsync(promotion);
        }

        public async Task<Promotion> UpdateAsync(int id, Promotion promotion)
        {
            if (promotion == null)
                throw new ArgumentNullException(nameof(promotion));

            // Kiểm tra thời gian hợp lệ
            if (promotion.StartTime >= promotion.EndTime)
                throw new ArgumentException("StartTime must be before EndTime.");

            var existingPromotion = await _repository.GetByIdAsync(id);
            if (existingPromotion == null)
                throw new KeyNotFoundException("Promotion not found or inactive.");

            return await _repository.UpdateAsync(promotion);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
