using MovieTheater.Web.Areas.Booking.Models.Enum;
namespace MovieTheater.Web.Areas.Booking.Models
{
    public class SeatSchedule
    {
        public int SeatId { get; set; }
        public string SeatLabel { get; set; } = "";
        public SeatStatus Status { get; set; }
        public bool IsSelectedByUser { get; set; } = false;

        public int ScheduleId { get; set; } // dùng cho group SignalR
        public int? HeldByUserId { get; set; } // ai đang giữ
    }
}
