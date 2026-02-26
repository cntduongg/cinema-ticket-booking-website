using BookingManagement.Api.Models.DTOs;
using BookingManagement.Api.Models.Entities;

namespace BookingManagement.Api.Repositories.IRepository
{
    public interface ISeatRepository
    {
        Task<List<Seat>> GetAllAsync();

        Task<List<Seat>> GetSeatsByRoomIdAsync(int roomId);

        Task AddRangeAsync(IEnumerable<Seat> seats);

        Task SaveChangesAsync();

        // New methods for CRUD operations
        Task<Seat?> GetByIdAsync(int id);
        Task<Seat> AddAsync(Seat seat);
        Task<Seat> UpdateAsync(Seat seat);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<int> GetSeatCountByRoomIdAsync(int roomId);
    }
}
