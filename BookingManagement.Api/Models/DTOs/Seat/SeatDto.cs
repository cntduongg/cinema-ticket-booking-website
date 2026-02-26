using BookingManagement.Api.Models.Entities;

namespace BookingManagement.Api.Models.DTOs.Seat
{
    public class SeatDto
    {
        public int Id { get; set; }
        public string Row { get; set; } = string.Empty;
        public int Column { get; set; }
        public string SeatType { get; set; } = string.Empty;
        public bool IsBooked { get; set; }
        public decimal? Price { get; set; } // Giá nếu ghế đã được book
    }

    public class ShowtimeSeatsResponseDto
    {
        public int ShowtimeId { get; set; }
        public string MovieName { get; set; } = string.Empty;
        public string RoomName { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<SeatDto> Seats { get; set; } = new List<SeatDto>();
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public int BookedSeats { get; set; }
    }

    // New DTOs for CRUD operations
    public class GetSeatByIdDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public string Row { get; set; } = string.Empty;
        public int Column { get; set; }
        public string SeatType { get; set; } = string.Empty;
        public SeatStatus Status { get; set; }
    }

    public class CreateSeatByRoomRequest
    {
        public int SeatQuantity { get; set; }
    }

    public class UpdateSeatRequest
    {
        public string Row { get; set; } = string.Empty;
        public int Column { get; set; }
        public string SeatType { get; set; } = string.Empty;
        public SeatStatus Status { get; set; }
    }

    public class CreateSeatResponse
    {
        public int RoomId { get; set; }
        public int TotalSeatsCreated { get; set; }
        public List<SeatDistribution> SeatDistribution { get; set; } = new List<SeatDistribution>();
        public DateTime CreatedAt { get; set; }
    }

    public class SeatDistribution
    {
        public string Row { get; set; } = string.Empty;
        public int SeatsInRow { get; set; }
    }
}
