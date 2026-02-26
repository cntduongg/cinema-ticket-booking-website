using BookingService.Models;
using BookingService.Models.Enums;
using BookingService.Repositories;
using Microsoft.AspNetCore.SignalR;
using BookingService.Services.SignalRConfig;

namespace BookingService.Services
{
    public class SeatScheduleService : ISeatScheduleService
    {
        private readonly ISeatScheduleRepository _repo;
        private readonly IHubContext<SeatHub> _hubContext;

        public SeatScheduleService(ISeatScheduleRepository repo, IHubContext<SeatHub> hubContext)
        {
            _repo = repo;
            _hubContext = hubContext;
        }

        public Task<SeatSchedule?> GetAsync(int seatId, int scheduleId)
            => _repo.GetAsync(seatId, scheduleId);

        public Task<IEnumerable<SeatSchedule>> GetByScheduleAsync(int scheduleId)
            => _repo.GetByScheduleAsync(scheduleId);

        public async Task HoldSeatAsync(int seatId, int scheduleId, int userId, DateTime holdUntil)
        {
            var existing = await _repo.GetAsync(seatId, scheduleId);
            Console.WriteLine($@"[DEBUG] HoldSeatAsync:
  seatId={seatId}
  scheduleId={scheduleId}
  userId={userId}
  existing.Status={existing?.Status}
  existing.HeldByUserId={existing?.HeldByUserId}
  existing.HeldUntil={existing?.HeldUntil}
  now={DateTime.UtcNow}");

            if (existing != null)
            {
                if (existing.Status == SeatStatus.Booked)
                    throw new InvalidOperationException("Seat already booked");

                if (existing.Status == SeatStatus.Held &&
                    existing.HeldByUserId != userId &&
                    existing.HeldUntil > DateTime.UtcNow)
                    throw new InvalidOperationException("Seat is currently held by another user");
            }

            var seat = existing ?? new SeatSchedule
            {
                SeatId = seatId,
                ScheduleId = scheduleId
            };

            seat.Status = SeatStatus.Held;
            seat.HeldByUserId = userId;
            seat.HeldUntil = holdUntil;

            if (existing == null)
                await _repo.AddAsync(seat);
            else
                await _repo.UpdateAsync(seat);
        }

        public async Task ConfirmBookingAsync(int seatId, int scheduleId)
        {
            var seat = await _repo.GetAsync(seatId, scheduleId)
                ?? throw new InvalidOperationException("Seat not found");

            seat.Status = SeatStatus.Booked;
            seat.HeldByUserId = null;
            seat.HeldUntil = null;

            await _repo.UpdateAsync(seat);
        }

        public async Task ReleaseExpiredHoldsAsync()
        {
            var now = DateTime.UtcNow;
            var allHeld = await _repo.GetByScheduleAsync(scheduleId: -1); // giả định, cần lọc theo từng lịch
            var expired = allHeld
                .Where(s => s.Status == SeatStatus.Held && s.HeldUntil <= now)
                .ToList();

            foreach (var s in expired)
            {
                s.Status = SeatStatus.Available;
                s.HeldByUserId = null;
                s.HeldUntil = null;
                await _repo.UpdateAsync(s);
            }
        }
        public async Task<int> ReleaseAllExpiredHoldsAsync()
        {
            var all = await _repo.GetAllAsync();
            var now = DateTime.UtcNow;

            var expired = all.Where(s => s.Status == SeatStatus.Held && s.HeldUntil <= now).ToList();

            foreach (var seat in expired)
            {
                seat.Status = SeatStatus.Available;
                seat.HeldByUserId = null;
                seat.HeldUntil = null;

                await _repo.UpdateAsync(seat);
                // Gửi sự kiện SignalR tới group của suất chiếu
                await _hubContext.Clients.Group($"schedule-{seat.ScheduleId}").SendAsync("SeatReleased", seat.SeatId, seat.ScheduleId);
            }

            return expired.Count;
        }
        public Task<bool> ValidateHeldSeatsAsync(int scheduleId, List<int> seatIds, int userId)
        {
            return _repo.ValidateHeldSeatsAsync(scheduleId, seatIds, userId);
        }

        public Task MarkSeatsAsBookedAsync(int scheduleId, List<int> seatIds, int userId)
        {
            return _repo.MarkSeatsAsBookedAsync(scheduleId, seatIds, userId);
        }
        public async Task<int> CreateSeatSchedulesFromRoomAsync(int scheduleId, int roomId)
        {
            return await _repo.CreateSeatSchedulesFromRoomAsync(scheduleId, roomId);
        }
        public async Task ReleaseSeatAsync(int seatId, int scheduleId, int userId)
        {
            await _repo.ReleaseSeatAsync(seatId, scheduleId, userId);
        }
    }

}
