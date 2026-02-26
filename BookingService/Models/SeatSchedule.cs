using BookingService.Models.Enums;

namespace BookingService.Models
{
    public class SeatSchedule
    {
        public int SeatId { get; set; }
        public int ScheduleId { get; set; }

        public SeatStatus Status { get; set; } = SeatStatus.Available;

        public int? HeldByUserId { get; set; }
        public DateTime? HeldUntil { get; set; }

        public Seat? Seat { get; set; } // lưu ghế từ room
        public Schedule? Schedule { get; set; }
    }
}
