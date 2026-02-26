using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingManagement.Api.Models.Entities
{
    public class Seat
    {
        [Key]
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
        public string SeatType { get; set; } // Normal, VIP

        [Required]
        public SeatStatus Status { get; set; } = SeatStatus.Active; // Thêm lại cho ghế hỏng/bảo trì

        // Navigation properties
        [ForeignKey("RoomId")]
        public virtual Room Room { get; set; }

        public virtual ICollection<TicketSeat> TicketSeats { get; set; } = new List<TicketSeat>();
        public virtual ICollection<ShowtimeSeat> ShowtimeSeats { get; set; } = new List<ShowtimeSeat>();
    }

    public enum SeatStatus
    {
        Active = 0,        // Ghế hoạt động bình thường
        Maintenance = 1,   // Ghế đang bảo trì
        Broken = 2,        // Ghế hỏng
        Disabled = 3       // Ghế bị vô hiệu hóa vĩnh viễn
    }
}
