using BookingService.Models;
using BookingService.Models.DTOs;
using BookingService.Repositories;


namespace BookingService.Services
{
    public class CinemaRoomService : ICinemaRoomService
    {
        private readonly ICinemaRoomRepository _repo;
        private readonly ISeatRepository _seatRepo;


        public CinemaRoomService(ICinemaRoomRepository repo, ISeatRepository seatRepo)
        {
            _repo = repo;
            _seatRepo = seatRepo;
        }


        public async Task<IEnumerable<CinemaRoom>> GetAllAsync()
        {
            var rooms = await _repo.GetAllAsync();
            return rooms.Select(r => new CinemaRoom
            {
                Id = r.Id,
                CinemaRoomName = r.CinemaRoomName,
                SeatQuantity = r.SeatQuantity,
                Status = r.Status
            });
        }

        public async Task<CinemaRoom?> GetByIdAsync(int id)
        {
            var r = await _repo.GetByIdAsync(id);
            if (r == null) return null;
            return new CinemaRoom
            {
                Id = r.Id,
                CinemaRoomName = r.CinemaRoomName,
                SeatQuantity = r.SeatQuantity,
                Status = r.Status
            };
        }

        //Tạo phòng kèm tạo ghế
        public async Task CreateAsync(CinemaRoom room)
        {
            // Check duplicate name
            var existingRoom = (await _repo.GetAllAsync())
        .FirstOrDefault(r => r.CinemaRoomName.Trim().ToLower() == room.CinemaRoomName.Trim().ToLower());

            if (existingRoom != null)
            {
                throw new InvalidOperationException($"Tên phòng '{room.CinemaRoomName}' đã tồn tại.");
            }
            //

            await _repo.AddAsync(room);

            //Tạo ghế kèm cho phòng chiếu
            int SeatRow = 10;
            int RowCount = (int)Math.Ceiling((double)room.SeatQuantity / SeatRow);
            var seats = new List<Seat>();
            for (int row = 0; row < RowCount; row++)
            {
                char seatRow = (char)('A' + row);
                for (int col = 0; col < SeatRow; col++)
                {
                    int seatNumber = (row * SeatRow) + col; //Tính số ghế
                    if (seatNumber > room.SeatQuantity) break;

                    seats.Add(new Seat
                    {
                        CinemaRoomId = room.Id,
                        SeatRow = seatRow,
                        SeatColumn = (char)('0' + col), //Chuyển number (cột) sang string
                        SeatStatus = true
                    });
                }
            }
            //Lưu vào DB
            await _seatRepo.AddRangeAsync(seats);
        }

        //public async Task<CinemaRoom?> UpdateAsync( CinemaRoom dto,int id)
        //{
        //    var existing = await _repo.GetByIdAsync(dto.Id);
        //    if (existing == null) return null;

        //    //ktre xem có cập nhật số ghế hay không
        //    bool seatQuantityChanged = existing.SeatQuantity != dto.SeatQuantity;

        //    existing.CinemaRoomName = dto.CinemaRoomName;
        //    existing.SeatQuantity = dto.SeatQuantity;
        //    existing.Status = dto.Status;

        //    var updated = await _repo.UpdateAsync(existing,id);

        //    return updated;

        //}

