using BookingManagement.Api.Models.DTOs.ShowtimeSeat;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingManagement.Api.Services.IService
{
    public interface IShowtimeSeatService
    {
        //        Task<List<ShowtimeSeatDto>> GetAllAsync();
        //        Task<List<ShowtimeSeatDto>> GetByShowtimeIdAsync(int showtimeId);
        //        Task<ShowtimeSeatDto> GetByShowtimeAndSeatIdAsync(int showtimeId, int seatId);
        //        Task<ShowtimeSeatDto> CreateAsync(CreateShowtimeSeatDto dto);
        //        Task<ShowtimeSeatDto> UpdateAsync(int showtimeId, int seatId, UpdateShowtimeSeatDto dto);
        //        Task DeleteAsync(int showtimeId, int seatId);

        Task<IEnumerable<ShowtimeSeatDto>> GetAllAsync();
        Task<ShowtimeSeatDto?> GetByIdAsync(int showtimeId, int seatId);
        Task<ShowtimeSeatDto> CreateAsync(CreateShowtimeSeatDto dto);
        Task<bool> UpdateAsync(int showtimeId, int seatId, CreateShowtimeSeatDto dto);
        Task<bool> DeleteAsync(int showtimeId, int seatId);


    }
}
