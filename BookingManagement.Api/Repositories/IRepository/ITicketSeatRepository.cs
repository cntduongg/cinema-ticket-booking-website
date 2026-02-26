using BookingManagement.Api.Models.Entities;

namespace BookingManagement.Api.Repositories.IRepository
{
    public interface ITicketSeatRepository
    {
        Task<IEnumerable<TicketSeat>> GetAllAsync();
        Task<TicketSeat?> GetByIdAsync(int id);
        Task<TicketSeat> AddAsync(TicketSeat entity);
        Task UpdateAsync(TicketSeat entity);
        Task DeleteAsync(int id);
    }
}
