using BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly BookingDbContext _context;

    public OrderRepository(BookingDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task<Order> AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order?> UpdateAsync(Order order)
    {
        var existing = await _context.Orders.FindAsync(order.Id);
        if (existing == null)
            return null;

        existing.UserId = order.UserId;
        existing.BookingDate = order.BookingDate;
        existing.TotalPrice = order.TotalPrice;
        existing.AddScore = order.AddScore;
        existing.Status = order.Status;
        existing.DiscountId = order.DiscountId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return false;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Order>> GetByUserIdAsync(int userId)
    {
        return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
    }

    public async Task<(List<Order> Orders, int TotalCount)> GetPagedByUserIdAsync(int userId, int page, int pageSize)
    {
        var query = _context.Orders.Where(o => o.UserId == userId);
        var total = await query.CountAsync();
        var orders = await query.OrderByDescending(o => o.BookingDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (orders, total);
    }
}
