using Microsoft.EntityFrameworkCore;
using MovieManagement.Api.Models.DTO;
using MovieManagement.Api.Models.DTO.Admin;
using MovieManagement.Api.Models.DTO.Movie;
using MovieManagement.Api.Repositories.IRepositories;
using MovieManagement.Api.Services.IServices;


namespace MovieManagement.Api.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repo;
        public MovieService(IMovieRepository repo) => _repo = repo;
        
        //Search
        public async Task<List<UserMovieListDto>> SearchByNameAsync(string keyword)
        {
            var query = _repo.GetAll();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword.ToLower();
                query = query.Where(m => m.Name != null && m.Name.ToLower().Contains(kw));
            }

            // Sắp xếp theo tên A-Z
            query = query.OrderBy(m => m.Name);

            return await query.Select(m => new UserMovieListDto
            {
                Id = m.Id,
                Name = m.Name,
                ReleaseDate = m.ReleaseDate,
                Actors = m.Actors,
                Director = m.Director,
                RunningTime = m.RunningTime,
                Trailer = m.Trailer,
                Type = m.Type,
                ImagePath = m.ImagePath,
                Content = m.Content,
                Status = m.Status
            }).ToListAsync();
        }

        // Sort by Name A-Z
        public async Task<List<UserMovieListDto>> SortByNameAsync()
        {
            var query = _repo.GetAll().OrderBy(m => m.Name);

            return await query.Select(m => new UserMovieListDto
            {
                Id = m.Id,
                Name = m.Name,
                ReleaseDate = m.ReleaseDate,
                Actors = m.Actors,
                Director = m.Director,
                RunningTime = m.RunningTime,
                Trailer = m.Trailer,
                Type = m.Type,
                ImagePath = m.ImagePath,
                Content = m.Content,
                Status = m.Status
            }).ToListAsync();
        }

        //Filter by Type
        public async Task<List<UserMovieListDto>> FilterByTypeAsync(string type)
        {
            var now = DateTime.Now;
            var query = _repo.GetAll();

            if (type == "dang-chieu")
                query = query.Where(m => m.FromDate <= now && m.ToDate >= now);
            else if (type == "sap-chieu")
                query = query.Where(m => m.FromDate > now);
            // Nếu muốn mặc định trả hết, bỏ else đi

            query = query.OrderBy(m => m.Name); // Tuỳ ý, có thể order theo FromDate nếu muốn

            return await query.Select(m => new UserMovieListDto
            {
                Id = m.Id,
                Name = m.Name,
                ReleaseDate = m.ReleaseDate,
                Actors = m.Actors,
                Director = m.Director,
                RunningTime = m.RunningTime,
                Trailer = m.Trailer,
                Type = m.Type,
                ImagePath = m.ImagePath,
                Content = m.Content,
                Status = m.Status
            }).ToListAsync();
        }
        public async Task<MovieDurationDTO> GetDurationByMovieId(int movieId)
        {
            return await _repo.getDurationByMovieId(movieId);
        }

    }
}
