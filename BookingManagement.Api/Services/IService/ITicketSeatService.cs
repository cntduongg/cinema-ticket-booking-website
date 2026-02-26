using BookingManagement.Api.Models.DTOs.TicketSeat;

namespace BookingManagement.Api.Services.IService
{
    public interface ITicketSeatService
    {
        Task<IEnumerable<TicketSeatDto>> GetAllAsync();
        Task<TicketSeatDto?> GetByIdAsync(int id);
        Task<TicketSeatDto> CreateAsync(CreateTicketSeatDto dto);
        Task<bool> UpdateAsync(int id, CreateTicketSeatDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
