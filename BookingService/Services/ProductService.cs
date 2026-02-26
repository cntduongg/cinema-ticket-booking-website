using BookingService.Models;
using BookingService.Repositories;

namespace BookingService.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<OrderProduct> CreateAsync(OrderProduct product)
        {
            return await _repository.AddAsync(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<List<OrderProduct>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<OrderProduct?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<OrderProduct?> UpdateAsync(OrderProduct product)
        {
            return await _repository.UpdateAsync(product);
        }
    }
}
