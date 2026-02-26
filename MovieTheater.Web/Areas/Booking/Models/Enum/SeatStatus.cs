namespace MovieTheater.Web.Areas.Booking.Models.Enum
{
    public enum SeatStatus
    {

        Available = 0,  // Ghế trống, có thể chọn
        Held = 1,       // Đang được giữ tạm thời (5-10 phút)
        Booked = 2   
    }
}
