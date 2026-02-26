namespace BookingService.Models;

public class Drink
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }

    public Drink(string name, int price)
    {
        Name = name;
        Price = price;
    }

    public Drink()
    {
    }
}