using System.ComponentModel.DataAnnotations;

namespace BookingService.Models;

public class Combo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên combo không được bỏ trống")]
    public string Name { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
    public int Price { get; set; }
    
    public string Description { get; set; }

    public Combo(string name, int price, string description )
    {
        Name = name;
        Price = price;
        Description = description;
    }

    public Combo()
    {
    }
}