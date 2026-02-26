namespace BookingManagement.Api.Models.DTOs
{
    public class ConvertTicketDto
    {
        public int Id { get; set; }       
        public string MovieName { get; set; }

        public string Screen { get; set; }

        public DateTime Date { get; set; }

        public string Time { get; set; }
        
        public string Seat { get; set; }

        public double Price { get; set; }

        public double TotalPrice { get; set; }

        public string UserId { get; set; }

        public UserInfoDto UserInfo { get; set; }


    }
}
