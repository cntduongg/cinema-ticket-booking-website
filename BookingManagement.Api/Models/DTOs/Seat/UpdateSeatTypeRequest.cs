// Models/DTOs/UpdateSeatTypeRequest.cs
namespace BookingManagement.Api.Models.DTOs.Seat
{
    public class UpdateSeatTypeRequest
    {
        public List<SeatUpdateDto> Seats { get; set; } = new List<SeatUpdateDto>();
    }

    public class SeatUpdateDto
    {
        public int SeatId { get; set; }
        public string SeatType { get; set; } = string.Empty;
    }

    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}
