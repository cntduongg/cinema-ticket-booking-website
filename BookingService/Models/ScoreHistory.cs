using BookingService.Models.Enums;

namespace BookingService.Models
{
 
    public class ScoreHistory
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Score { get; set; }

        public ScoreActionType ActionType { get; set; }

        // Liên kết với Order
        public int OrderId { get; set; }
        public Order Order { get; set; }  // Navigation Property

        public ScoreHistory() { }

        public ScoreHistory(int userId, DateTime createdAt, int score, ScoreActionType actionType, int orderId)
        {
            UserId = userId;
            CreatedAt = createdAt;
            Score = score;
            ActionType = actionType;
            OrderId = orderId;
        }
    }
}
