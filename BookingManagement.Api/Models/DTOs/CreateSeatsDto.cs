using System.ComponentModel.DataAnnotations;

public class CreateSeatsDto
{
    [Required]
    [Range(1, 26, ErrorMessage = "RowCount must be between 1-26 (A-Z)")]
    public int RowCount { get; set; }

    [Required]
    public int ColumnCount { get; set; }
}

