// Services/Service/RoomService.cs
using Microsoft.EntityFrameworkCore;
using BookingManagement.Api.Data;
using BookingManagement.Api.Services.Service;
using BookingManagement.Api.Models.DTOs.Seat;
using BookingManagement.Api.Models.DTOs.Room;
using BookingManagement.Api.Models.Entities;
using BookingManagement.Api.Repositories.IRepository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace BookingManagement.Api.Services.Service
{
    public class RoomService : IRoomService
    {
        //private readonly ShowtimeManagementDbContext _context;
        //private readonly ILogger<RoomService> _logger;
        //private readonly string[] _validSeatTypes = { "Normal", "VIP", "Couple" };

        //public RoomService(ShowtimeManagementDbContext context, ILogger<RoomService> logger, IRoomRepository roomRepository)
        //{
        //    _context = context;
        //    _logger = logger;
        //    _roomRepository = roomRepository;
        //}

        //public async Task<bool> UpdateSeatsTypeAsync(int roomId, List<SeatUpdateDto> seatUpdates)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();

        //    try
        //    {
        //        // Validate room exists
        //        if (!await RoomExistsAsync(roomId))
        //        {
        //            _logger.LogWarning("Room with ID {RoomId} not found", roomId);
        //            return false;
        //        }

        //        // Validate seats belong to room
        //        var seatIds = seatUpdates.Select(s => s.SeatId).ToList();
        //        var validationErrors = await ValidateSeatsInRoomAsync(roomId, seatIds);

        //        if (validationErrors.Any())
        //        {
        //            _logger.LogWarning("Validation failed for room {RoomId}: {Errors}",
        //                roomId, string.Join(", ", validationErrors));
        //            return false;
        //        }

        //        // Get seats to update
        //        var seats = await _context.Seats
        //            .Where(s => s.RoomId == roomId && seatIds.Contains(s.Id))
        //            .ToListAsync();

        //        // Update seat types
        //        foreach (var seatUpdate in seatUpdates)
        //        {
        //            var seat = seats.FirstOrDefault(s => s.Id == seatUpdate.SeatId);
        //            if (seat != null && _validSeatTypes.Contains(seatUpdate.SeatType))
        //            {
        //                seat.SeatType = seatUpdate.SeatType;
        //                _logger.LogDebug("Updated seat {SeatId} to type {SeatType}",
        //                    seat.Id, seatUpdate.SeatType);
        //            }
        //        }

        //        await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        _logger.LogInformation("Successfully updated {Count} seats in room {RoomId}",
        //            seatUpdates.Count, roomId);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        _logger.LogError(ex, "Error updating seats in room {RoomId}", roomId);
        //        return false;
        //    }
        //}

        //public async Task<bool> RoomExistsAsync(int roomId)
        //{
        //    return await _context.Rooms.AnyAsync(r => r.Id == roomId);
        //}

        //public async Task<List<string>> ValidateSeatsInRoomAsync(int roomId, List<int> seatIds)
        //{
        //    var errors = new List<string>();

        //    var existingSeats = await _context.Seats
        //        .Where(s => s.RoomId == roomId && seatIds.Contains(s.Id))
        //        .Select(s => s.Id)
        //        .ToListAsync();

        //    var missingSeatIds = seatIds.Except(existingSeats).ToList();

        //    if (missingSeatIds.Any())
        //    {
        //        errors.Add($"Seats not found in room: {string.Join(", ", missingSeatIds)}");
        //    }

        //    return errors;
        //}

        //CRUD operations for Room
        private readonly IRoomRepository _roomRepository;
        private readonly ISeatRepository _seatRepository;
        public RoomService(IRoomRepository roomRepository, ISeatRepository seatRepository)
        {
            _roomRepository = roomRepository;
            _seatRepository = seatRepository;
        }

        public async Task<IEnumerable<RoomDto>> GetAllAsync()
        {
            var rooms = await _roomRepository.GetAllAsync();
            return rooms.Select(r => new RoomDto { Id = r.Id, Name = r.Name, CinemaId = r.CinemaId, SeatQuantity = r.SeatQuantity });
        }

        public async Task<RoomDto?> GetByIdAsync(int id)
        {
            var r = await _roomRepository.GetByIdAsync(id);
            if (r == null) return null;
            return new RoomDto { Id = r.Id, Name = r.Name, CinemaId = r.CinemaId, SeatQuantity = r.SeatQuantity };
        }

        public async Task<RoomDto> CreateAsync(CreateRoomRequest request)
        {
            var room = new Room { Name = request.Name, CinemaId = request.CinemaId, SeatQuantity = request.SeatQuantity };
            
            // Ensure ID is not set manually - let database auto-generate
            room.Id = 0;
            
            var created = await _roomRepository.AddAsync(room);
            return new RoomDto { Id = created.Id, Name = created.Name, CinemaId = created.CinemaId, SeatQuantity = created.SeatQuantity };
        }

        public async Task<bool> UpdateAsync(int id, UpdateRoomRequest request)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            if (room == null) return false;
            room.Name = request.Name;
            room.CinemaId = request.CinemaId;
            room.SeatQuantity = request.SeatQuantity;
            return await _roomRepository.UpdateAsync(room);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _roomRepository.DeleteAsync(id);
        }

        public async Task<RoomDto> CreateRoomWithSeatsAsync(CreateRoomWithSeatsRequest request)
        {
            var room = new Room
            {
                Name = request.Name,
                SeatQuantity = request.SeatQuantity,
                CinemaId = 1
            };
            
            // Ensure ID is not set manually - let database auto-generate
            //room.Id = 0;
            
            var created = await _roomRepository.AddAsync(room);
            if (request.SeatQuantity > 0)
            {
                await CreateSeatsForRoom(created.Id, request.SeatQuantity);
            }
            return new RoomDto
            {
                Id = created.Id,
                Name = created.Name,
                CinemaId = created.CinemaId,
                SeatQuantity = created.SeatQuantity
            };
        }

        private async Task CreateSeatsForRoom(int roomId, int seatQuantity)
        {
            const int maxSeatsPerRow = 10;
            var seatDistribution = CalculateSeatDistribution(seatQuantity, maxSeatsPerRow);
            var newSeats = new List<Seat>();
            var currentRow = 'A';
            foreach (var distribution in seatDistribution)
            {
                for (int col = 1; col <= distribution.SeatsInRow; col++)
                {
                    newSeats.Add(new Seat
                    {
                        RoomId = roomId,
                        Row = currentRow.ToString(),
                        Column = col,
                        SeatType = "Normal",
                        Status = BookingManagement.Api.Models.Entities.SeatStatus.Active
                    });
                }
                currentRow = (char)(currentRow + 1);
            }
            if (newSeats.Any())
            {
                await _seatRepository.AddRangeAsync(newSeats);
                await _seatRepository.SaveChangesAsync();
            }
        }

        private List<SeatDistribution> CalculateSeatDistribution(int totalSeats, int maxSeatsPerRow)
        {
            var distribution = new List<SeatDistribution>();

            if (totalSeats <= maxSeatsPerRow)
            {
                distribution.Add(new SeatDistribution { Row = "A", SeatsInRow = totalSeats });
                return distribution;
            }

            int fullRows = totalSeats / maxSeatsPerRow;
            int remainingSeats = totalSeats % maxSeatsPerRow;

            for (int i = 0; i < fullRows; i++)
            {
                distribution.Add(new SeatDistribution
                {
                    Row = ((char)('A' + i)).ToString(),
                    SeatsInRow = maxSeatsPerRow
                });
            }

            if (remainingSeats > 0)
            {
                if (remainingSeats < maxSeatsPerRow / 2)
                {
                    //Phân bổ đều ghế dư từ hàng cuối cùng trở ngược lên
                    int idx = distribution.Count - 1;
                    while (remainingSeats > 0)
                    {
                        distribution[idx].SeatsInRow++;
                        idx--;
                        if (idx < 0)
                            idx = distribution.Count - 1;
                        remainingSeats--;
                    }
                }
                else
                {
                    //Tạo thêm hàng mới nếu số ghế dư nhiều
                    distribution.Add(new SeatDistribution
                    {
                        Row = ((char)('A' + fullRows)).ToString(),
                        SeatsInRow = remainingSeats
                    });
                }
            }

            return distribution;
        }

    }

    public class SeatDistribution
    {
        public string Row { get; set; } = string.Empty;
        public int SeatsInRow { get; set; }
    }
}