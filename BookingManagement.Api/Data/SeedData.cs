//using BookingManagement.Api.Data;
//using BookingManagement.Api.Models.Entities;
//using Microsoft.EntityFrameworkCore;

//namespace BookingManagement.Api.Data
//{
//    public static class SeedData
//    {
//        public static async Task Initialize(ShowtimeManagementDbContext context)
//        {
//            // CHỈ SEED NẾU CHƯA CÓ DATA
//            if (await context.Cinemas.AnyAsync())
//            {
//                Console.WriteLine("📊 Database đã có data, bỏ qua seed");
//                return;
//            }

//            Console.WriteLine("🎬 Database trống, bắt đầu seed data...");

//            try
//            {
//                // 1. Tạo cinemas
//                var cinemas = await CreateCinemas(context);

//                // 2. Tạo rooms
//                var rooms = await CreateRooms(context, cinemas);

//                // 3. Tạo seats
//                await CreateSeats(context, rooms);

//                // 4. Tạo showtimes
//                var showtimes = await CreateShowtimes(context, rooms);

//                // 5. Tạo ShowtimeSeats
//                await CreateShowtimeSeats(context, showtimes);

//                // 6. Tạo test bookings
//                await CreateTestBookings(context);

//                Console.WriteLine("✅ Seed data hoàn thành!");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"❌ Lỗi seed data: {ex.Message}");
//                Console.WriteLine(ex.StackTrace);
//            }
//        }

//        private static async Task<List<Cinema>> CreateCinemas(ShowtimeManagementDbContext context)
//        {
//            Console.WriteLine("🏢 Tạo cinemas...");

//            var cinemas = new List<Cinema>
//            {
//                new Cinema { Name = "CGV Vincom", Address = "72 Le Thanh Ton, Q1, HCMC" },
//                new Cinema { Name = "Galaxy Nguyen Du", Address = "116 Nguyen Du, Q1, HCMC" }
//            };

//            await context.Cinemas.AddRangeAsync(cinemas);
//            await context.SaveChangesAsync();

//            Console.WriteLine($"✅ Tạo {cinemas.Count} cinemas");
//            return cinemas;
//        }

//        private static async Task<List<Room>> CreateRooms(ShowtimeManagementDbContext context, List<Cinema> cinemas)
//        {
//            Console.WriteLine("🏠 Tạo rooms...");

//            var rooms = new List<Room>
//            {
//                new Room { Name = "Room A", CinemaId = cinemas[0].Id, SeatQuantity = 20 },
//                new Room { Name = "Room B", CinemaId = cinemas[0].Id, SeatQuantity = 30 },
//                new Room { Name = "Room C", CinemaId = cinemas[1].Id, SeatQuantity = 25 }
//            };

//            await context.Rooms.AddRangeAsync(rooms);
//            await context.SaveChangesAsync();

//            Console.WriteLine($"✅ Tạo {rooms.Count} rooms");
//            return rooms;
//        }

//        private static async Task CreateSeats(ShowtimeManagementDbContext context, List<Room> rooms)
//        {
//            Console.WriteLine("💺 Tạo seats...");

//            var allSeats = new List<Seat>();

//            foreach (var room in rooms)
//            {
//                int seatsPerRow = 5;
//                int numberOfRows = room.SeatQuantity / seatsPerRow;

//                for (int row = 0; row < numberOfRows; row++)
//                {
//                    char rowLetter = (char)('A' + row);

//                    for (int col = 1; col <= seatsPerRow; col++)
//                    {
//                        var seatType = (row == numberOfRows - 1) ? "VIP" : "Normal";

//                        allSeats.Add(new Seat
//                        {
//                            RoomId = room.Id,
//                            Row = rowLetter.ToString(),
//                            Column = col,
//                            SeatType = seatType,
//                            Status = SeatStatus.Active
//                        });
//                    }
//                }
//            }

//            await context.Seats.AddRangeAsync(allSeats);
//            await context.SaveChangesAsync();

//            Console.WriteLine($"✅ Tạo {allSeats.Count} seats");
//        }

//        private static async Task<List<Showtime>> CreateShowtimes(ShowtimeManagementDbContext context, List<Room> rooms)
//        {
//            Console.WriteLine("🎬 Tạo showtimes...");

//            var showtimes = new List<Showtime>();

//            for (int day = 0; day < 2; day++)
//            {
//                var date = DateTime.Today.AddDays(day);
//                var timeSlots = new[]
//                {
//                    new TimeSpan(14, 0, 0),  // 2PM
//                    new TimeSpan(17, 0, 0),  // 5PM  
//                    new TimeSpan(20, 0, 0)   // 8PM
//                };

