using BookingService.Models;
using BookingService.Repositories;

namespace BookingService.Services
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _repo;
        public FoodService(IFoodRepository repo) => _repo = repo;

        public async Task<IEnumerable<Food>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            return data.Select(x => new Food { Id = x.Id, Name = x.Name, Price = x.Price });
        }

        public async Task<Food?> GetByIdAsync(int id)
        {
            var food = await _repo.GetByIdAsync(id);
            return food == null ? null : new Food { Id = food.Id, Name = food.Name, Price = food.Price };
        }

        public async Task<Food> CreateAsync(Food dto)
        {
            var food = new Food { Name = dto.Name, Price = dto.Price };
            var created = await _repo.AddAsync(food);
            return new Food { Id = created.Id, Name = created.Name, Price = created.Price };
        }

        public async Task<Food?> UpdateAsync(int id, Food dto)
        {
            var food = await _repo.GetByIdAsync(id);
            if (food == null) return null;

            food.Name = dto.Name;
            food.Price = dto.Price;
            var updated = await _repo.UpdateAsync(food);

            return new Food { Id = updated.Id, Name = updated.Name, Price = updated.Price };
        }

        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
}
