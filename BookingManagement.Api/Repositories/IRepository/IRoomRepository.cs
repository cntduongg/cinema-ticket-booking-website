using BookingManagement.Api.Models.Entities;
using BookingManagement.Api.Repositories;
namespace BookingManagement.Api.Repositories.IRepository
{
    public interface IRoomRepository 
    {
        Task<IEnumerable<Room>> GetAllAsync();
        Task<Room?> GetByIdAsync(int id);
        Task<Room> AddAsync(Room room);
        Task<bool> UpdateAsync(Room room);
        Task<bool> DeleteAsync(int id);
    }
}
