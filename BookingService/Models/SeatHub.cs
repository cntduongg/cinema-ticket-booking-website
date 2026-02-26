using Microsoft.AspNetCore.SignalR;

namespace BookingService.Models
{
    public class SeatHub : Hub
    {
        // Khi người dùng mở trang chọn ghế cho một suất chiếu
        public async Task JoinScheduleGroup(int scheduleId)
        {
            try
            {
                Console.WriteLine($"🔗 Attempting to join schedule-{scheduleId}, Connection: {Context.ConnectionId}");
                await Groups.AddToGroupAsync(Context.ConnectionId, $"schedule-{scheduleId}");
                Console.WriteLine($"✅ Joined schedule-{scheduleId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in JoinScheduleGroup: {ex.Message}");
                throw; // để FE bắt được
            }
        }

        // Khi người dùng rời trang chọn ghế
        public async Task LeaveScheduleGroup(int scheduleId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"schedule-{scheduleId}");
        }
    }
}