        public async Task<CinemaRoom?> UpdateAsync(CinemaRoom dto, int id)
        {
            var existing = await _repo.GetByIdAsync(dto.Id);
            if (existing == null) return null;

            //check duplicate name
            var duplicateRoom = (await _repo.GetAllAsync())
        .FirstOrDefault(r => r.Id != existing.Id &&
                             r.CinemaRoomName.Trim().ToLower() == dto.CinemaRoomName.Trim().ToLower());

            if (duplicateRoom != null)
            {
                throw new InvalidOperationException($"Tên phòng '{dto.CinemaRoomName}' đã tồn tại.");
            }
            //

            int seatPerRow = 10;

            //tính index ghế (để thao tác với index)
            int SeatIndex(Seat s) => (s.SeatRow - 'A') * seatPerRow + (s.SeatColumn - '0');

            // Lấy toàn bộ ghế hiện tại của phòng này và sắp xếp theo index
            var seats = (await _seatRepo.GetByRoomIdAsync(existing.Id))
                .OrderBy(SeatIndex)
                .ToList();

            int newSeatQuantity = dto.SeatQuantity;
            int activeSeatQuantity = seats.Count(s => s.SeatStatus == true);

            existing.CinemaRoomName = dto.CinemaRoomName;
            existing.SeatQuantity = dto.SeatQuantity;
            existing.Status = dto.Status;

            if (newSeatQuantity > activeSeatQuantity)
            {
                var activeSeats = seats.Where(s => s.SeatStatus == true).OrderBy(SeatIndex).ToList();
                var softDeletedSeats = seats.Where(s => s.SeatStatus == false).OrderBy(SeatIndex).ToList();

                int totalExistingSeats = activeSeats.Count + softDeletedSeats.Count;
                int needToActivate = newSeatQuantity - activeSeats.Count;

                if (newSeatQuantity > totalExistingSeats)
                {
                    // Active tất cả ghế xoá mềm
                    foreach (var seat in softDeletedSeats)
                    {
                        seat.SeatStatus = true;
                        await _seatRepo.UpdateAsync(seat);
                    }

                    int stillNeed = newSeatQuantity - totalExistingSeats;
                    if (stillNeed > 0)
                    {
                        // Tạo thêm ghế mới nếu còn thiếu
                        seats = await _seatRepo.GetByRoomIdAsync(existing.Id);
                        var existingSeatPositions = new HashSet<(char, char)>(
                            seats.Select(s => (s.SeatRow, s.SeatColumn))
                        );
                        var newSeats = new List<Seat>();
                        int created = 0;
                        int rowCount = (int)Math.Ceiling((double)newSeatQuantity / seatPerRow);

                        for (int row = 0; row < rowCount; row++)
                        {
                            char seatRow = (char)('A' + row);
                            for (int col = 0; col < seatPerRow; col++)
                            {
                                int seatNumber = row * seatPerRow + col;
                                if (seatNumber >= newSeatQuantity) break;

                                var pos = (seatRow, (char)('0' + col));
                                if (existingSeatPositions.Contains(pos)) continue;

                                newSeats.Add(new Seat
                                {
                                    CinemaRoomId = existing.Id,
                                    SeatRow = seatRow,
                                    SeatColumn = (char)('0' + col),
                                    SeatStatus = true
                                });

                                created++;
                                if (created >= stillNeed) break;
                            }
                            if (created >= stillNeed) break;
                        }
                        if (newSeats.Count > 0)
                            await _seatRepo.AddRangeAsync(newSeats);
                    }
                }
                else
                {
                    // Nếu total seat đã đủ thì chỉ cần active seat đã xoá mềm
                    foreach (var seat in softDeletedSeats.Take(needToActivate))
                    {
                        seat.SeatStatus = true;
                        await _seatRepo.UpdateAsync(seat);
                    }
                }
            }
            else if (newSeatQuantity < activeSeatQuantity)
            {
                // Xoá mềm các ghế dư (theo index lớn nhất trước)
                var seatsToDisable = seats
                    .Where(s => s.SeatStatus == true)
                    .OrderByDescending(SeatIndex)
                    .Take(activeSeatQuantity - newSeatQuantity)
                    .ToList();
                foreach (var seat in seatsToDisable)
                {
                    seat.SeatStatus = false;
                    await _seatRepo.UpdateAsync(seat);
                }
            }
            // Nếu bằng thì không cần update
            var updated = await _repo.UpdateAsync(existing, id);
            return updated;
        }

        public async Task<bool> DeleteAsync(int id)
            => await _repo.DeleteAsync(id);
        public async Task<PagingResult<CinemaRoomResponse>> GetPagedAsync(PagingRequest request)
        {
            var pagedData = await _repo.GetPagedCinemaRoomAsync(request.PageIndex, request.PageSize);

            var items = pagedData.Items.Select(r => new CinemaRoomResponse
            {
                Id = r.Id,
                CinemaRoomName = r.CinemaRoomName,
                SeatQuantity = r.SeatQuantity,
                Status = r.Status
            }).ToList();

            return new PagingResult<CinemaRoomResponse>(items, pagedData.TotalRecords, request.PageIndex, request.PageSize);
        }
    }
}
