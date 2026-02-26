// Services/Service/IRoomService.cs
using BookingManagement.Api.Models.DTOs.Room;
using BookingManagement.Api.Models.DTOs.Seat;

namespace BookingManagement.Api.Services.Service
{
    public interface IRoomService
    {
        //Task<bool> UpdateSeatsTypeAsync(int roomId, List<SeatUpdateDto> seatUpdates);
        //Task<bool> RoomExistsAsync(int roomId);
        //Task<List<string>> ValidateSeatsInRoomAsync(int roomId, List<int> seatIds);

        //CRUD operations for Room
        Task<IEnumerable<RoomDto>> GetAllAsync();
        Task<RoomDto?> GetByIdAsync(int id);
        Task<RoomDto> CreateAsync(CreateRoomRequest request);
        Task<bool> UpdateAsync(int id, UpdateRoomRequest request);
        Task<bool> DeleteAsync(int id);
        Task<RoomDto> CreateRoomWithSeatsAsync(CreateRoomWithSeatsRequest request);
    }
}