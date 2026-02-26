using BookingService.Models;
using BookingService.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repositories
{
    public class SeatScheduleRepository : ISeatScheduleRepository
    {
        private readonly BookingDbContext _context;

        public SeatScheduleRepository(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SeatSchedule>> GetAllAsync()
        {
            return await _context.SeatSchedules
                .Include(s => s.Seat)
                .Include(s => s.Schedule)

                .ToListAsync();
        }
        public async Task<SeatSchedule?> GetAsync(int seatId, int scheduleId)
        {
            return await _context.SeatSchedules.FindAsync(seatId, scheduleId);
        }

        public async Task<IEnumerable<SeatSchedule>> GetByScheduleAsync(int scheduleId)
        {
            return await _context.SeatSchedules
                .Where(ss => ss.ScheduleId == scheduleId)
                .Include(ss => ss.Seat)
                .Include(ss => ss.Schedule)
                .ThenInclude(seat => seat.CinemaRoom)// ❗ Rất quan trọng nếu cần thông tin ghế
                .ToListAsync();
        }

        public async Task AddAsync(SeatSchedule entity)
        {
            _context.SeatSchedules.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(SeatSchedule entity)
        {
            _context.SeatSchedules.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int seatId, int scheduleId)
        {
            var existing = await GetAsync(seatId, scheduleId);
            if (existing != null)
            {
                _context.SeatSchedules.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> ValidateHeldSeatsAsync(int scheduleId, List<int> seatIds, int userId)
        {
            var heldSeats = await _context.SeatSchedules
                .Where(s => s.ScheduleId == scheduleId && seatIds.Contains(s.SeatId))
                .ToListAsync();

            return heldSeats.Count == seatIds.Count &&
        heldSeats.All(s =>
            s.Status == SeatStatus.Held &&
            s.HeldByUserId == userId &&
            s.HeldUntil > DateTime.UtcNow
        );
        }

        public async Task MarkSeatsAsBookedAsync(int scheduleId, List<int> seatIds, int userId)
        {
            // BỎ điều kiện HeldUntil > now để đảm bảo mọi ghế Held bởi user đều được Booked sau thanh toán
            var seats = await _context.SeatSchedules
                .Where(s =>
                    s.ScheduleId == scheduleId &&
                    seatIds.Contains(s.SeatId) &&
                    s.Status == SeatStatus.Held &&
                    s.HeldByUserId == userId)
                .ToListAsync();

            foreach (var seat in seats)
            {
                seat.Status = SeatStatus.Booked;
                seat.HeldUntil = null;
                seat.HeldByUserId = null;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> CreateSeatSchedulesFromRoomAsync(int scheduleId, int roomId)
        {
            var seats = await _context.Seats
                .Where(s => s.CinemaRoomId == roomId)

                .ToListAsync();

            var seatSchedules = seats.Select(seat => new SeatSchedule
            {
                SeatId = seat.Id,
                ScheduleId = scheduleId,
                Status = SeatStatus.Available
            }).ToList();

            await _context.SeatSchedules.AddRangeAsync(seatSchedules);
            var created = await _context.SaveChangesAsync();
            return created;
        }

        public async Task ReleaseSeatAsync(int seatId, int scheduleId, int userId)
        {
            var seat = await _context.SeatSchedules
                .FirstOrDefaultAsync(s => s.SeatId == seatId && s.ScheduleId == scheduleId);

            if (seat == null || seat.Status != SeatStatus.Held || seat.HeldByUserId != userId)
            {
                throw new InvalidOperationException("Seat not held by user or already released.");
            }

            seat.Status = SeatStatus.Available;
            seat.HeldUntil = null;
            seat.HeldByUserId = null;

            _context.SeatSchedules.Update(seat);
            await _context.SaveChangesAsync();
        }
    }
}
