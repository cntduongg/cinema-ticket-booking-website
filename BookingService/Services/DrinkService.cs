// Services/DrinkService.cs
using BookingService.Models;
using BookingService.Repositories;

namespace BookingService.Services
{
    public class DrinkService : IDrinkService
    {
        private readonly IDrinkRepository _repo;

        public DrinkService(IDrinkRepository repo)
            => _repo = repo;

        public async Task<IEnumerable<Drink>> GetAllAsync()
        {
            var drinks = await _repo.GetAllAsync();
            return drinks.Select(d => new Drink
            {
                Id = d.Id,
                Name = d.Name,
                Price = d.Price
            });
        }

        public async Task<Drink?> GetByIdAsync(int id)
        {
            var d = await _repo.GetByIdAsync(id);
            if (d == null) return null;
            return new Drink
            {
                Id = d.Id,
                Name = d.Name,
                Price = d.Price
            };
        }

        public async Task<Drink> CreateAsync(Drink dto)
        {
            var drink = new Drink { Name = dto.Name, Price = dto.Price };
            var created = await _repo.AddAsync(drink);
            return new Drink { Id = created.Id, Name = created.Name, Price = created.Price };
        }

        public async Task<Drink?> UpdateAsync(int id, Drink dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Name = dto.Name;
            existing.Price = dto.Price;

            var updated = await _repo.UpdateAsync(existing);
            return new Drink { Id = updated.Id, Name = updated.Name, Price = updated.Price };
        }

        public async Task<bool> DeleteAsync(int id)
            => await _repo.DeleteAsync(id);
    }
}
