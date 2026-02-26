using BookingService.Models;

public interface ISeatRepository
{
    Task<List<Seat>> GetAllAsync();
    Task<List<Seat>> GetActiveAsync();
    Task<Seat?> GetByIdAsync(int id);
    Task<Seat> AddAsync(Seat seat);
    Task<Seat?> UpdateAsync(Seat seat);
    Task<bool> DeleteAsync(int id);
    Task<List<Seat>> AddRangeAsync(List<Seat> seats);
    Task<List<Seat>> GetByRoomIdAsync(int roomId);
    Task<List<Seat>> GetSeatsByIdsAsync(List<int> ids);
    Task DeleteByRoomIdAsync(int roomId);
}
