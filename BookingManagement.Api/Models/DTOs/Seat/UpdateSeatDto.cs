using System.ComponentModel.DataAnnotations;

namespace BookingManagement.Api.Models.DTOs.Seat
{
    public class UpdateSeatDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        [StringLength(5)]
        public string Row { get; set; }

        [Required]
        public int Column { get; set; }

        [Required]
        [StringLength(20)]
        public string SeatType { get; set; }

        [Required]
        public SeatStatus Status { get; set; }
    }
}