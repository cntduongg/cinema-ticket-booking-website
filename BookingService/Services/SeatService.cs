using BookingService.Models;
using BookingService.Repositories;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _repo;
    private readonly ICinemaRoomRepository _roomRepo;

    public SeatService(ISeatRepository repo, ICinemaRoomRepository roomRepo)
    {
        _repo = repo;
        _roomRepo = roomRepo;
    }

    public async Task<List<Seat>> GetAllAsync() => await _repo.GetAllAsync();
    public async Task<List<Seat>> GetActiveAsync() => await _repo.GetActiveAsync();
    public async Task<Seat?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
    public async Task<Seat> CreateAsync(Seat seat) => await _repo.AddAsync(seat);
    public async Task<Seat?> UpdateAsync(Seat seat) => await _repo.UpdateAsync(seat);

    public async Task<bool> DeleteAsync(int id)
    {
        bool delete = await _repo.DeleteAsync(id);
        Seat seat = await _repo.GetByIdAsync(id);
        bool isSeatAvaiable = false;
        foreach (var s in await _repo.GetByRoomIdAsync(seat.CinemaRoomId))
        {
            if (s.SeatStatus)
            { isSeatAvaiable = true;
                var room = await _roomRepo.GetByIdAsync(seat.CinemaRoomId);
                if (room != null)
                {
                    room.Status = true;
                    await _roomRepo.UpdateAsync(room, id);
                }
                break;
            }
        }
        if(!isSeatAvaiable)
        {
            var room = await _roomRepo.GetByIdAsync(seat.CinemaRoomId);
            if (room != null)
            {
                await _roomRepo.DeleteAsync(room.Id);
            }
        }
        return delete;
    }
    
    public async Task<List<Seat>> getByRoomIdAsync(int roomId) => await _repo.GetByRoomIdAsync(roomId);

    public async Task<List<Seat>> GetSeatsByIdsAsync(List<int> ids)
    {
        return await _repo.GetSeatsByIdsAsync(ids);
    }
}
