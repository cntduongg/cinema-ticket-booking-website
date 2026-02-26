using Xunit;
using Moq;
using FluentAssertions;
using BookingService.Models;
using BookingService.Models.DTOs;
using BookingService.Repositories;
using BookingService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Allure.Xunit.Attributes;
using Allure.Net.Commons;

namespace UnitTestSp3
{
    using SeverityLevel = Allure.Net.Commons.SeverityLevel;

    /// <summary>
    /// Unit tests for Booking Service using mocked dependencies
    /// </summary>
    [AllureSuite("Booking Service Unit Tests")]
    public class BookingServiceTest
    {
        private readonly Mock<IOrderRepository> _orderRepoMock;
        private readonly Mock<IOrderDetailService> _orderDetailServiceMock;
        private readonly Mock<ISeatService> _seatServiceMock;
        private readonly Mock<ICinemaRoomService> _cinemaServiceMock;
        private readonly Mock<IPromotionService> _promotionServiceMock;
        private readonly OrderService _orderService;

        public BookingServiceTest()
        {
            _orderRepoMock = new Mock<IOrderRepository>();
            _orderDetailServiceMock = new Mock<IOrderDetailService>();
            _seatServiceMock = new Mock<ISeatService>();
            _cinemaServiceMock = new Mock<ICinemaRoomService>();
            _promotionServiceMock = new Mock<IPromotionService>();

            _orderService = new OrderService(
                _orderRepoMock.Object,
                _orderDetailServiceMock.Object,
                _seatServiceMock.Object,
                _cinemaServiceMock.Object,
                _promotionServiceMock.Object
            );
        }

        // -------------------------------
        // OrderService - Create Order
        // -------------------------------

        [Fact]
        [AllureFeature("Order Management")]
        [AllureStory("Create Order")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureTag("unit", "order", "create")]
        public async Task CreateAsync_ShouldCreateOrderSuccessfully_WhenValidOrderProvided()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                UserId = 1,
                TotalPrice = 160000,
                Status = false,
                BookingDate = DateOnly.FromDateTime(DateTime.Now),
                DiscountId = 0
            };

            _orderRepoMock.Setup(r => r.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync(order);

            // Act
            var result = await _orderService.CreateAsync(order);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.TotalPrice.Should().Be(160000);
            result.Status.Should().BeFalse();
            _orderRepoMock.Verify(r => r.AddAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        [AllureFeature("Order Management")]
        [AllureStory("Create Order with Discount")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "order", "create", "discount")]
        public async Task CreateAsync_ShouldApplyDiscount_WhenValidPromotionProvided()
        {
            // Arrange
            var promotion = new Promotion
            {
                PromotionId = 1,
                DiscountLevel = 20,
                IsActive = true
            };

            var order = new Order
            {
                Id = 1,
                UserId = 1,
                TotalPrice = 100000,
                Status = false,
                BookingDate = DateOnly.FromDateTime(DateTime.Now),
                DiscountId = 1
            };

            var expectedDiscountedPrice = 80000;

            _promotionServiceMock.Setup(p => p.GetByIdAsync(1))
                .ReturnsAsync(promotion);

            _orderRepoMock.Setup(r => r.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order o) => o);

            // Act
            var result = await _orderService.CreateAsync(order);

            // Assert
            result.Should().NotBeNull();
            result.TotalPrice.Should().Be(expectedDiscountedPrice);
            _promotionServiceMock.Verify(p => p.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        [AllureFeature("Order Management")]
        [AllureStory("Create Order with Invalid Promotion")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "order", "create", "error")]
        public async Task CreateAsync_ShouldThrowException_WhenInvalidPromotionProvided()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                UserId = 1,
                TotalPrice = 100000,
                Status = false,
                BookingDate = DateOnly.FromDateTime(DateTime.Now),
                DiscountId = 1
            };

