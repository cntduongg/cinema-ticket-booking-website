using BookingManagement.Api.Models.DTOs.Seat;

namespace BookingManagement.Api.Services.IService
{
    public interface ISeatService
    {
        Task<List<ListSeatDto>> GetAllSeatsAsync();

        Task<List<ListSeatByRoomDto>> GetSeatsByRoomIdAsync(int roomId);
        Task<List<ListSeatByRoomDto>> CreateSeatsAsync(int roomId, CreateSeatsDto request);

        // New methods for CRUD operations
        Task<GetSeatByIdDto?> GetSeatByIdAsync(int id);
        Task<CreateSeatResponse> CreateSeatsByRoomAsync(int roomId, CreateSeatByRoomRequest request);
        Task<bool> UpdateSeatAsync(int id, UpdateSeatRequest request);
        Task<bool> DeleteSeatAsync(int id);
    }
}
