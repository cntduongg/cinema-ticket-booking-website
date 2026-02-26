using BookingManagement.Api.Data;
using BookingManagement.Api.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingManagement.Api.Controllers
{
    public class Showtimes1Controller : Controller
    {
        private readonly ShowtimeManagementDbContext _context;

        public Showtimes1Controller(ShowtimeManagementDbContext context)
        {
            _context = context;
        }

        [HttpPost("tickets/pending")]
        public async Task<IActionResult> CreatePendingTicket([FromBody] CreatePendingTicketRequest request)
        {
            const decimal defaultPrice = 80000m;

            // Lấy danh sách các seatId đã được đặt (từ các vé không bị Cancelled)
            var bookedSeatIds = await _context.Tickets
                .Where(t => t.ShowtimeId == request.ShowtimeId && t.Status != "Cancelled")
                .SelectMany(t => t.TicketSeats.Select(ts => ts.SeatId))
                .ToListAsync();

            // Kiểm tra trùng ghế
            var duplicateSeats = request.SeatIds.Intersect(bookedSeatIds).ToList();
            if (duplicateSeats.Any())
            {
                return BadRequest(new
                {
                    message = $"These seat IDs are already booked: {string.Join(", ", duplicateSeats)}",
                    invalidSeatIds = duplicateSeats
                });
            }

            // Tạo Ticket
            var ticket = new Ticket
            {
                UserId = request.UserId,
                ShowtimeId = request.ShowtimeId,
                BookingTime = DateTime.Now,
                Status = "Pending",
                SubTotal = defaultPrice * request.SeatIds.Count,
                TotalPrice = defaultPrice * request.SeatIds.Count,
                PromotionCode = ""
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync(); // để lấy được ticket.Id

            // Tạo TicketSeats
            foreach (var seatId in request.SeatIds)
            {
                _context.TicketSeats.Add(new TicketSeat
                {
                    TicketId = ticket.Id,
                    SeatId = seatId,
                    Price = defaultPrice
                });
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                TicketId = ticket.Id,
                SeatIds = request.SeatIds,
                Status = ticket.Status,
                Message = "Ticket created successfully."
            });
        }

        public class CreatePendingTicketRequest
        {
            public int UserId { get; set; }
            public int ShowtimeId { get; set; }
            public List<int> SeatIds { get; set; }

        }
    }
}
