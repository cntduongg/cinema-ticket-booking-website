using BookingManagement.Api.Utils;
using System.Text.Json.Serialization;

namespace BookingManagement.Api.Models.DTOs.Booking
{
    public class ConfirmUserShowtimeDto
    {
        public string MovieTitle { get; set; }

        public string RoomName { get; set; }

        public DateOnly ShowDate { get; set; }

        [JsonConverter(typeof(TimeOnlyToStringConverter))]
        public TimeOnly StartTime { get; set; }

    }
}
