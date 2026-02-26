using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingManagement.Api.Models.Entities
{
    public class ShowtimeSeat
    {
        // Composite Primary Key - KHÔNG có Id riêng
        [Key, Column(Order = 0)]
        public int ShowtimeId { get; set; }

        [Key, Column(Order = 1)]
        public int SeatId { get; set; }

        [Required]
        public ShowtimeSeatStatus Status { get; set; } = ShowtimeSeatStatus.Available;

        public int? UserId { get; set; } // User đang hold ghế (nếu có)

        [StringLength(100)]
        public string? SessionId { get; set; } // THÊM ? để làm nullable

        public DateTime? ReservedAt { get; set; } // Thời gian bắt đầu hold ghế

        public DateTime? ExpiresAt { get; set; } // Thời gian hết hạn hold ghế

        // Navigation properties
        [ForeignKey("ShowtimeId")]
        public virtual Showtime Showtime { get; set; }

        [ForeignKey("SeatId")]
        public virtual Seat Seat { get; set; }
    }

    public enum ShowtimeSeatStatus
    {
        Available = 0,    // Ghế trống, có thể chọn
        Reserved = 1,     // Ghế đang được hold tạm thời (10-15 phút)
        Booked = 2,       // Ghế đã được đặt (có vé)
        Blocked = 3       // Ghế bị khóa (do ghế hỏng hoặc bảo trì)
    }
}
