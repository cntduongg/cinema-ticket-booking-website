using System.ComponentModel.DataAnnotations;

namespace BookingManagement.Api.Models.DTOs.Ticket
{
    public class CreateTicketDto
    {
        public int UserId { get; set; }
        public int ShowtimeId { get; set; }
        public DateTime BookingTime { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public string PromotionCode { get; set; }
        public decimal SubTotal { get; set; }
    }

    public class SeatSelectionDto
    {
        [Required(ErrorMessage = "SeatId là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "SeatId phải lớn hơn 0")]
        public int SeatId { get; set; }

        [Required(ErrorMessage = "Price là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }
    }

    public class TicketResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ShowtimeId { get; set; }
        public string MovieName { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime BookingTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<TicketSeatDto> Seats { get; set; } = new List<TicketSeatDto>();
        public decimal TotalPrice { get; set; }
    }

    public class TicketSeatDto
    {
        public int Id { get; set; }
        public int SeatId { get; set; }
        public string Row { get; set; } = string.Empty;
        public int Column { get; set; }
        public string SeatType { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
