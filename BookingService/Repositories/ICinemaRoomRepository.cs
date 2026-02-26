using BookingService.Models;
using BookingService.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingService.Repositories
{
    public interface ICinemaRoomRepository
    {
        Task<IEnumerable<CinemaRoom>> GetAllAsync();
        Task<CinemaRoom?> GetByIdAsync(int id);
        Task AddAsync(CinemaRoom room);
        Task<CinemaRoom> UpdateAsync(CinemaRoom room,int id);
        Task<bool> DeleteAsync(int id);
        Task<PagingResult<CinemaRoom>> GetPagedCinemaRoomAsync(int pageIndex, int pageSize);
        Task<PagingResult<T>> GetPagedListAsync<T>(IQueryable<T> query, int pageIndex, int pageSize);
    }
}
