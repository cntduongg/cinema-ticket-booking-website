using System.ComponentModel.DataAnnotations.Schema;

namespace BookingService.Models;

public class OrderProduct
{
    public int Id { get; set; }

    [ForeignKey("Order")]
    public int OrderId { get; set; }
    public Order? Order { get; set; }

    [ForeignKey("Drink")]
    public int? DrinkId { get; set; }
    public Drink? Drink { get; set; }

    [ForeignKey("Food")]
    public int? FoodId { get; set; }
    public Food? Food { get; set; }

    [ForeignKey("Combo")]
    public int? ComboId { get; set; }
    public Combo? Combo { get; set; }

    public OrderProduct(int orderId, int? drinkId = null, int? foodId = null, int? comboId = null)
    {
        OrderId = orderId;
        DrinkId = drinkId;
        FoodId = foodId;
        ComboId = comboId;
    }
    public OrderProduct()
    {
    }
}