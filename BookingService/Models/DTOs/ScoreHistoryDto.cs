using BookingService.Models.Enums;
using System;

public class ScoreHistoryDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Score { get; set; }
    public ScoreActionType ActionType { get; set; }
    public int OrderId { get; set; }
}
