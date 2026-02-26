using System.ComponentModel.DataAnnotations;

public class Movie
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(255)]
    public string Name { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    [Required]
    public DateTime FromDate { get; set; }

    [Required]
    public DateTime ToDate { get; set; }

    [Required, MaxLength(500)]
    public string Actors { get; set; }

    [Required, MaxLength(255)]
    public string ProductionCompany { get; set; }

    [Required, MaxLength(255)]
    public string Director { get; set; }

    [Required]
    public int RunningTime { get; set; }

    [Required, MaxLength(50)]
    public string Version { get; set; }

    [Required, MaxLength(500)]
    public string Trailer { get; set; }

    [Required, MaxLength(255)]
    public string Type { get; set; }


    [Required, MaxLength(1000)]
    public string Content { get; set; }

    [Required, MaxLength(255)]
    public string ImagePath { get; set; }

    [Required]
    public bool Status { get; set; }

    // 🔥 Bắt đầu phần audit đây:
    [Required]
    public int CreatedById { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; }

}
