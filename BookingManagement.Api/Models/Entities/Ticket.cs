using BookingManagement.Api.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Ticket
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int ShowtimeId { get; set; }

    [Required]
    public DateTime BookingTime { get; set; }

    [Required]
    [StringLength(20)]
    public string Status { get; set; } // Reserved, Confirmed, Cancelled

    // NULLABLE - có thể null nếu chưa tính giá
    [Column(TypeName = "decimal(12,2)")]
    public decimal? SubTotal { get; set; } = null;

    [Column(TypeName = "decimal(12,2)")]
    public decimal? DiscountAmount { get; set; } = null;

    [Column(TypeName = "decimal(12,2)")]
    public decimal? TotalPrice { get; set; } = null;

    [StringLength(50)]
    public string PromotionCode { get; set; } = null;

    // Navigation properties
    [ForeignKey("ShowtimeId")]
    public virtual Showtime Showtime { get; set; }

    public virtual ICollection<TicketSeat> TicketSeats { get; set; } = new List<TicketSeat>();
}
