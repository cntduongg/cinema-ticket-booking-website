using MovieTheater.Web.Areas.Booking.Models;
using MovieTheater.Web.Areas.Booking.Models.DTOs;
using MovieTheater.Web.Areas.MovieManagement.Models;

namespace MovieTheater.Web.Areas.Booking.Service
{
    public interface ICinemaRoomService
    {
        Task<List<CinemaRoom>> GetAllCinemaRoomsAsync();
        Task<bool> CreateRoomAsync(CinemaRoom model);
        Task<List<CinemaRoomDTO>> GetAllAsync();
        Task<CinemaRoomDTO?> GetByIdAsync(int id);
        Task<bool> CreateAsync(CinemaRoomDTO room);
        Task<CinemaRoomDTO?> UpdateAsync(int id, CinemaRoomDTO updatedRoom);
        Task<bool> DeleteAsync(int id);
        Task<PagingResponse<CinemaRoomDTO>> GetPagedCinemaRoomsAsync(int pageIndex, int pageSize);
        Task<bool> ToggleRoomStatusAsync(int id);
    }
}
