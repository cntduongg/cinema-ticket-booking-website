using MovieTheater.Web.Areas.Booking.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieTheater.Web.Areas.Booking.Service
{
    public interface IOrderService
    {
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<(List<Order> Orders, int TotalCount)> GetPagedOrdersByUserIdAsync(int userId, int page, int pageSize);
    }
} 