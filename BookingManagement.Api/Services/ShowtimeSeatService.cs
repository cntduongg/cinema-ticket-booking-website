using BookingManagement.Api.Models.DTOs.ShowtimeSeat;
using BookingManagement.Api.Models.Entities;
using BookingManagement.Api.Repositories;
using BookingManagement.Api.Services.IService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Api.Services.Service
{
    public class ShowtimeSeatService : IShowtimeSeatService
    {
        //        private readonly IShowtimeSeatRepository _showtimeSeatRepository;
        //        private readonly IShowtimeRepository _showtimeRepository;
        //        private readonly ILogger<ShowtimeSeatService> _logger;

        //        public ShowtimeSeatService(
        //            IShowtimeSeatRepository showtimeSeatRepository,
        //            IShowtimeRepository showtimeRepository,
        //            ILogger<ShowtimeSeatService> logger)
        //        {
        //            _showtimeSeatRepository = showtimeSeatRepository;
        //            _showtimeRepository = showtimeRepository;
        //            _logger = logger;
        //        }

        //        public async Task<List<ShowtimeSeatDto>> GetAllAsync()
        //        {
        //            try
        //            {
        //                var showtimeSeats = await _showtimeSeatRepository.GetAllAsync();
        //                return showtimeSeats.Select(MapToDto).ToList();
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error getting all showtime seats");
        //                throw;
        //            }
        //        }

        //        public async Task<List<ShowtimeSeatDto>> GetByShowtimeIdAsync(int showtimeId)
        //        {
        //            try
        //            {
        //                var showtime = await _showtimeRepository.GetByIdAsync(showtimeId);
        //                if (showtime == null)
        //                {
        //                    throw new ArgumentException($"Không tìm thấy suất chiếu với ID {showtimeId}");
        //                }

        //                var showtimeSeats = await _showtimeSeatRepository.GetByShowtimeIdAsync(showtimeId);
        //                return showtimeSeats.Select(MapToDto).ToList();
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error getting showtime seats for showtime {ShowtimeId}", showtimeId);
        //                throw;
        //            }
        //        }

        //        public async Task<ShowtimeSeatDto> GetByShowtimeAndSeatIdAsync(int showtimeId, int seatId)
        //        {
        //            try
        //            {
        //                var showtimeSeat = await _showtimeSeatRepository.GetByShowtimeAndSeatIdAsync(showtimeId, seatId);
        //                if (showtimeSeat == null)
        //                {
        //                    throw new ArgumentException($"Không tìm thấy thông tin ghế với ShowtimeId {showtimeId} và SeatId {seatId}");
        //                }

        //                return MapToDto(showtimeSeat);
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error getting showtime seat for showtime {ShowtimeId}, seat {SeatId}", showtimeId, seatId);
        //                throw;
        //            }
        //        }

        //        public async Task<ShowtimeSeatDto> CreateAsync(CreateShowtimeSeatDto dto)
        //        {
        //            try
        //            {
        //                // Kiểm tra showtime tồn tại
        //                var showtime = await _showtimeRepository.GetShowtimeWithRoomAndSeatsAsync(dto.ShowtimeId);
        //                if (showtime == null)
        //                {
        //                    throw new ArgumentException($"Không tìm thấy suất chiếu với ID {dto.ShowtimeId}");
        //                }

        //                // Kiểm tra seat thuộc về room của showtime
        //                var seat = showtime.Room.Seats.FirstOrDefault(s => s.Id == dto.SeatId);
        //                if (seat == null)
        //                {
        //                    throw new ArgumentException($"Ghế với ID {dto.SeatId} không thuộc về phòng chiếu của suất chiếu này");
        //                }

        //                // Tạo ShowtimeSeat
        //                var showtimeSeat = new ShowtimeSeat
        //                {
        //                    ShowtimeId = dto.ShowtimeId,
        //                    SeatId = dto.SeatId,
        //                    Status = dto.Status,
        //                    UserId = dto.UserId,
        //                    SessionId = dto.SessionId,
        //                    ReservedAt = dto.ReservedAt ?? DateTime.Now,
        //                    ExpiresAt = dto.ExpiresAt ?? DateTime.Now.AddMinutes(15) // Mặc định 15 phút
        //                };

        //                await _showtimeSeatRepository.CreateAsync(showtimeSeat);

        //                // Lấy thông tin mới nhất
        //                var createdShowtimeSeat = await _showtimeSeatRepository.GetByShowtimeAndSeatIdAsync(dto.ShowtimeId, dto.SeatId);
        //                return MapToDto(createdShowtimeSeat);
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error creating showtime seat for showtime {ShowtimeId}, seat {SeatId}", dto.ShowtimeId, dto.SeatId);
        //                throw;
        //            }
        //        }

        //        public async Task<ShowtimeSeatDto> UpdateAsync(int showtimeId, int seatId, UpdateShowtimeSeatDto dto)
        //        {
        //            try
        //            {
        //                // Kiểm tra ShowtimeSeat tồn tại
        //                var showtimeSeat = await _showtimeSeatRepository.GetByShowtimeAndSeatIdAsync(showtimeId, seatId);
        //                if (showtimeSeat == null)
        //                {
        //                    throw new ArgumentException($"Không tìm thấy thông tin ghế với ShowtimeId {showtimeId} và SeatId {seatId}");
        //                }

        //                // Cập nhật thông tin
        //                showtimeSeat.Status = dto.Status;
        //                showtimeSeat.UserId = dto.UserId;
        //                showtimeSeat.SessionId = dto.SessionId;
        //                showtimeSeat.ReservedAt = dto.ReservedAt ?? showtimeSeat.ReservedAt;
        //                showtimeSeat.ExpiresAt = dto.ExpiresAt ?? showtimeSeat.ExpiresAt;

        //                await _showtimeSeatRepository.UpdateAsync(showtimeSeat);

        //                // Lấy thông tin mới nhất
        //                var updatedShowtimeSeat = await _showtimeSeatRepository.GetByShowtimeAndSeatIdAsync(showtimeId, seatId);
        //                return MapToDto(updatedShowtimeSeat);
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error updating showtime seat for showtime {ShowtimeId}, seat {SeatId}", showtimeId, seatId);
        //                throw;
        //            }
        //        }

        //        public async Task DeleteAsync(int showtimeId, int seatId)
        //        {
        //            try
        //            {
        //                // Kiểm tra ShowtimeSeat tồn tại
        //                var showtimeSeat = await _showtimeSeatRepository.GetByShowtimeAndSeatIdAsync(showtimeId, seatId);
        //                if (showtimeSeat == null)
        //                {
        //                    throw new ArgumentException($"Không tìm thấy thông tin ghế với ShowtimeId {showtimeId} và SeatId {seatId}");
        //                }

        //                await _showtimeSeatRepository.DeleteAsync(showtimeId, seatId);
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError(ex, "Error deleting showtime seat for showtime {ShowtimeId}, seat {SeatId}", showtimeId, seatId);
        //                throw;
        //            }
        //        }

        //        private ShowtimeSeatDto MapToDto(ShowtimeSeat showtimeSeat)
        //        {
        //            return new ShowtimeSeatDto
        //            {
        //                ShowtimeId = showtimeSeat.ShowtimeId,
        //                SeatId = showtimeSeat.SeatId,
        //                Status = showtimeSeat.Status,
        //                UserId = showtimeSeat.UserId,
        //                SessionId = showtimeSeat.SessionId,
        //                ReservedAt = showtimeSeat.ReservedAt,
        //                ExpiresAt = showtimeSeat.ExpiresAt,
        //                SeatRow = showtimeSeat.Seat?.Row,
        //                SeatColumn = showtimeSeat.Seat?.Column ?? 0,
        //                SeatType = showtimeSeat.Seat?.SeatType
        //            };
        //        }

        private readonly IShowtimeSeatRepository _repo;
        public ShowtimeSeatService(IShowtimeSeatRepository repo) => _repo = repo;

        public async Task<IEnumerable<ShowtimeSeatDto>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return list.Select(s => new ShowtimeSeatDto
            {
                ShowtimeId = s.ShowtimeId,
                SeatId = s.SeatId,
                Status = s.Status.ToString(),
                UserId = s.UserId,
                SessionId = s.SessionId,
                ReservedAt = s.ReservedAt,
                ExpiresAt = s.ExpiresAt
            });
        }

        public async Task<ShowtimeSeatDto?> GetByIdAsync(int showtimeId, int seatId)
        {
            var s = await _repo.GetByIdAsync(showtimeId, seatId);
            if (s == null) return null;
            return new ShowtimeSeatDto
            {
                ShowtimeId = s.ShowtimeId,
                SeatId = s.SeatId,
                Status = s.Status.ToString(),
                UserId = s.UserId,
                SessionId = s.SessionId,
                ReservedAt = s.ReservedAt,
                ExpiresAt = s.ExpiresAt
            };
        }

        public async Task<ShowtimeSeatDto> CreateAsync(CreateShowtimeSeatDto dto)
        {
            var entity = new ShowtimeSeat
            {
                ShowtimeId = dto.ShowtimeId,
                SeatId = dto.SeatId,
                Status = Enum.Parse<ShowtimeSeatStatus>(dto.Status),
                UserId = dto.UserId,
                SessionId = dto.SessionId,
                ReservedAt = dto.ReservedAt,
                ExpiresAt = dto.ExpiresAt
            };
            var result = await _repo.AddAsync(entity);
            return new ShowtimeSeatDto
            {
                ShowtimeId = result.ShowtimeId,
                SeatId = result.SeatId,
                Status = result.Status.ToString(),
                UserId = result.UserId,
                SessionId = result.SessionId,
                ReservedAt = result.ReservedAt,
                ExpiresAt = result.ExpiresAt
            };
        }

        public async Task<bool> UpdateAsync(int showtimeId, int seatId, CreateShowtimeSeatDto dto)
        {
            var entity = await _repo.GetByIdAsync(showtimeId, seatId);
            if (entity == null) return false;
            entity.Status = Enum.Parse<ShowtimeSeatStatus>(dto.Status);
            entity.UserId = dto.UserId;
            entity.SessionId = dto.SessionId;
            entity.ReservedAt = dto.ReservedAt;
            entity.ExpiresAt = dto.ExpiresAt;
            await _repo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(int showtimeId, int seatId)
        {
            var entity = await _repo.GetByIdAsync(showtimeId, seatId);
            if (entity == null) return false;
            await _repo.DeleteAsync(showtimeId, seatId);
            return true;
        }
    }
}