//                foreach (var room in rooms)
//                {
//                    foreach (var timeSlot in timeSlots)
//                    {
//                        var startTime = date.Add(timeSlot);
//                        var movieId = Random.Shared.Next(1, 11);

//                        showtimes.Add(new Showtime
//                        {
//                            MovieId = movieId,
//                            RoomId = room.Id,
//                            StartTime = startTime
//                        });
//                    }
//                }
//            }

//            await context.Showtimes.AddRangeAsync(showtimes);
//            await context.SaveChangesAsync();

//            Console.WriteLine($"✅ Tạo {showtimes.Count} showtimes");
//            return showtimes;
//        }

//        private static async Task CreateShowtimeSeats(ShowtimeManagementDbContext context, List<Showtime> showtimes)
//        {
//            Console.WriteLine("🎫 Tạo ShowtimeSeats...");

//            // Xử lý từng batch để tránh OutOfMemory
//            int batchSize = 100;
//            int totalSeats = 0;

//            foreach (var showtime in showtimes)
//            {
//                var roomSeats = await context.Seats
//                    .Where(s => s.RoomId == showtime.RoomId)
//                    .ToListAsync();

//                var batch = new List<ShowtimeSeat>();

//                foreach (var seat in roomSeats)
//                {
//                    // FIX: Explicit set all nullable fields
//                    batch.Add(new ShowtimeSeat
//                    {
//                        ShowtimeId = showtime.Id,
//                        SeatId = seat.Id,
//                        Status = ShowtimeSeatStatus.Available,
//                        UserId = null,
//                        SessionId = null, // Explicit null
//                        ReservedAt = null,
//                        ExpiresAt = null
//                    });

//                    if (batch.Count >= batchSize)
//                    {
//                        await context.ShowtimeSeats.AddRangeAsync(batch);
//                        await context.SaveChangesAsync();
//                        totalSeats += batch.Count;
//                        batch.Clear();
//                    }
//                }

//                if (batch.Count > 0)
//                {
//                    await context.ShowtimeSeats.AddRangeAsync(batch);
//                    await context.SaveChangesAsync();
//                    totalSeats += batch.Count;
//                }
//            }

//            Console.WriteLine($"✅ Tạo {totalSeats} ShowtimeSeats");
//        }

//        private static async Task CreateTestBookings(ShowtimeManagementDbContext context)
//        {
//            Console.WriteLine("🎟️ Tạo test bookings...");

//            try
//            {
//                var firstShowtime = await context.Showtimes.FirstOrDefaultAsync();
//                if (firstShowtime == null)
//                {
//                    Console.WriteLine("❌ Không tìm thấy showtime để tạo booking");
//                    return;
//                }

//                var firstSeats = await context.Seats
//                    .Where(s => s.RoomId == firstShowtime.RoomId)
//                    .Take(3)
//                    .ToListAsync();

//                // Tạo ticket
//                var ticket = new Ticket
//                {
//                    UserId = 3,
//                    ShowtimeId = firstShowtime.Id,
//                    BookingTime = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified), // SỬA DÒNG NÀY
//                    Status = "Confirmed",
//                    SubTotal = null,
//                    DiscountAmount = null,
//                    TotalPrice = null,
//                    PromotionCode = null
//                };

//                await context.Tickets.AddAsync(ticket);
//                await context.SaveChangesAsync();

//                // Tạo TicketSeats
//                var ticketSeats = new List<TicketSeat>();
//                foreach (var seat in firstSeats)
//                {
//                    ticketSeats.Add(new TicketSeat
//                    {
//                        TicketId = ticket.Id,
//                        SeatId = seat.Id,
//                        Price = 80000
//                    });

//                    // Update ShowtimeSeat status
//                    var showtimeSeat = await context.ShowtimeSeats
//                        .FirstOrDefaultAsync(ss => ss.ShowtimeId == firstShowtime.Id && ss.SeatId == seat.Id);

//                    if (showtimeSeat != null)
//                    {
//                        showtimeSeat.Status = ShowtimeSeatStatus.Booked;
//                        showtimeSeat.UserId = ticket.UserId;
//                    }
//                }

//                await context.TicketSeats.AddRangeAsync(ticketSeats);
//                await context.SaveChangesAsync();

//                Console.WriteLine($"✅ Tạo 1 ticket với {ticketSeats.Count} seats");
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"❌ Lỗi tạo test bookings: {ex.Message}");
//            }
//        }
//    }
//}
