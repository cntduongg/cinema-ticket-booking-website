using Microsoft.EntityFrameworkCore;
using MovieManagement.Api.Models.DTO;
using MovieManagement.Api.Models.DTO.Admin;
using MovieManagement.Api.Services.IServices;
using MovieManagement.Data;

namespace MovieManagement.Api.Services
{
    public class AdminMovieService : IAdminMovieService
    {
        private readonly AppDbContext _context;

        public AdminMovieService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovieListDto>> GetAllMoviesAsync()
        {
            return await _context.Movies
                .AsNoTracking()               
                .Select(m => new MovieListDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    ReleaseDate = m.ReleaseDate,
                    ProductionCompany = m.ProductionCompany,
                    RunningTime = m.RunningTime,
                    Version = m.Version,
                    ImagePath = m.ImagePath,    // ← Thêm field này
                    Trailer = m.Trailer,        // ← Thêm field này
                    Content = m.Content,        // ← Thêm field này
                    Type = m.Type,              // ← Thêm field này
                    Status = m.Status           // ← Thêm field này
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AdminMovieListDto>> GetAllMoviesAdminAsync()
        {
            return await _context.Movies
                .AsNoTracking()
                .Where(m => m.Status == true)
                .Select(m => new AdminMovieListDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    ReleaseDate = m.ReleaseDate,
                    ProductionCompany = m.ProductionCompany,
                    RunningTime = m.RunningTime,
                    Version = m.Version,
                })
                .ToListAsync();
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await _context.Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Movie?> GetWithTrackingByIdAsync(int id)
        {
            return await _context.Movies
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
        }

        public async Task UpdateAsync(Movie movie)
        {
            _context.Movies.Update(movie);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddMovieFromDtoAsync(MovieCreateDto dto)
        {
            var movie = new Movie
            {
                Name = dto.Name,
                ReleaseDate = dto.ReleaseDate,
                FromDate = dto.FromDate,
                ToDate = dto.ToDate,
                Actors = dto.Actors,
                ProductionCompany = dto.ProductionCompany,
                Director = dto.Director,
                RunningTime = dto.RunningTime,
                Version = dto.Version,
                Trailer = dto.Trailer,
                Type = dto.Type,
                Content = dto.Content,
                ImagePath = dto.ImagePath,
                CreatedById = dto.CreatedById,
                CreatedDate = dto.CreatedDate,
                Status = true
            };

            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMovieFromDtoAsync(int id, MovieUpdateDto dto)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return;

            movie.Name = dto.Name;
            movie.ReleaseDate = dto.ReleaseDate;
            movie.FromDate = dto.FromDate;
            movie.ToDate = dto.ToDate;
            movie.Actors = dto.Actors;
            movie.ProductionCompany = dto.ProductionCompany;
            movie.Director = dto.Director;
            movie.RunningTime = dto.RunningTime;
            movie.Version = dto.Version;
            movie.Trailer = dto.Trailer;
            movie.Type = dto.Type;
            movie.Content = dto.Content;
            movie.ImagePath = dto.ImagePath;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ToggleStatusAsync(int id)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null) return false;

            movie.Status = !movie.Status;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
