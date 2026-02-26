using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly BookingDbContext _ctx;
        public OrderDetailRepository(BookingDbContext ctx) => _ctx = ctx;

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _ctx.OrderDetails
                .Include(o => o.Order)
                .Include(s => s.Seat)
                .Include(r=>r.Seat.CinemaRoom)
                .Include(sc => sc.Schedule)
                .ToListAsync();
        }

        public async Task<OrderDetail?> GetByIdAsync(int id)
        {
            return await _ctx.OrderDetails
                .Include(o => o.Order)
                .Include(s => s.Seat)
                .ThenInclude(c => c.CinemaRoom)
                .Include(sc => sc.Schedule)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return _ctx.OrderDetails
                .Where(od => od.OrderId == orderId)
                .Include(od => od.Order)
                .Include(od => od.Seat)
                .ThenInclude(s => s.CinemaRoom)
                .Include(od => od.Schedule)
                .ThenInclude(s => s.CinemaRoom)
                .ToListAsync();
        }
        public async Task<OrderDetail> AddAsync(OrderDetail detail)
        {
            _ctx.OrderDetails.Add(detail);
            await _ctx.SaveChangesAsync();
            return detail;
        }
        public async Task<OrderDetail> UpdateAsync(OrderDetail detail)
        {
            _ctx.OrderDetails.Update(detail);
            await _ctx.SaveChangesAsync();
            return detail;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _ctx.OrderDetails.FindAsync(id);
            if (item == null) return false;
            _ctx.OrderDetails.Remove(item);
            await _ctx.SaveChangesAsync();
            return true;
        }
        // ✅ METHODS MỚI CHO SEAT BOOKING LOGIC
        public async Task<List<OrderDetail>> GetByOrderIdAsync(int orderId)
        {
            return await _ctx.OrderDetails
                .Where(od => od.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<List<OrderDetail>> GetByScheduleIdAsync(int scheduleId)
        {
            return await _ctx.OrderDetails
                .Where(od => od.ScheduleId == scheduleId)
                .Include(od => od.Order)
                .Include(od => od.Seat)
                    .ThenInclude(s => s.CinemaRoom)
                .Include(od => od.Schedule)
                .ToListAsync();
        }

        public async Task<List<int>> GetBookedSeatIdsByScheduleAsync(int scheduleId)
        {
            return await _ctx.OrderDetails
                .Where(od => od.ScheduleId == scheduleId)
                .Select(od => od.SeatId)
                .ToListAsync();
        }

        public async Task<List<OrderDetail>> GetBySeatIdAsync(int seatId)
        {
            return await _ctx.OrderDetails
                .Where(od => od.SeatId == seatId)
                .ToListAsync();
        }

        public async Task<bool> IsSeatBookedForScheduleAsync(int seatId, int scheduleId)
        {
            return await _ctx.OrderDetails
                .AnyAsync(od => od.SeatId == seatId && od.ScheduleId == scheduleId);
        }

        public async Task<int> GetTotalBookedSeatsForRoomAsync(int roomId)
        {
            // Join với Seats để lấy ghế theo phòng
            return await _ctx.OrderDetails
                .Join(_ctx.Seats,
                      od => od.SeatId,
                      s => s.Id,
                      (od, s) => new { od, s })
                .Where(joined => joined.s.CinemaRoomId == roomId)
                .CountAsync();
        }

        public async Task<List<OrderDetail>> GetByScheduleAndSeatAsync(int scheduleId, List<int> seatIds)
        {
            return await _ctx.OrderDetails
                .Where(od => od.ScheduleId == scheduleId && seatIds.Contains(od.SeatId))
                .ToListAsync();
        }
    }
}

