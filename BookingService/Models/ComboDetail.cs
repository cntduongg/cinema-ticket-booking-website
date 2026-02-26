using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.Models;

public class ComboDetail
{
    public int Id { get; set; }

    [ForeignKey("Combo")]
    public int ComboId { get; set; }
    public Combo? Combo { get; set; }

    [ForeignKey("Drink")]
    public int? DrinkId { get; set; }
    public Drink? Drink { get; set; }

    [ForeignKey("Food")]
    public int? FoodId { get; set; }
    public Food? Food { get; set; }

    public int Price { get; set; }

    public ComboDetail(int comboId, int price, int? drinkId = null, int? foodId = null)
    {
        ComboId = comboId;
        Price = price;
        DrinkId = drinkId;
        FoodId = foodId;
    }
    
    public ComboDetail()
    {
    }
}