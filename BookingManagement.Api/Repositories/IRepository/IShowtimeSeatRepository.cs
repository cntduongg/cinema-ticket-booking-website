using BookingManagement.Api.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookingManagement.Api.Repositories
{
    public interface IShowtimeSeatRepository
    {
        //        Task<List<ShowtimeSeat>> GetAllAsync();
        //        Task<List<ShowtimeSeat>> GetByShowtimeIdAsync(int showtimeId);
        //        Task<ShowtimeSeat> GetByShowtimeAndSeatIdAsync(int showtimeId, int seatId);
        //        Task CreateAsync(ShowtimeSeat showtimeSeat);
        //        Task UpdateAsync(ShowtimeSeat showtimeSeat);
        //        Task DeleteAsync(int showtimeId, int seatId);
        //        Task<bool> ExistsAsync(int showtimeId, int seatId);

        Task<IEnumerable<ShowtimeSeat>> GetAllAsync();
        Task<ShowtimeSeat?> GetByIdAsync(int showtimeId, int seatId);
        Task<ShowtimeSeat> AddAsync(ShowtimeSeat entity);
        Task UpdateAsync(ShowtimeSeat entity);
        Task DeleteAsync(int showtimeId, int seatId);


    }
}
