using BookingService.Models;
using BookingService.Models.DTOs;
using BookingService.Repositories;

namespace BookingService.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _repo;

        public OrderDetailService(IOrderDetailRepository repo) 
            => _repo = repo;

        public async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }   

        public async Task<OrderDetail?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<OrderDetail> CreateAsync(OrderDetail dto)
        {
            var detail = new OrderDetail(dto.OrderId, dto.SeatId, dto.ScheduleId, dto.Price);
            var created = await _repo.AddAsync(detail);
            return new OrderDetail
            {
                Id = created.Id,
                OrderId = created.OrderId,
                ScheduleId = created.ScheduleId,
                SeatId = created.SeatId,
                Price = created.Price,
            };
        }

        public async Task<OrderDetail?> UpdateAsync(int id, OrderDetail dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            existing.OrderId = dto.OrderId;
            existing.SeatId = dto.SeatId;
            existing.ScheduleId = dto.ScheduleId;
            existing.Price = dto.Price;

            var updated = await _repo.UpdateAsync(existing);
            return new OrderDetail
            {
                Id = updated.Id,
                OrderId = updated.OrderId,
                ScheduleId = updated.ScheduleId,
                SeatId = updated.SeatId,
                Price = updated.Price
            };
        }

        public async Task<bool> DeleteAsync(int id) 
            => await _repo.DeleteAsync(id);
        public async Task<List<OrderDetail>> GetByOrderIdAsync(int orderId)
        {
            return await _repo.GetByOrderIdAsync(orderId);
        }

        public async Task<List<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _repo.GetOrderDetailsByOrderIdAsync(orderId);
        }


        public async Task<List<OrderDetail>> GetByScheduleIdAsync(int scheduleId)
        {
            return await _repo.GetByScheduleIdAsync(scheduleId);
        }

        public async Task<List<int>> GetBookedSeatIdsByScheduleAsync(int scheduleId)
        {
            return await _repo.GetBookedSeatIdsByScheduleAsync(scheduleId);
        }

        public async Task<bool> IsSeatAvailableForScheduleAsync(int seatId, int scheduleId)
        {
            var isBooked = await _repo.IsSeatBookedForScheduleAsync(seatId, scheduleId);
            return !isBooked;
        }

        public async Task<int> GetTotalBookedSeatsForRoomAsync(int roomId)
        {
            return await _repo.GetTotalBookedSeatsForRoomAsync(roomId);
        }

        public async Task<bool> ValidateBookingAsync(int scheduleId, List<int> seatIds)
        {
            foreach (var seatId in seatIds)
            {
                var isBooked = await _repo.IsSeatBookedForScheduleAsync(seatId, scheduleId);
                if (isBooked)
                {
                    return false; // Có ghế đã được đặt
                }
            }
            return true; // Tất cả ghế đều available
        }

        public async Task<List<int>> GetConflictingSeatsAsync(int scheduleId, List<int> seatIds)
        {
            var conflictingSeats = new List<int>();

            foreach (var seatId in seatIds)
            {
                var isBooked = await _repo.IsSeatBookedForScheduleAsync(seatId, scheduleId);
                if (isBooked)
                {
                    conflictingSeats.Add(seatId);
                }
            }

            return conflictingSeats;
        }
    }
}
    
