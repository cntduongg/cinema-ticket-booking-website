namespace BookingManagement.Api.Models.DTOs.ShowtimeSeat
{
    public class ShowtimeSeatDto
    {
        //public int ShowtimeId { get; set; }
        //public int SeatId { get; set; }
        //public ShowtimeSeatStatus Status { get; set; }
        //public int? UserId { get; set; }
        //public string SessionId { get; set; }
        //public DateTime? ReservedAt { get; set; }
        //public DateTime? ExpiresAt { get; set; }

        //// Thông tin bổ sung
        //public string SeatRow { get; set; }
        //public int SeatColumn { get; set; }
        //public string SeatType { get; set; }

        public int ShowtimeId { get; set; }
        public int SeatId { get; set; }
        public string Status { get; set; }
        public int? UserId { get; set; }
        public string? SessionId { get; set; }
        public DateTime? ReservedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

    }
}
