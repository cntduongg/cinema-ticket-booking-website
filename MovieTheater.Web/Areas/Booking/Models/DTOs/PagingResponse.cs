namespace MovieTheater.Web.Areas.Booking.Models.DTOs
{
    public class PagingResponse<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalRecords { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
        public bool IsSuccess { get; set; } = true; 
    }
}
