using BookingService.Models;

public interface ISeatService
{
    Task<List<Seat>> GetAllAsync();
    Task<List<Seat>> GetActiveAsync();
    Task<Seat?> GetByIdAsync(int id);
    Task<Seat> CreateAsync(Seat seat);
    Task<Seat?> UpdateAsync(Seat seat);
    Task<bool> DeleteAsync(int id);
    Task<List<Seat>> getByRoomIdAsync(int roomId);
    Task<List<Seat>> GetSeatsByIdsAsync(List<int> ids);
}
