namespace BookingManagement.Api.Models.DTOs.ShowtimeSeat
{
    public class UpdateShowtimeSeatDto
    {
        //[Required]
        //public ShowtimeSeatStatus Status { get; set; }

        //public int? UserId { get; set; }

        //public string SessionId { get; set; }

        //public DateTime? ReservedAt { get; set; }

        //public DateTime? ExpiresAt { get; set; }

        public string Status { get; set; }
        public int? UserId { get; set; }
        public string? SessionId { get; set; }
        public DateTime? ReservedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
