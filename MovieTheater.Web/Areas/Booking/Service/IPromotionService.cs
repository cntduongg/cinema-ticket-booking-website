using MovieTheater.Web.Areas.Booking.Models;
using MovieTheater.Web.Areas.Booking.Models.DTOs;

namespace MovieTheater.Web.Areas.Booking.Service
{
    public interface IPromotionService
    {
        Task<IEnumerable<Promotion>> GetAllAsync();
        Task<Promotion> GetByIdAsync(int id);
        Task CreateAsync(Promotion promotion);
        Task<Promotion> UpdateAsync(int id, Promotion promotion);
        Task<bool> DeleteAsync(int id);
    }
}