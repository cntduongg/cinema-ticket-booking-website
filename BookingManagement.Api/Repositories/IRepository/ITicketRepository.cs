using BookingManagement.Api.Models.Entities;

namespace BookingManagement.Api.Repositories.IRepository
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllAsync();
        Task<Ticket> GetByIdAsync(int id);
        Task<Ticket> AddAsync(Ticket ticket);
        Task<bool> UpdateAsync(Ticket ticket);
        Task<bool> DeleteAsync(int id);
        
        Task<IEnumerable<Ticket>> GetTicketsWithDetailsAsync();
        Task<Ticket> GetTicketWithDetailsAsync(int id);
    }
}
