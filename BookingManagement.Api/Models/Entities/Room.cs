using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingManagement.Api.Models.Entities
{
    public class Room
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public int CinemaId { get; set; }

        [Required]
        [Range(1, 500)]
        public int SeatQuantity { get; set; } // Số lượng ghế - tự tạo ghế khi tạo phòng

        // Navigation properties
        [ForeignKey("CinemaId")]
        public virtual Cinema Cinema { get; set; }

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
        public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}
