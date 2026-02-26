using Microsoft.EntityFrameworkCore;
using BookingManagement.Api.Repositories.IRepository;
using BookingManagement.Api.Services.IService;
using BookingManagement.Api.Models.DTOs.Seat;
using BookingManagement.Api.Models.Entities;
namespace BookingManagement.Api.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;

        public SeatService(ISeatRepository seatRepository)
        {
            _seatRepository = seatRepository;
        }

        public async Task<List<ListSeatDto>> GetAllSeatsAsync()
        {
            var seats = await _seatRepository.GetAllAsync();
            return seats.Select(s => new ListSeatDto
            {
                Id = s.Id,
                RoomId = s.RoomId,
                Row = s.Row,
                Column = s.Column,
                SeatType = s.SeatType
            }).ToList();
        }

        public async Task<List<ListSeatByRoomDto>> GetSeatsByRoomIdAsync(int roomId)
        {
            var seats = await _seatRepository.GetSeatsByRoomIdAsync(roomId);
            var room = seats.FirstOrDefault()?.Room; // Nếu entity Seat có navigation property Room
            return seats.OrderBy(s => s.Id).Select(s => new ListSeatByRoomDto
            {
                Id = s.Id,
                Row = s.Row,
                Column = s.Column,
                SeatType = s.SeatType
            }).ToList();
        }

        public async Task<List<ListSeatByRoomDto>> CreateSeatsAsync(int roomId, CreateSeatsDto request)
        {
            //Validate input
            if (request.RowCount < 1 || request.RowCount > 26)
                throw new ArgumentException("RowCount must be between 1 and 26 (A-Z)");
            if (request.ColumnCount < 1)
                throw new ArgumentException("ColumnCount minimum is 1");

            var seats = await _seatRepository.GetSeatsByRoomIdAsync(roomId);
            var existingSeatSet = new HashSet<string>(seats.Select(s => $"{s.Row}-{s.Column}"));
            var newSeats = new List<Seat>();
            //create seat, kiem tra neu co seat da ton tai thi chi them hang seat moi
            for (int i = 0; i < request.RowCount; i++)
            {
                char rowChar = (char)('A' + i);
                for (int col = 1; col <= request.ColumnCount; col++)
                {
                    string key = $"{rowChar}-{col}";
                    if (!existingSeatSet.Contains(key))
                    {
                        newSeats.Add(new Seat
                        {
                            RoomId = roomId,
                            Row = rowChar.ToString(),
                            Column = col,
                            SeatType = "Normal"
                        });
                    }
                }
            }
            if (newSeats.Any())
            {
                await _seatRepository.AddRangeAsync(newSeats);
                await _seatRepository.SaveChangesAsync();
            }
            //Return list seat sau khi create
            var updatedSeats = await _seatRepository.GetSeatsByRoomIdAsync(roomId);
            return updatedSeats.OrderBy(s => s.Id).Select(s => new ListSeatByRoomDto
            {
                Id = s.Id,
                Row = s.Row,
                Column = s.Column,
                SeatType = s.SeatType
            }).ToList();
        }

        // New method implementations for CRUD operations
        public async Task<GetSeatByIdDto?> GetSeatByIdAsync(int id)
        {
            var seat = await _seatRepository.GetByIdAsync(id);
            if (seat == null)
                return null;

            return new GetSeatByIdDto
            {
                Id = seat.Id,
                RoomId = seat.RoomId,
                Row = seat.Row,
                Column = seat.Column,
                SeatType = seat.SeatType,
                Status = (BookingManagement.Api.Models.DTOs.Seat.SeatStatus)seat.Status
            };
        }

        public async Task<CreateSeatResponse> CreateSeatsByRoomAsync(int roomId, CreateSeatByRoomRequest request)
        {
            // Validate input
            if (request.SeatQuantity <= 0)
                throw new ArgumentException("SeatQuantity must be greater than 0");

            const int maxSeatsPerRow = 10;
            var seatDistribution = CalculateSeatDistribution(request.SeatQuantity, maxSeatsPerRow);
            
            var newSeats = new List<Seat>();
            var currentRow = 'A';
            var existingSeats = await _seatRepository.GetSeatsByRoomIdAsync(roomId);
            
            // Find the next available row
            if (existingSeats.Any())
            {
                var maxRow = existingSeats.Max(s => s.Row);
                currentRow = (char)(maxRow[0] + 1);
            }

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

            return new CreateSeatResponse
            {
                RoomId = roomId,
                TotalSeatsCreated = newSeats.Count,
                SeatDistribution = seatDistribution,
                CreatedAt = DateTime.UtcNow
            };
        }

        public async Task<bool> UpdateSeatAsync(int id, UpdateSeatRequest request)
        {
            var seat = await _seatRepository.GetByIdAsync(id);
            if (seat == null)
                return false;

            seat.Row = request.Row;
            seat.Column = request.Column;
            seat.SeatType = request.SeatType;
            seat.Status = (BookingManagement.Api.Models.Entities.SeatStatus)request.Status;

            await _seatRepository.UpdateAsync(seat);
            return true;
        }

        public async Task<bool> DeleteSeatAsync(int id)
        {
            return await _seatRepository.DeleteAsync(id);
        }

        private List<SeatDistribution> CalculateSeatDistribution(int totalSeats, int maxSeatsPerRow)
        {
            var distribution = new List<SeatDistribution>();
            
            if (totalSeats <= maxSeatsPerRow)
            {
                distribution.Add(new SeatDistribution { Row = "A", SeatsInRow = totalSeats });
                return distribution;
            }

            // Calculate initial distribution
            var fullRows = totalSeats / maxSeatsPerRow;
            var remainingSeats = totalSeats % maxSeatsPerRow;

            // Add full rows
            for (int i = 0; i < fullRows; i++)
            {
                distribution.Add(new SeatDistribution 
                { 
                    Row = ((char)('A' + i)).ToString(), 
                    SeatsInRow = maxSeatsPerRow 
                });
            }

            // Handle remaining seats
            if (remainingSeats > 0)
            {
                if (remainingSeats < maxSeatsPerRow / 2)
                {
                    // Distribute remaining seats to existing rows from back to front
                    var seatsToDistribute = remainingSeats;
                    for (int i = distribution.Count - 1; i >= 0 && seatsToDistribute > 0; i--)
                    {
                        distribution[i].SeatsInRow++;
                        seatsToDistribute--;
                    }
                }
                else
                {
                    // Create a new row for remaining seats
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
}
