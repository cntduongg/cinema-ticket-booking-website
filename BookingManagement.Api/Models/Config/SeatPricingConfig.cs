namespace BookingManagement.Api.Models.Config
{
    public class SeatPricingConfig
    {
        public decimal Normal { get; set; } = 80000;
        public decimal VIP { get; set; } = 120000;
        public decimal Couple { get; set; } = 160000;

        public decimal GetPrice(string seatType)
        {
            return seatType?.ToLower() switch
            {
                "normal" => Normal,
                "vip" => VIP,
                "couple" => Couple,
                _ => Normal
            };
        }
    }
}
