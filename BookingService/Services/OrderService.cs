using BookingService.Models;
using BookingService.Models.DTOs;
using BookingService.Repositories;

namespace BookingService.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IOrderDetailService _orderDetailService;
    private readonly ISeatService _seatService;
    private readonly ICinemaRoomService _cinemaService;
    private readonly IPromotionService _promotionService;


    public OrderService(IOrderRepository orderRepo, IOrderDetailService orderDetailService,ISeatService seatService, ICinemaRoomService cinemaService, IPromotionService promotionService)
    {
        _repository = orderRepo;
        _orderDetailService = orderDetailService;
        _seatService = seatService;
        _cinemaService = cinemaService;
        _promotionService = promotionService;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Order> CreateAsync(Order order)
    {
        if (order.DiscountId > 0)
        {
            var promotion = await _promotionService.GetByIdAsync(order.DiscountId);
            if (promotion == null || !promotion.IsActive)
                throw new Exception("Mã giảm giá không hợp lệ");

            order.TotalPrice = (int)(order.TotalPrice - (order.TotalPrice * promotion.DiscountLevel / 100));
        }
        return await _repository.AddAsync(order);
    }

    public async Task<BookingDTO> Booking(BookingDTO dto)
    {
        await CreateAsync(dto.order);
        foreach (var s in dto.seats)
        {
            // ✅ KIỂM TRA GHẾT CÓ AVAILABLE KHÔNG TRƯỚC KHI TẠO
            var isAvailable = await _orderDetailService.IsSeatAvailableForScheduleAsync(s.Id, dto.schedule.Id);
            if (!isAvailable)
            {
                throw new InvalidOperationException($"Seat {s.Id} is already booked for schedule {dto.schedule.Id}");
            }

            var detail = new OrderDetail(dto.order.Id, s.Id, dto.schedule.Id, 75);
            await _orderDetailService.CreateAsync(detail);
        }
        return dto;
    }
    
    public async Task<Order?> UpdateAsync(Order order)
    {
        return await _repository.UpdateAsync(order);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<List<Order>> GetByUserIdAsync(int userId)
    {
        return await _repository.GetByUserIdAsync(userId);
    }

    public async Task<(List<Order> Orders, int TotalCount)> GetPagedByUserIdAsync(int userId, int page, int pageSize)
    {
        return await _repository.GetPagedByUserIdAsync(userId, page, pageSize);
    }
}
