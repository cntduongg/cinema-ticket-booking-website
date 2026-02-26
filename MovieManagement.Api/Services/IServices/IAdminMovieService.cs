using MovieManagement.Api.Models.DTO;
using MovieManagement.Api.Models.DTO.Admin;

namespace MovieManagement.Api.Services.IServices
{
    public interface IAdminMovieService
    {
        Task<IEnumerable<MovieListDto>> GetAllMoviesAsync();
        Task<IEnumerable<AdminMovieListDto>> GetAllMoviesAdminAsync();
        Task<Movie?> GetByIdAsync(int id);
        Task<Movie?> GetWithTrackingByIdAsync(int id);
        Task AddAsync(Movie movie);
        Task UpdateAsync(Movie movie);
        Task SaveChangesAsync();
        Task AddMovieFromDtoAsync(MovieCreateDto dto);
        Task UpdateMovieFromDtoAsync(int id, MovieUpdateDto dto);
        Task<bool> ToggleStatusAsync(int id);
    }
}
