using BookingManagement.Api.Models.DTOs;
using BookingManagement.Api.Models.DTOs.Ticket;

namespace BookingManagement.Api.Services.IService
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>> GetAllAsync();
        Task<TicketDto> GetByIdAsync(int id);
        Task<TicketDto> AddAsync(CreateTicketDto dto);
        Task<bool> UpdateAsync(int id, CreateTicketDto dto);
        Task<bool> DeleteAsync(int id);
        
        Task<IEnumerable<AdminTicketListDto>> GetAdminTicketListAsync();
        Task<AdminTicketDetailDto> GetAdminTicketDetailAsync(int id);
    }
}
