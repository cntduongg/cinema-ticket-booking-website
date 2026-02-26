using BookingManagement.Api.Models.DTOs.TicketSeat;
using BookingManagement.Api.Models.Entities;
using BookingManagement.Api.Repositories.IRepository;
using BookingManagement.Api.Services.IService;

namespace BookingManagement.Api.Services
{
    public class TicketSeatService : ITicketSeatService
    {
        private readonly ITicketSeatRepository _repo;
        public TicketSeatService(ITicketSeatRepository repo) => _repo = repo;

        public async Task<IEnumerable<TicketSeatDto>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            return data.Select(x => new TicketSeatDto
            {
                Id = x.Id,
                TicketId = x.TicketId,
                SeatId = x.SeatId,
                Price = x.Price
            });
        }

        public async Task<TicketSeatDto?> GetByIdAsync(int id)
        {
            var x = await _repo.GetByIdAsync(id);
            if (x == null) return null;
            return new TicketSeatDto
            {
                Id = x.Id,
                TicketId = x.TicketId,
                SeatId = x.SeatId,
                Price = x.Price
            };
        }

        public async Task<TicketSeatDto> CreateAsync(CreateTicketSeatDto dto)
        {
            var entity = new TicketSeat
            {
                TicketId = dto.TicketId,
                SeatId = dto.SeatId,
                Price = dto.Price
            };
            var result = await _repo.AddAsync(entity);
            return new TicketSeatDto
            {
                Id = result.Id,
                TicketId = result.TicketId,
                SeatId = result.SeatId,
                Price = result.Price
            };
        }

        public async Task<bool> UpdateAsync(int id, CreateTicketSeatDto dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            entity.TicketId = dto.TicketId;
            entity.SeatId = dto.SeatId;
            entity.Price = dto.Price;
            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return false;
            await _repo.DeleteAsync(id);
            return true;
        }
    }
}
