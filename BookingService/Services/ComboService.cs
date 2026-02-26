using BookingService.Models;
using BookingService.Repositories;

namespace BookingService.Services
{
    public class ComboService : IComboService
    {
        private readonly IComboRepository _repo;

        public ComboService(IComboRepository repo) 
            => _repo = repo;

        public async Task<IEnumerable<Combo>> GetAllAsync()
        {
            var items = await _repo.GetAllAsync();
            return items.Select(c => new Combo
            {
                Id = c.Id,
                Name = c.Name,
                Price = c.Price,
                Description = c.Description
            });
        }

        public async Task<Combo?> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            if (c == null) return null;
            return new Combo
            {
                Id = c.Id,
                Name = c.Name,
                Price = c.Price,
                Description = c.Description
            };
        }

        public async Task<Combo> CreateAsync(Combo dto)
        {
            var combo = new Combo(dto.Name, dto.Price, dto.Description);
            var created = await _repo.AddAsync(combo);
            return new Combo
            {
                Id = created.Id,
                Name = created.Name,
                Price = created.Price,
                Description = created.Description
            };
        }

        public async Task<Combo?> UpdateAsync(int id, Combo dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            existing.Name = dto.Name;
            existing.Price = dto.Price;
            existing.Description = dto.Description;

            var updated = await _repo.UpdateAsync(existing);
            return new Combo
            {
                Id = updated.Id,
                Name = updated.Name,
                Price = updated.Price,
                Description = updated.Description
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