            _promotionServiceMock.Setup(p => p.GetByIdAsync(1))
                .ReturnsAsync((Promotion)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _orderService.CreateAsync(order));
            exception.Message.Should().Be("Mã giảm giá không hợp lệ");
        }

        // -------------------------------
        // OrderService - Booking Process (Main Flow)
        // -------------------------------

        [Fact]
        [AllureFeature("Booking Process")]
        [AllureStory("Successful Booking")]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureTag("unit", "booking", "success")]
        public async Task Booking_ShouldSucceed_WhenAllSeatsAreAvailable()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                UserId = 1,
                TotalPrice = 150000,
                Status = false,
                BookingDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var seats = new List<Seat>
            {
                new Seat { Id = 1, SeatRow = 'A', SeatColumn = '1', CinemaRoomId = 1 },
                new Seat { Id = 2, SeatRow = 'A', SeatColumn = '2', CinemaRoomId = 1 }
            };

            var schedule = new Schedule
            {
                Id = 1,
                MovieId = 1,
                CinemaRoomId = 1,
                ShowDate = DateOnly.Parse("2024-12-01"),
                FromTime = TimeOnly.Parse("14:00"),
                ToTime = TimeOnly.Parse("16:30")
            };

            var bookingDto = new BookingDTO
            {
                order = order,
                seats = seats,
                schedule = schedule
            };

            _orderRepoMock.Setup(r => r.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync(order);

            _orderDetailServiceMock.Setup(s => s.IsSeatAvailableForScheduleAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            _orderDetailServiceMock.Setup(s => s.CreateAsync(It.IsAny<OrderDetail>()))
                .ReturnsAsync((OrderDetail od) => od);

            // Act
            var result = await _orderService.Booking(bookingDto);

            // Assert
            result.Should().NotBeNull();
            result.order.Id.Should().Be(1);
            result.seats.Should().HaveCount(2);
            result.schedule.Id.Should().Be(1);
            _orderDetailServiceMock.Verify(s => s.IsSeatAvailableForScheduleAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
            _orderDetailServiceMock.Verify(s => s.CreateAsync(It.IsAny<OrderDetail>()), Times.Exactly(2));
        }

        [Fact]
        [AllureFeature("Booking Process")]
        [AllureStory("Booking Failure - Seat Already Booked")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "booking", "failure", "seat")]
        public async Task Booking_ShouldFail_WhenSeatIsAlreadyBooked()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                UserId = 1,
                TotalPrice = 150000,
                Status = false,
                BookingDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var seats = new List<Seat>
            {
                new Seat { Id = 1, SeatRow = 'A', SeatColumn = '1', CinemaRoomId = 1 }
            };

            var schedule = new Schedule
            {
                Id = 1,
                MovieId = 1,
                CinemaRoomId = 1,
                ShowDate = DateOnly.Parse("2024-12-01"),
                FromTime = TimeOnly.Parse("14:00"),
                ToTime = TimeOnly.Parse("16:30")
            };

            var bookingDto = new BookingDTO
            {
                order = order,
                seats = seats,
                schedule = schedule
            };

            _orderRepoMock.Setup(r => r.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync(order);

            _orderDetailServiceMock.Setup(s => s.IsSeatAvailableForScheduleAsync(1, 1))
                .ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _orderService.Booking(bookingDto));
            exception.Message.Should().Be("Seat 1 is already booked for schedule 1");
            _orderDetailServiceMock.Verify(s => s.IsSeatAvailableForScheduleAsync(1, 1), Times.Once);
        }

        [Fact]
        [AllureFeature("Booking Process")]
        [AllureStory("Booking Failure - Multiple Seats Unavailable")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "booking", "failure", "multiple-seats")]
        public async Task Booking_ShouldFail_WhenMultipleSeatsUnavailable()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                UserId = 1,
                TotalPrice = 225000,
                Status = false,
                BookingDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var seats = new List<Seat>
            {
                new Seat { Id = 1, SeatRow = 'A', SeatColumn = '1', CinemaRoomId = 1 },
                new Seat { Id = 2, SeatRow = 'A', SeatColumn = '2', CinemaRoomId = 1 },
                new Seat { Id = 3, SeatRow = 'A', SeatColumn = '3', CinemaRoomId = 1 }
            };

            var schedule = new Schedule
            {
                Id = 1,
                MovieId = 1,
                CinemaRoomId = 1,
                ShowDate = DateOnly.Parse("2024-12-01"),
                FromTime = TimeOnly.Parse("14:00"),
                ToTime = TimeOnly.Parse("16:30")
            };

            var bookingDto = new BookingDTO
            {
                order = order,
                seats = seats,
                schedule = schedule
            };

            _orderRepoMock.Setup(r => r.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync(order);

            _orderDetailServiceMock.Setup(s => s.IsSeatAvailableForScheduleAsync(1, 1))
                .ReturnsAsync(true);
            _orderDetailServiceMock.Setup(s => s.IsSeatAvailableForScheduleAsync(2, 1))
                .ReturnsAsync(false);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _orderService.Booking(bookingDto));
            exception.Message.Should().Be("Seat 2 is already booked for schedule 1");
            _orderDetailServiceMock.Verify(s => s.IsSeatAvailableForScheduleAsync(1, 1), Times.Once);
            _orderDetailServiceMock.Verify(s => s.IsSeatAvailableForScheduleAsync(2, 1), Times.Once);
        }

        // -------------------------------
        // OrderService - Get Orders
        // -------------------------------

        [Fact]
        [AllureFeature("Order Retrieval")]
        [AllureStory("Get All Orders - Empty Result")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureTag("unit", "order", "retrieve", "list")]
        public async Task GetAllAsync_ShouldReturnEmpty_WhenNoOrdersExist()
        {
            // Arrange
            _orderRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Order>());

            // Act
            var result = await _orderService.GetAllAsync();

            // Assert
            result.Should().BeEmpty();
            _orderRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        [AllureFeature("Order Retrieval")]
        [AllureStory("Get All Orders - With Data")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureTag("unit", "order", "retrieve", "list")]
        public async Task GetAllAsync_ShouldReturnOrders_WhenOrdersExist()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order { Id = 1, UserId = 1, TotalPrice = 160000, Status = true, BookingDate = DateOnly.FromDateTime(DateTime.Now) },
                new Order { Id = 2, UserId = 2, TotalPrice = 240000, Status = false, BookingDate = DateOnly.FromDateTime(DateTime.Now) },
                new Order { Id = 3, UserId = 1, TotalPrice = 80000, Status = true, BookingDate = DateOnly.FromDateTime(DateTime.Now) }
            };

            _orderRepoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetAllAsync();

            // Assert
            result.Should().HaveCount(3);
            result.First().Id.Should().Be(1);
            result.Should().Contain(o => o.UserId == 1);
            result.Should().Contain(o => o.UserId == 2);
            _orderRepoMock.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        [AllureFeature("Order Retrieval")]
        [AllureStory("Get Order By Id - Not Found")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "order", "retrieve", "by-id")]
        public async Task GetByIdAsync_ShouldReturnNull_WhenOrderNotFound()
        {
            // Arrange
            var orderId = 999;
            _orderRepoMock.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync((Order)null);

            // Act
            var result = await _orderService.GetByIdAsync(orderId);

            // Assert
            result.Should().BeNull();
            _orderRepoMock.Verify(r => r.GetByIdAsync(orderId), Times.Once);
        }

        [Fact]
        [AllureFeature("Order Retrieval")]
        [AllureStory("Get Order By Id - Found")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "order", "retrieve", "by-id")]
        public async Task GetByIdAsync_ShouldReturnOrder_WhenOrderExists()
        {
            // Arrange
            var orderId = 1;
            var order = new Order
            {
                Id = 1,
                UserId = 1,
                TotalPrice = 160000,
                Status = true,
                BookingDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _orderRepoMock.Setup(r => r.GetByIdAsync(orderId))
                .ReturnsAsync(order);

            // Act
            var result = await _orderService.GetByIdAsync(orderId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(orderId);
            result.UserId.Should().Be(1);
            result.TotalPrice.Should().Be(160000);
            _orderRepoMock.Verify(r => r.GetByIdAsync(orderId), Times.Once);
        }

        [Fact]
        [AllureFeature("Order Retrieval")]
        [AllureStory("Get Orders By User Id - Empty Result")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureTag("unit", "order", "retrieve", "by-user")]
        public async Task GetByUserIdAsync_ShouldReturnEmpty_WhenUserHasNoOrders()
        {
            // Arrange
            var userId = 1;
            _orderRepoMock.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(new List<Order>());

            // Act
            var result = await _orderService.GetByUserIdAsync(userId);

            // Assert
            result.Should().BeEmpty();
            _orderRepoMock.Verify(r => r.GetByUserIdAsync(userId), Times.Once);
        }

        [Fact]
        [AllureFeature("Order Retrieval")]
        [AllureStory("Get Orders By User Id - With Data")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureTag("unit", "order", "retrieve", "by-user")]
        public async Task GetByUserIdAsync_ShouldReturnOrders_WhenUserHasOrders()
        {
            // Arrange
            var userId = 1;
            var orders = new List<Order>
            {
                new Order { Id = 1, UserId = 1, TotalPrice = 160000, Status = true, BookingDate = DateOnly.FromDateTime(DateTime.Now) },
                new Order { Id = 2, UserId = 1, TotalPrice = 80000, Status = false, BookingDate = DateOnly.FromDateTime(DateTime.Now) }
            };

            _orderRepoMock.Setup(r => r.GetByUserIdAsync(userId))
                .ReturnsAsync(orders);

            // Act
            var result = await _orderService.GetByUserIdAsync(userId);

            // Assert
            result.Should().HaveCount(2);
            result.All(o => o.UserId == userId).Should().BeTrue();
            result.First().Id.Should().Be(1);
            result.Last().Id.Should().Be(2);
            _orderRepoMock.Verify(r => r.GetByUserIdAsync(userId), Times.Once);
        }

        [Fact]
        [AllureFeature("Order Retrieval")]
        [AllureStory("Get Paged Orders By User Id")]
        [AllureSeverity(SeverityLevel.minor)]
        [AllureTag("unit", "order", "retrieve", "paged")]
        public async Task GetPagedByUserIdAsync_ShouldReturnPagedOrders()
        {
            // Arrange
            var userId = 1;
            var page = 1;
            var pageSize = 5;
            var orders = new List<Order>
            {
                new Order { Id = 1, UserId = 1, TotalPrice = 160000, Status = true, BookingDate = DateOnly.FromDateTime(DateTime.Now) },
                new Order { Id = 2, UserId = 1, TotalPrice = 80000, Status = false, BookingDate = DateOnly.FromDateTime(DateTime.Now) }
            };
            var totalCount = 2;

            _orderRepoMock.Setup(r => r.GetPagedByUserIdAsync(userId, page, pageSize))
                .ReturnsAsync((orders, totalCount));

            // Act
            var result = await _orderService.GetPagedByUserIdAsync(userId, page, pageSize);

            // Assert
            result.Orders.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
            _orderRepoMock.Verify(r => r.GetPagedByUserIdAsync(userId, page, pageSize), Times.Once);
        }

        // -------------------------------
        // OrderService - Update & Delete
        // -------------------------------

        [Fact]
        [AllureFeature("Order Management")]
        [AllureStory("Update Order - Not Found")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "order", "update")]
        public async Task UpdateAsync_ShouldReturnNull_WhenOrderNotFound()
        {
            // Arrange
            var order = new Order { Id = 999, UserId = 1, TotalPrice = 160000, Status = false, BookingDate = DateOnly.FromDateTime(DateTime.Now) };
            _orderRepoMock.Setup(r => r.UpdateAsync(order))
                .ReturnsAsync((Order)null);

            // Act
            var result = await _orderService.UpdateAsync(order);

            // Assert
            result.Should().BeNull();
            _orderRepoMock.Verify(r => r.UpdateAsync(order), Times.Once);
        }

        [Fact]
        [AllureFeature("Order Management")]
        [AllureStory("Update Order - Success")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "order", "update")]
        public async Task UpdateAsync_ShouldUpdateOrderSuccessfully_WhenOrderExists()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                UserId = 1,
                TotalPrice = 160000,
                Status = true,
                BookingDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _orderRepoMock.Setup(r => r.UpdateAsync(It.IsAny<Order>()))
                .ReturnsAsync(order);

            // Act
            var result = await _orderService.UpdateAsync(order);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Status.Should().BeTrue();
            _orderRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Order>()), Times.Once);
        }

        [Fact]
        [AllureFeature("Order Management")]
        [AllureStory("Delete Order - Not Found")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "order", "delete")]
        public async Task DeleteAsync_ShouldReturnFalse_WhenOrderNotFound()
        {
            // Arrange
            var orderId = 999;
            _orderRepoMock.Setup(r => r.DeleteAsync(orderId))
                .ReturnsAsync(false);

            // Act
            var result = await _orderService.DeleteAsync(orderId);

            // Assert
            result.Should().BeFalse();
            _orderRepoMock.Verify(r => r.DeleteAsync(orderId), Times.Once);
        }

        [Fact]
        [AllureFeature("Order Management")]
        [AllureStory("Delete Order - Success")]
        [AllureSeverity(SeverityLevel.normal)]
        [AllureTag("unit", "order", "delete")]
        public async Task DeleteAsync_ShouldReturnTrue_WhenOrderDeleted()
        {
            // Arrange
            var orderId = 1;
            _orderRepoMock.Setup(r => r.DeleteAsync(orderId))
                .ReturnsAsync(true);

            // Act
            var result = await _orderService.DeleteAsync(orderId);

            // Assert
            result.Should().BeTrue();
            _orderRepoMock.Verify(r => r.DeleteAsync(orderId), Times.Once);
        }
    }
}