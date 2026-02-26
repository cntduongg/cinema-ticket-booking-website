using BookingManagement.Api.Models.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace BookingManagement.Api.Models.DTOs.ShowtimeSeat
{
    public class CreateShowtimeSeatDto
    {
        //[Required]
        //public int ShowtimeId { get; set; }

        //[Required]
        //public int SeatId { get; set; }

        //[Required]
        //public ShowtimeSeatStatus Status { get; set; } = ShowtimeSeatStatus.Reserved;

        //public int? UserId { get; set; }

        //public string SessionId { get; set; }

        //public DateTime? ReservedAt { get; set; }

        //public DateTime? ExpiresAt { get; set; }

        public int ShowtimeId { get; set; }
        public int SeatId { get; set; }
        public string Status { get; set; }
        public int? UserId { get; set; }
        public string? SessionId { get; set; }
        public DateTime? ReservedAt { get; set; }
        public DateTime? ExpiresAt { get; set; }

    }
}
