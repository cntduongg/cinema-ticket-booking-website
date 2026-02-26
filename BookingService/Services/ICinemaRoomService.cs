
using BookingService.Models;
using BookingService.Models.DTOs;

namespace BookingService.Services
{
    public interface ICinemaRoomService
    {
        Task<IEnumerable<CinemaRoom>> GetAllAsync();
        Task<CinemaRoom?> GetByIdAsync(int id);
        Task CreateAsync(CinemaRoom dto);
        Task<CinemaRoom?> UpdateAsync( CinemaRoom dto,int id);
        Task<bool> DeleteAsync(int id);
        Task<PagingResult<CinemaRoomResponse>> GetPagedAsync(PagingRequest request);
    }
}

