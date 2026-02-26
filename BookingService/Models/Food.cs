namespace BookingService.Models;

public class Food
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }

    public Food(string name, int price)
    {
        Name = name;
        Price = price;
    }
    public Food()
    {
    }
}