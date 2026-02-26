using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingManagement.Api.Models.Entities
{
    public class Showtime
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MovieId { get; set; }

        [Required]
        public int RoomId { get; set; }
        [Required]
        public DateOnly ShowDate { get; set; }    // maps to DATE
        [Required]
        public TimeOnly StartTime { get; set; }   // maps to TIME
        [Required]
        public TimeOnly EndTime { get; set; }     // maps to TIME

        // Navigation properties
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public virtual ICollection<ShowtimeSeat> ShowtimeSeats { get; set; } = new List<ShowtimeSeat>(); // Thêm navigation
    }
}
