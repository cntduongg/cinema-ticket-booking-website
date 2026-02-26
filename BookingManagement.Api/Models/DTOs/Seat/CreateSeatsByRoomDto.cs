using System.ComponentModel.DataAnnotations;

namespace BookingManagement.Api.Models.DTOs.Seat
{
    public class CreateSeatsByRoomDto
    {
        [Required]
        public int RoomId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng ghế phải lớn hơn 0")]
        public int SeatQuantity { get; set; }
    }
}